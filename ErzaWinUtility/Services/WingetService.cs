using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
namespace ErzaWinUtility.Services

{

    /// <summary>

    /// Service to manage application installations using Windows Package Manager (Winget).

    /// Handles asynchronous process execution to keep the UI responsive.

    /// </summary>

    public static class WingetService

    {

        /// <summary>

        /// Installs an application by its Winget ID.

        /// </summary>

        /// <param name="appId">The unique ID of the application (e.g., "Brave.Brave")</param>

        /// <returns>A task representing the asynchronous operation.</returns>

        public static async Task InstallAppAsync(string appId)

        {

            // We use Task.Run to execute the process on a background thread

            await Task.Run(() =>

            {

                try

                {

                    ProcessStartInfo startInfo = new ProcessStartInfo

                    {

                        FileName = "winget",

                        // --accept-source-agreements and --accept-package-agreements bypasses prompts

                        Arguments = $"install --id {appId} -e --silent --accept-source-agreements --accept-package-agreements",

                        UseShellExecute = false,

                        CreateNoWindow = true,

                        RedirectStandardOutput = true,

                        RedirectStandardError = true

                    };



                    using (Process process = new Process { StartInfo = startInfo })

                    {

                        process.Start();

                        // In a more advanced version, we could read output here to show progress

                        process.WaitForExit();



                        Debug.WriteLine($"Installation of {appId} finished with code: {process.ExitCode}");

                    }

                }

                catch (Exception ex)

                {

                    Debug.WriteLine($"Error installing {appId}: {ex.Message}");

                }

            });

        }

    }

}