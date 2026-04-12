using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ErzaWinUtility.Services;

namespace ErzaWinUtility.MVVM.Views
{
    /// <summary>
    /// Interaction logic for OptimizeView.xaml.
    /// Handles system performance tweaks and power plan deployment.
    /// </summary>
    public partial class OptimizeView : UserControl
    {
        public OptimizeView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Extracts the embedded .pow file and applies it to the system.
        /// </summary>
        private void BtnInstallPlan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();

                // IMPORTANT: Ensure the folder name in your project is "Resources" (not "Resoruces").
                // The resource name follows the pattern: [AssemblyName].[FolderName].[FileName]
                string resourceName = "ErzaWinUtility.Resources.Erza_PowerPlan.pow";

                using (Stream? stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        MainWindow.Log("ERROR", "Power Plan resource not found in assembly.");
                        MessageBox.Show($"Internal resource missing: {resourceName}\nCheck if Build Action is set to Embedded Resource.", "Resource Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    byte[] data = new byte[stream.Length];
                    stream.Read(data, 0, data.Length);

                    // Delegate the binary data to the PowerService for system application.
                    if (PowerService.ImportAndActivatePlan(data))
                    {
                        MainWindow.Log("POWER", "Erza Ultimate Power Plan applied and activated.");
                    }
                    else
                    {
                        MainWindow.Log("ERROR", "Failed to apply Power Plan. Administrator rights required.");
                    }
                }
            }
            catch (Exception ex)
            {
                MainWindow.Log("CRITICAL", $"Optimization error: {ex.Message}");
            }
        }

        /// <summary>
        /// Toggles Windows Telemetry settings via RegistryService.
        /// </summary>
        private void TelemetryToggle_Click(object sender, RoutedEventArgs e)
        {
            if (TelemetryToggle != null)
            {
                bool disable = TelemetryToggle.IsChecked ?? false;

                // Apply the setting to the registry and system services.
                RegistryService.SetTelemetry(disable);

                string status = disable ? "Disabled (Privacy Optimized)" : "Enabled (Default)";
                MainWindow.Log("OPTIMIZE", $"Windows Telemetry: {status}");
            }
        }
    }
}