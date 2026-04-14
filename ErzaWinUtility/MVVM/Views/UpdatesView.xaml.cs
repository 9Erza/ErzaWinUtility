using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ErzaWinUtility.Services;

namespace ErzaWinUtility.MVVM.Views
{
    /// <summary>
    /// Interaction logic for UpdatesView.xaml.
    /// Provides an interface for managing Windows Update services and repairing system components.
    /// </summary>
    public partial class UpdatesView : UserControl
    {
        public UpdatesView()
        {
            InitializeComponent();
        }

        // ============================================================
        // WINDOWS UPDATE CONTROL
        // ============================================================

        /// <summary>
        /// Disables the Windows Update service and prevents automatic check-ins.
        /// </summary>
        private void BtnPauseUpdates_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RegistryService.SetWindowsUpdateStatus(false);
                MainWindow.Log("SYSTEM", "Windows Update service has been disabled.");
            }
            catch (Exception ex)
            {
                MainWindow.Log("ERROR", $"Failed to pause updates: {ex.Message}");
            }
        }

        /// <summary>
        /// Enables the Windows Update service and restores default automatic behavior.
        /// </summary>
        private void BtnResumeUpdates_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RegistryService.SetWindowsUpdateStatus(true);
                MainWindow.Log("SYSTEM", "Windows Update service has been restored to Automatic.");
            }
            catch (Exception ex)
            {
                MainWindow.Log("ERROR", $"Failed to resume updates: {ex.Message}");
            }
        }

        // ============================================================
        // COMPONENT MAINTENANCE
        // ============================================================

        /// <summary>
        /// Performs a full reset of Windows Update components including cache clearing.
        /// </summary>
        private async void BtnResetUpdates_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null) btn.IsEnabled = false;

            try
            {
                MainWindow.Log("PROCESS", "Resetting Windows Update components (stopping services and clearing cache)...");

                // Execute heavy I/O and service operations on a background thread
                await Task.Run(() => RegistryService.ResetWindowsUpdateComponents());

                MainWindow.Log("SUCCESS", "Windows Update components reset and services restarted.");
            }
            catch (Exception ex)
            {
                MainWindow.Log("ERROR", $"Component reset failed: {ex.Message}");
            }
            finally
            {
                if (btn != null) btn.IsEnabled = true;
            }
        }
    }
}