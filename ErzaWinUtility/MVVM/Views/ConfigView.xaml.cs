using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using ErzaWinUtility.Services;

namespace ErzaWinUtility.MVVM.Views
{
    /// <summary>
    /// Interaction logic for ConfigView.xaml.
    /// Manages system configuration settings and UI-to-Registry synchronization.
    /// </summary>
    public partial class ConfigView : UserControl
    {
        public ConfigView()
        {
            InitializeComponent();

            // Sync UI states with current Windows configuration on initialization
            SynchronizeViewWithSystem();
        }

        /// <summary>
        /// Reads current Windows settings from the registry to update the toggle states.
        /// </summary>
        private void SynchronizeViewWithSystem()
        {
            try
            {
                if (BsodToggle != null) BsodToggle.IsChecked = RegistryService.IsDetailedBsodEnabled();
                if (HibernationToggle != null) HibernationToggle.IsChecked = RegistryService.IsHibernationEnabled();
                if (ProtectionToggle != null) ProtectionToggle.IsChecked = RegistryService.IsSystemProtectionEnabled();
                if (CoreIsolationToggle != null) CoreIsolationToggle.IsChecked = RegistryService.IsCoreIsolationEnabled();
                if (SecondsToggle != null) SecondsToggle.IsChecked = RegistryService.IsSecondsInClockEnabled();
                if (HiddenFilesToggle != null) HiddenFilesToggle.IsChecked = RegistryService.IsHiddenFilesVisible();
            }
            catch (Exception ex)
            {
                MainWindow.Log("ERROR", $"OS Sync failed: {ex.Message}");
            }
        }

        // ============================================================
        // SECURITY & SYSTEM EVENTS
        // ============================================================

        private void BsodToggle_Click(object sender, RoutedEventArgs e)
        {
            bool isEnabled = BsodToggle.IsChecked ?? false;
            RegistryService.SetDetailedBsod(isEnabled);
            MainWindow.Log("CONFIG", $"Detailed BSoD {(isEnabled ? "Enabled" : "Disabled")}");
        }

        private void HibernationToggle_Click(object sender, RoutedEventArgs e)
        {
            bool isEnabled = HibernationToggle.IsChecked ?? false;
            RegistryService.SetHibernation(isEnabled);
            MainWindow.Log("CONFIG", $"System Hibernation {(isEnabled ? "Enabled" : "Disabled")}");
        }

        private void ProtectionToggle_Click(object sender, RoutedEventArgs e)
        {
            bool isEnabled = ProtectionToggle.IsChecked ?? false;
            RegistryService.SetSystemProtection(isEnabled);
            MainWindow.Log("CONFIG", $"System Protection {(isEnabled ? "Enabled" : "Disabled")}");
        }

        private void CoreIsolationToggle_Click(object sender, RoutedEventArgs e)
        {
            bool isEnabled = CoreIsolationToggle.IsChecked ?? false;
            RegistryService.SetCoreIsolation(isEnabled);
            MainWindow.Log("SECURITY", $"Core Isolation {(isEnabled ? "Enabled" : "Disabled")}");

            MessageBox.Show("Core Isolation modified. A system restart is required for changes to take effect.",
                            "Security Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // ============================================================
        // INTERFACE & MOUSE EVENTS
        // ============================================================

        private void MouseSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Launches the native Windows Mouse Properties dialog (main.cpl)
                Process.Start(new ProcessStartInfo { FileName = "main.cpl", UseShellExecute = true });
                MainWindow.Log("SYSTEM", "Opened Windows Mouse Properties.");
            }
            catch (Exception ex)
            {
                MainWindow.Log("ERROR", $"Could not open mouse settings: {ex.Message}");
            }
        }

        private void SecondsToggle_Click(object sender, RoutedEventArgs e)
        {
            bool isEnabled = SecondsToggle.IsChecked ?? false;
            RegistryService.SetSecondsInClock(isEnabled);
            MainWindow.Log("UI", $"Clock Seconds {(isEnabled ? "Enabled" : "Disabled")}");
        }

        private void HiddenFilesToggle_Click(object sender, RoutedEventArgs e)
        {
            bool isEnabled = HiddenFilesToggle.IsChecked ?? false;
            RegistryService.SetHiddenFilesVisibility(isEnabled);
            MainWindow.Log("UI", $"Hidden Files visibility updated.");
        }

        // ============================================================
        // NETWORK EVENTS
        // ============================================================

        private void DnsSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // IsLoaded check ensures logic runs only after UI is ready
            if (DnsSelector != null && DnsSelector.IsLoaded)
            {
                if (DnsSelector.SelectedItem is ComboBoxItem item)
                {
                    NetworkService.SetSystemDns(DnsSelector.SelectedIndex);
                    MainWindow.Log("NETWORK", $"DNS Provider updated to: {item.Content}");
                }
            }
        }
    }
}