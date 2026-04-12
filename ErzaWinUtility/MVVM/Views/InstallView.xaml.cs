using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ErzaWinUtility.Services;

namespace ErzaWinUtility.MVVM.Views
{
    /// <summary>
    /// Interaction logic for InstallView.xaml.
    /// Manages bulk application installation using the Winget package manager.
    /// </summary>
    public partial class InstallView : UserControl
    {
        public InstallView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Collects all selected applications and initiates the asynchronous installation process.
        /// </summary>
        private async void BtnInstall_Click(object sender, RoutedEventArgs e)
        {
            // Disable button to prevent multiple concurrent installation sessions
            BtnInstall.IsEnabled = false;
            MainWindow.Log("INSTALL", "Starting automated installation process...");

            // Map UI CheckBoxes to their respective Winget IDs
            var appMappings = new Dictionary<CheckBox, string>
            {
                { ChromeCheck, "Google.Chrome" },
                { BraveCheck, "Brave.Brave" },
                { FirefoxCheck, "Mozilla.Firefox" },
                { OperaCheck, "Opera.OperaGX" },
                { VsCodeCheck, "Microsoft.VisualStudioCode" },
                { GitCheck, "Git.Git" },
                { SevenZipCheck, "7zip.7zip" },
                { DiscordCheck, "Discord.Discord" }
            };

            List<Task> installationTasks = new List<Task>();

            foreach (var app in appMappings)
            {
                if (app.Key.IsChecked == true)
                {
                    MainWindow.Log("WINGET", $"Queuing installation for: {app.Value}");
                    // We call the asynchronous service method to handle the process
                    installationTasks.Add(HandleInstallation(app.Value));
                }
            }

            if (installationTasks.Count > 0)
            {
                await Task.WhenAll(installationTasks);
                MainWindow.Log("INSTALL", "All selected applications have been processed.");
            }
            else
            {
                MainWindow.Log("WARNING", "No applications selected for installation.");
            }

            BtnInstall.IsEnabled = true;
        }

        /// <summary>
        /// Wrapper to handle individual application installation and logging.
        /// </summary>
        /// <param name="appId">The Winget ID of the application.</param>
        private async Task HandleInstallation(string appId)
        {
            try
            {
                await WingetService.InstallAppAsync(appId); //
                MainWindow.Log("SUCCESS", $"Finished processing {appId}");
            }
            catch (Exception ex)
            {
                MainWindow.Log("ERROR", $"Failed to install {appId}: {ex.Message}");
            }
        }
    }
}