using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace ErzaWinUtility.Services
{
    /// <summary>
    /// Core service for managing Windows Registry modifications and system-level tweaks.
    /// Provides methods to toggle security features, system UI elements, and performance optimizations.
    /// </summary>
    public static class RegistryService
    {
        private const string CrashControlPath = @"System\CurrentControlSet\Control\CrashControl";
        private const string HibernatePath = @"System\CurrentControlSet\Control\Power";
        private const string SystemRestorePath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore";
        private const string CoreIsolationPath = @"SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity";
        private const string ExplorerAdvancedPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced";
        private const string TelemetryPath = @"SOFTWARE\Policies\Microsoft\Windows\DataCollection";

        // ============================================================
        // SECURITY & SYSTEM
        // ============================================================

        /// <summary>
        /// Checks if the Blue Screen of Death (BSoD) is configured to show technical details.
        /// </summary>
        public static bool IsDetailedBsodEnabled()
        {
            try { using (var key = Registry.LocalMachine.OpenSubKey(CrashControlPath)) return (int)(key?.GetValue("DisplayParameters", 0) ?? 0) == 1; } catch { return false; }
        }

        /// <summary>
        /// Configures the system to show detailed technical information on a crash (BSoD).
        /// </summary>
        public static void SetDetailedBsod(bool enable)
        {
            try { using (var key = Registry.LocalMachine.OpenSubKey(CrashControlPath, true)) key?.SetValue("DisplayParameters", enable ? 1 : 0, RegistryValueKind.DWord); } catch { }
        }

        /// <summary>
        /// Checks if system hibernation is currently enabled in the registry.
        /// </summary>
        public static bool IsHibernationEnabled()
        {
            try { using (var key = Registry.LocalMachine.OpenSubKey(HibernatePath)) return (int)(key?.GetValue("HibernateEnabled", 0) ?? 0) == 1; } catch { return false; }
        }

        /// <summary>
        /// Toggles system hibernation state using the powercfg utility.
        /// </summary>
        public static void SetHibernation(bool enable)
        {
            try { Process.Start(new ProcessStartInfo("powercfg", enable ? "-h on" : "-h off") { WindowStyle = ProcessWindowStyle.Hidden, CreateNoWindow = true, Verb = "runas" }); } catch { }
        }

        /// <summary>
        /// Verifies if System Protection (Restore Points) is enabled for the primary drive.
        /// </summary>
        public static bool IsSystemProtectionEnabled()
        {
            try { using (var key = Registry.LocalMachine.OpenSubKey(SystemRestorePath)) return key?.GetValue("RPSessionInterval") != null; } catch { return false; }
        }

        /// <summary>
        /// Enables or disables System Protection via PowerShell commands.
        /// </summary>
        public static void SetSystemProtection(bool enable)
        {
            try { Process.Start(new ProcessStartInfo("powershell.exe", $"-Command \"{(enable ? "Enable" : "Disable")}-ComputerRestore -Drive 'C:\\'\"") { WindowStyle = ProcessWindowStyle.Hidden, CreateNoWindow = true, Verb = "runas" }); } catch { }
        }

        /// <summary>
        /// Checks if Core Isolation (Memory Integrity) is enabled in the registry.
        /// </summary>
        public static bool IsCoreIsolationEnabled()
        {
            try { using (var key = Registry.LocalMachine.OpenSubKey(CoreIsolationPath)) return (int)(key?.GetValue("Enabled", 0) ?? 0) == 1; } catch { return false; }
        }

        /// <summary>
        /// Toggles Core Isolation (VBS) state. Note: Requires a system reboot to apply.
        /// </summary>
        public static void SetCoreIsolation(bool enable)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.CreateSubKey(CoreIsolationPath))
                {
                    key.SetValue("Enabled", enable ? 1 : 0, RegistryValueKind.DWord);
                }
            }
            catch { }
        }

        // ============================================================
        // OPTIMIZATION (TELEMETRY)
        // ============================================================

        /// <summary>
        /// Disables Windows Data Collection (Telemetry) and stops related background services.
        /// </summary>
        public static void SetTelemetry(bool disable)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.CreateSubKey(TelemetryPath))
                {
                    // 0 = Security (Minimal data), 3 = Full (Default)
                    key.SetValue("AllowTelemetry", disable ? 0 : 3, RegistryValueKind.DWord);
                }

                // Manage the 'Connected User Experiences and Telemetry' service (DiagTrack)
                string cmd = disable ? "stop-service DiagTrack; set-service DiagTrack -startupType Disabled"
                                     : "set-service DiagTrack -startupType Automatic; start-service DiagTrack";

                Process.Start(new ProcessStartInfo("powershell.exe", $"-Command \"{cmd}\"") { WindowStyle = ProcessWindowStyle.Hidden, CreateNoWindow = true, Verb = "runas" });
            }
            catch { }
        }

        // ============================================================
        // INTERFACE TWEAKS
        // ============================================================

        /// <summary>
        /// Checks if the system clock taskbar is configured to show seconds.
        /// </summary>
        public static bool IsSecondsInClockEnabled()
        {
            try { using (var key = Registry.CurrentUser.OpenSubKey(ExplorerAdvancedPath)) return (int)(key?.GetValue("ShowSecondsInSystemClock", 0) ?? 0) == 1; } catch { return false; }
        }

        /// <summary>
        /// Toggles the visibility of seconds in the system taskbar clock.
        /// </summary>
        public static void SetSecondsInClock(bool enable)
        {
            try { using (var key = Registry.CurrentUser.OpenSubKey(ExplorerAdvancedPath, true)) key?.SetValue("ShowSecondsInSystemClock", enable ? 1 : 0, RegistryValueKind.DWord); } catch { }
        }

        /// <summary>
        /// Verifies if hidden files and folders are currently set to be visible in Explorer.
        /// </summary>
        public static bool IsHiddenFilesVisible()
        {
            try { using (var key = Registry.CurrentUser.OpenSubKey(ExplorerAdvancedPath)) return (int)(key?.GetValue("Hidden", 2) ?? 2) == 1; } catch { return false; }
        }

        /// <summary>
        /// Toggles the visibility of hidden files, system files, and extensions, then refreshes all Explorer windows.
        /// </summary>
        public static void SetHiddenFilesVisibility(bool visible)
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(ExplorerAdvancedPath, true))
                {
                    if (key == null) return;
                    key.SetValue("Hidden", visible ? 1 : 2, RegistryValueKind.DWord);
                    key.SetValue("HideFileExt", visible ? 0 : 1, RegistryValueKind.DWord);
                    key.SetValue("ShowSuperHidden", visible ? 1 : 2, RegistryValueKind.DWord);
                }
                // Refresh shell windows to apply changes immediately
                Process.Start(new ProcessStartInfo("powershell.exe", "-Command \"$s=New-Object -ComObject Shell.Application;$s.Windows() | %{ $_.Refresh() }\"") { WindowStyle = ProcessWindowStyle.Hidden, CreateNoWindow = true });
            }
            catch { }
        }
    }
}