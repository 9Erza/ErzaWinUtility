using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;

namespace ErzaWinUtility.Services
{
    /// <summary>
    /// Service responsible for system-level modifications via Windows Registry and Shell commands.
    /// All methods assume Administrative privileges via app.manifest.
    /// </summary>
    public static class RegistryService
    {
        // Registry Paths
        private const string CrashControlPath = @"System\CurrentControlSet\Control\CrashControl";
        private const string HibernatePath = @"System\CurrentControlSet\Control\Power";
        private const string SystemRestorePath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore";
        private const string CoreIsolationPath = @"SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity";
        private const string ExplorerAdvancedPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced";
        private const string TelemetryPath = @"SOFTWARE\Policies\Microsoft\Windows\DataCollection";

        // ============================================================
        // SECURITY & SYSTEM
        // ============================================================

        public static bool IsDetailedBsodEnabled() => GetDword(Registry.LocalMachine, CrashControlPath, "DisplayParameters") == 1;
        public static void SetDetailedBsod(bool enable) => SetDword(Registry.LocalMachine, CrashControlPath, "DisplayParameters", enable ? 1 : 0);

        public static bool IsHibernationEnabled() => GetDword(Registry.LocalMachine, HibernatePath, "HibernateEnabled") == 1;
        public static void SetHibernation(bool enable) => RunProcess("powercfg.exe", enable ? "/hibernate on" : "/hibernate off");

        public static bool IsSystemProtectionEnabled() => Registry.LocalMachine.OpenSubKey(SystemRestorePath)?.GetValue("RPSessionInterval") != null;
        public static void SetSystemProtection(bool enable) => RunPowerShell(enable ? "Enable-ComputerRestore -Drive 'C:\\'" : "Disable-ComputerRestore -Drive 'C:\\'");

        public static bool IsCoreIsolationEnabled() => GetDword(Registry.LocalMachine, CoreIsolationPath, "Enabled") == 1;
        public static void SetCoreIsolation(bool enable) => SetDword(Registry.LocalMachine, CoreIsolationPath, "Enabled", enable ? 1 : 0);

        // ============================================================
        // OPTIMIZATION ENGINE (OptimizeView)
        // ============================================================

        /// <summary>
        /// Creates a Windows System Restore Point.
        /// </summary>
        public static void CreateRestorePoint(string description)
        {
            // We use environment variable to handle spaces in description safely
            Environment.SetEnvironmentVariable("RESTORE_DESC", description);

            string script = "Set-ItemProperty -Path 'HKLM:\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\SystemRestore' -Name 'SystemRestorePointCreationFrequency' -Value 0 -Force; " +
                            "Checkpoint-Computer -Description $env:RESTORE_DESC -RestorePointType MODIFY_SETTINGS -ErrorAction SilentlyContinue";

            RunPowerShellSync(script);
        }

        /// <summary>
        /// Cleans temporary files from User and System directories.
        /// </summary>
        public static void CleanupTempFiles() => RunPowerShell("Remove-Item -Path $env:TEMP\\* -Recurse -Force -ErrorAction SilentlyContinue; Remove-Item -Path C:\\Windows\\Temp\\* -Recurse -Force -ErrorAction SilentlyContinue");

        /// <summary>
        /// Launches the native Disk Cleanup tool for Drive C.
        /// </summary>
        public static void RunExtendedDiskCleanup()
        {
            try
            {
                Process.Start(new ProcessStartInfo("cleanmgr.exe", "/d C") { UseShellExecute = true });
            }
            catch { }
        }

        /// <summary>
        /// Placeholder for OneDrive removal. Feature pending fix.
        /// </summary>
        public static void RemoveOneDrive()
        {
            MainWindow.Log("WARNING", "OneDrive removal is currently a PLACEHOLDER. Feature pending fix.");
            try
            {
                Process.Start(new ProcessStartInfo("cmd.exe", "/c echo OneDrive removal triggered (Simulation)") { CreateNoWindow = true });
            }
            catch { }
        }

        /// <summary>
        /// Restores the classic Windows 10 style context menu on Windows 11.
        /// </summary>
        public static void SetClassicContextMenu(bool enable)
        {
            try
            {
                string keyPath = @"Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32";
                if (enable)
                {
                    using (var key = Registry.CurrentUser.CreateSubKey(keyPath)) key.SetValue("", "");
                }
                else
                {
                    Registry.CurrentUser.DeleteSubKeyTree(@"Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}", false);
                }
                RestartExplorer();
            }
            catch { }
        }

        // --- System Tweaks Implementation ---
        public static void SetActivityHistory(bool enable) => SetDword(Registry.LocalMachine, @"SOFTWARE\Policies\Microsoft\Windows\System", "PublishUserActivities", enable ? 1 : 0);
        public static void SetConsumerFeatures(bool enable) => SetDword(Registry.LocalMachine, @"SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsConsumerFeatures", enable ? 0 : 1);
        public static void SetLocationTracking(bool enable) => RunPowerShell($"Set-Service -Name lfsvc -StartupType {(enable ? "Automatic" : "Disabled")}");
        public static void DisableWPBT() => SetDword(Registry.LocalMachine, @"SYSTEM\CurrentControlSet\Control\Session Manager", "DisableWPBT", 1);
        public static void EnableEndTask() => SetString(Registry.CurrentUser, @"Control Panel\Desktop", "EndTaskThreshold", "5000");
        public static void RemoveWidgets() => RunPowerShell("Get-AppxPackage -allusers *WebExperience* | Remove-AppxPackage -AllUsers");
        public static void OptimizeServices() => RunPowerShell("Set-Service -Name SysMain -StartupType Disabled");
        public static void SetCopilot(bool enable) => SetDword(Registry.CurrentUser, @"Software\Policies\Microsoft\Windows\WindowsCopilot", "TurnOffWindowsCopilot", enable ? 0 : 1);
        public static void DebloatEdge() => SetDword(Registry.LocalMachine, @"SOFTWARE\Policies\Microsoft\Edge", "HubsSidebarEnabled", 0);
        public static void SetTelemetry(bool disable) => SetDword(Registry.LocalMachine, TelemetryPath, "AllowTelemetry", disable ? 0 : 3);

        // ============================================================
        // INTERFACE TWEAKS (ConfigView)
        // ============================================================

        public static bool IsSecondsInClockEnabled() => GetDword(Registry.CurrentUser, ExplorerAdvancedPath, "ShowSecondsInSystemClock") == 1;
        public static void SetSecondsInClock(bool enable) => SetDword(Registry.CurrentUser, ExplorerAdvancedPath, "ShowSecondsInSystemClock", enable ? 1 : 0);

        public static bool IsHiddenFilesVisible() => GetDword(Registry.CurrentUser, ExplorerAdvancedPath, "Hidden") == 1;
        public static void SetHiddenFilesVisibility(bool visible)
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(ExplorerAdvancedPath, true))
                {
                    key?.SetValue("Hidden", visible ? 1 : 2);
                    key?.SetValue("HideFileExt", visible ? 0 : 1);
                    key?.SetValue("ShowSuperHidden", visible ? 1 : 2);
                }
                RunPowerShell("$s=New-Object -ComObject Shell.Application;$s.Windows() | %{ $_.Refresh() }");
            }
            catch { }
        }

        // ============================================================
        // HELPERS
        // ============================================================

        private static void RestartExplorer()
        {
            try
            {
                Process.Start("taskkill", "/f /im explorer.exe")?.WaitForExit();
                Process.Start("explorer.exe");
            }
            catch { }
        }

        private static void RunPowerShell(string cmd) => RunProcess("powershell.exe", $"-NoProfile -ExecutionPolicy Bypass -Command \"{cmd}\"");

        private static void RunPowerShellSync(string cmd)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo("powershell.exe", $"-NoProfile -ExecutionPolicy Bypass -Command \"{cmd}\"")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                using (Process p = Process.Start(psi)) { p?.WaitForExit(); }
            }
            catch { }
        }

        private static void RunProcess(string file, string args)
        {
            try
            {
                Process.Start(new ProcessStartInfo(file, args) { CreateNoWindow = true, UseShellExecute = false });
            }
            catch { }
        }

        private static int GetDword(RegistryKey root, string path, string name) { try { return (int)(root.OpenSubKey(path)?.GetValue(name, 0) ?? 0); } catch { return 0; } }
        private static void SetDword(RegistryKey root, string path, string name, int value) { try { root.CreateSubKey(path).SetValue(name, value, RegistryValueKind.DWord); } catch { } }
        private static void SetString(RegistryKey root, string path, string name, string value) { try { root.CreateSubKey(path).SetValue(name, value, RegistryValueKind.String); } catch { } }
    }
}