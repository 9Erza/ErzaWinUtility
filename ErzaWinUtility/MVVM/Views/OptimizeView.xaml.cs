using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ErzaWinUtility.Services;

namespace ErzaWinUtility.MVVM.Views
{
    /// <summary>
    /// Interaction logic for OptimizeView.xaml.
    /// Manages system performance profiles, safety locks, and registry-based optimization tweaks.
    /// </summary>
    public partial class OptimizeView : UserControl
    {
        public OptimizeView()
        {
            InitializeComponent();
        }

        // ============================================================
        // SAFETY & PROTECTION LOGIC
        // ============================================================

        /// <summary>
        /// Handles the safety lock checkbox. Enables or disables the tweaks container 
        /// based on user acknowledgment of risks.
        /// </summary>
        private void CheckSafetyLock_Changed(object sender, RoutedEventArgs e)
        {
            bool isSafe = CheckSafetyLock.IsChecked ?? false;

            // Update UI state based on the lock
            TweaksContainer.IsEnabled = isSafe;
            TweaksContainer.Opacity = isSafe ? 1.0 : 0.3;

            if (isSafe)
                MainWindow.Log("SYSTEM", "Safety lock released. Tweaks are now accessible.");
            else
                MainWindow.Log("SYSTEM", "Safety lock engaged. Registry modifications restricted.");
        }

        /// <summary>
        /// Triggers the creation of a Windows System Restore Point.
        /// </summary>
        private async void BtnCreateRestore_Click(object sender, RoutedEventArgs e)
        {
            BtnCreateRestore.IsEnabled = false;
            MainWindow.Log("SYSTEM", "Initializing System Restore Point creation...");

            try
            {
                // Execute restore point creation on a background thread
                await Task.Run(() => RegistryService.CreateRestorePoint("ErzaUtility_PreOptimization"));

                MainWindow.Log("SUCCESS", "System Restore Point created successfully.");
            }
            catch (Exception ex)
            {
                MainWindow.Log("ERROR", $"Failed to create restore point: {ex.Message}");
            }
            finally
            {
                BtnCreateRestore.IsEnabled = true;
            }
        }

        // ============================================================
        // PROFILE MANAGEMENT (Recommended, All, Clear)
        // ============================================================

        private void BtnProfileRecommended_Click(object sender, RoutedEventArgs e)
        {
            // First, clear all existing selections
            BtnProfileClear_Click(null, null);

            // Select recommended tweaks
            TweakTempFiles.IsChecked = true;
            TweakConsumerFeatures.IsChecked = true;
            TweakLocation.IsChecked = true;
            TweakTelemetry.IsChecked = true;
            TweakDiskCleanup.IsChecked = true;
            TweakServices.IsChecked = true;
            TweakCopilot.IsChecked = true;

            MainWindow.Log("UI", "Recommended optimization profile applied.");
        }

        private void BtnProfileAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var child in TweaksList.Children)
            {
                if (child is CheckBox cb) cb.IsChecked = true;
            }
            MainWindow.Log("UI", "All optimization tweaks selected.");
        }

        private void BtnProfileClear_Click(object sender, RoutedEventArgs e)
        {
            foreach (var child in TweaksList.Children)
            {
                if (child is CheckBox cb) cb.IsChecked = false;
            }
            MainWindow.Log("UI", "Selection cleared.");
        }

        // ============================================================
        // TWEAK EXECUTION ENGINE
        // ============================================================

        /// <summary>
        /// Iterates through selected tweaks and applies them sequentially.
        /// </summary>
        private async void BtnRunTweaks_Click(object sender, RoutedEventArgs e)
        {
            BtnRunTweaks.IsEnabled = false;
            MainWindow.Log("OPTIMIZER", "Starting system optimization process...");

            try
            {
                // Run registry modifications on a background thread
                await Task.Run(() =>
                {
                    if (CheckState(TweakTempFiles)) RegistryService.CleanupTempFiles();
                    if (CheckState(TweakActivityHistory)) RegistryService.SetActivityHistory(false);
                    if (CheckState(TweakConsumerFeatures)) RegistryService.SetConsumerFeatures(false);
                    if (CheckState(TweakLocation)) RegistryService.SetLocationTracking(false);
                    if (CheckState(TweakTelemetry)) RegistryService.SetTelemetry(true); // true = disable
                    if (CheckState(TweakWPBT)) RegistryService.DisableWPBT();
                    if (CheckState(TweakEndTask)) RegistryService.EnableEndTask();
                    if (CheckState(TweakWidgets)) RegistryService.RemoveWidgets();
                    if (CheckState(TweakDiskCleanup)) RegistryService.RunExtendedDiskCleanup();
                    if (CheckState(TweakServices)) RegistryService.OptimizeServices();
                    if (CheckState(TweakCopilot)) RegistryService.SetCopilot(false);
                    if (CheckState(TweakEdge)) RegistryService.DebloatEdge();
                    if (CheckState(TweakOneDrive)) RegistryService.RemoveOneDrive();
                    if (CheckState(TweakClassicMenu)) RegistryService.SetClassicContextMenu(true);
                });

                MainWindow.Log("SUCCESS", "All selected tweaks applied successfully.");
            }
            catch (Exception ex)
            {
                MainWindow.Log("CRITICAL", $"Optimization failed: {ex.Message}");
            }
            finally
            {
                BtnRunTweaks.IsEnabled = true;
            }
        }

        /// <summary>
        /// Safely checks the checkbox state from a background thread using the Dispatcher.
        /// </summary>
        private bool CheckState(CheckBox cb)
        {
            bool result = false;
            Application.Current.Dispatcher.Invoke(() => result = cb.IsChecked ?? false);
            return result;
        }

        // ============================================================
        // POWER PLAN DEPLOYMENT
        // ============================================================

        private void BtnInstallPlan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                string resourceName = "ErzaWinUtility.Resources.Erza_PowerPlan.pow";

                using (Stream? stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        MainWindow.Log("ERROR", "Power Plan resource missing from assembly.");
                        return;
                    }

                    byte[] data = new byte[stream.Length];
                    stream.Read(data, 0, data.Length);

                    if (PowerService.ImportAndActivatePlan(data))
                        MainWindow.Log("POWER", "Erza Ultimate Power Plan activated.");
                    else
                        MainWindow.Log("ERROR", "Power Plan deployment failed.");
                }
            }
            catch (Exception ex)
            {
                MainWindow.Log("CRITICAL", $"Power Plan error: {ex.Message}");
            }
        }
    }
}