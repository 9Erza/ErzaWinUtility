using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Versioning;

namespace ErzaWinUtility.Services
{
    /// <summary>
    /// Service responsible for managing Windows Power Plans.
    /// Handles importing, activating, and configuring custom power profiles via powercfg.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class PowerService
    {
        // Unique identifier for the custom power profile to ensure consistent targeting.
        private const string PlanGuid = "81b11e0c-c34c-4a2e-85ab-c5be4cd02b28";

        // ============================================================
        // PLAN DEPLOYMENT
        // ============================================================

        /// <summary>
        /// Imports a binary power plan file, registers it with a unique GUID, and sets it as the active plan.
        /// </summary>
        /// <param name="planData">The binary content of the .pow file.</param>
        /// <returns>True if the operation succeeded; otherwise, false.</returns>
        public static bool ImportAndActivatePlan(byte[] planData)
        {
            try
            {
                // 1. Save the binary data to a temporary file for the import process.
                string tempPath = Path.Combine(Path.GetTempPath(), "erza_plan.pow");
                File.WriteAllBytes(tempPath, planData);

                // 2. Import the plan using the specified GUID.
                RunPowerCfg($"-import \"{tempPath}\" {PlanGuid}");

                // 3. Activate the newly imported plan.
                RunPowerCfg($"-setactive {PlanGuid}");

                // Clean up the temporary file after successful import.
                if (File.Exists(tempPath))
                    File.Delete(tempPath);

                return true;
            }
            catch (Exception ex)
            {
                // Log failure to the main window for diagnostic visibility.
                MainWindow.Log("ERROR", $"Power Plan deployment failed: {ex.Message}");
                return false;
            }
        }

        // ============================================================
        // PRIVATE HELPERS
        // ============================================================

        /// <summary>
        /// Executes a powercfg command with elevated privileges.
        /// </summary>
        /// <param name="args">The command line arguments for powercfg.exe.</param>
        private static void RunPowerCfg(string args)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "powercfg",
                    Arguments = args,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = true, // Required for 'runas' verb
                    Verb = "runas"          // Request administrative elevation for power modification
                };

                using (Process? p = Process.Start(psi))
                {
                    p?.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[FATAL] PowerCfg execution error: {ex.Message}");
            }
        }
    }
}