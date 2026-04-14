using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.Versioning;

namespace ErzaWinUtility.Services
{
    /// <summary>
    /// Service responsible for managing software deployments via the Windows Package Manager (Winget).
    /// Optimized for silent installations with automated agreement acceptance.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class WingetService
    {
        // ============================================================
        // APPLICATION DEPLOYMENT
        // ============================================================

        /// <summary>
        /// Installs an application using its official Winget identifier asynchronously.
        /// </summary>
        /// <param name="appId">The unique package ID used by Winget (e.g., 'Google.Chrome').</param>
        public static async Task InstallAppAsync(string appId)
        {
            // Execute the installation process on a background thread to prevent UI freezing
            await Task.Run(() =>
            {
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = "winget",
                        // -e: Exact ID match
                        // --silent: No UI interaction
                        // --force: Overwrite existing versions if necessary
                        // --accept-*: Automatically agree to licenses
                        Arguments = $"install --id {appId} -e --silent --force --accept-source-agreements --accept-package-agreements",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };

                    using (Process? process = Process.Start(startInfo))
                    {
                        process?.WaitForExit();

                        // Log exit code if the installation was not successful (0 = success)
                        if (process?.ExitCode != 0)
                        {
                            Debug.WriteLine($"[WINGET] {appId} finished with non-zero exit code: {process?.ExitCode}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[CRITICAL] Winget Service Error: {ex.Message}");

                    // Re-throw the exception so the calling View can log it to the UI Terminal
                    throw;
                }
            });
        }
    }
}