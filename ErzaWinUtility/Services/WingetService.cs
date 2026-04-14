using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ErzaWinUtility.Services
{
    /// <summary>
    /// Service responsible for managing software deployments via Winget.
    /// Optimized for silent installations with automated agreement acceptance.
    /// </summary>
    public static class WingetService
    {
        /// <summary>
        /// Installs an application using its official Winget identifier.
        /// </summary>
        public static async Task InstallAppAsync(string appId)
        {
            await Task.Run(() =>
            {
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = "winget",
                        // Standard flags that work for 99% of apps in your list
                        Arguments = $"install --id {appId} -e --silent --force --accept-source-agreements --accept-package-agreements",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };

                    using (Process? process = Process.Start(startInfo))
                    {
                        process?.WaitForExit();

                        if (process?.ExitCode != 0)
                        {
                            Debug.WriteLine($"[WINGET] {appId} finished with exit code: {process?.ExitCode}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[CRITICAL] Winget Service Error: {ex.Message}");
                    throw;
                }
            });
        }
    }
}