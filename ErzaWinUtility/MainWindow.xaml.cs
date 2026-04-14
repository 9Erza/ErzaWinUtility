using System;
using System.Windows;
using System.Windows.Input;
using ErzaWinUtility.MVVM.Views;

namespace ErzaWinUtility
{
    /// <summary>
    /// Interaction logic for the main application window.
    /// Handles navigation management, window state control, and the global logging system.
    /// </summary>
    public partial class MainWindow : Window
    {
        // Static instance for global UI logging access across services and views
        private static MainWindow? _instance;

        public MainWindow()
        {
            InitializeComponent();
            _instance = this;

            // Set the default startup view (Optimization)
            SwitchView(new OptimizeView(), "System Optimization");

            Log("SYSTEM", "Erza Win Utility Engine initialized successfully.");
        }

        // ============================================================
        // WINDOW CONTROL & NAVIGATION ENGINE
        // ============================================================

        /// <summary>
        /// Allows the borderless window to be dragged from any part of the background.
        /// Linked via MouseDown in XAML.
        /// </summary>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Helper method to handle View switching and UI terminal logging.
        /// </summary>
        private void SwitchView(object view, string logName)
        {
            try
            {
                ViewContainer.Content = view;
                Log("NAV", $"Switched to {logName}");
            }
            catch (Exception ex)
            {
                Log("ERROR", $"Navigation failure: {ex.Message}");
                MessageBox.Show($"Error loading view: {ex.Message}", "View Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ============================================================
        // GLOBAL LOGGING SYSTEM
        // ============================================================

        /// <summary>
        /// Global logging method to display messages in the UI terminal.
        /// Thread-safe: can be called from background tasks via Dispatcher.
        /// </summary>
        public static void Log(string category, string message)
        {
            // Thread-safe invocation on the UI thread
            _instance?.Dispatcher.Invoke(() =>
            {
                if (_instance.StatusLog != null)
                {
                    string timestamp = DateTime.Now.ToString("HH:mm:ss");
                    _instance.StatusLog.Text += $"\n[{timestamp}] [{category.ToUpper()}] {message}";

                    // Automatically scroll the ScrollViewer to the latest entry
                    _instance.LogScrollViewer?.ScrollToEnd();
                }
            });
        }

        // ============================================================
        // SIDEBAR BUTTON HANDLERS
        // ============================================================

        private void NavInstall_Click(object sender, RoutedEventArgs e)
        {
            SwitchView(new InstallView(), "App Installer");
        }

        private void NavOptimize_Click(object sender, RoutedEventArgs e)
        {
            SwitchView(new OptimizeView(), "System Optimization");
        }

        private void NavConfig_Click(object sender, RoutedEventArgs e)
        {
            SwitchView(new ConfigView(), "Additional Configuration");
        }

        private void NavUpdates_Click(object sender, RoutedEventArgs e)
        {
            // Fully qualified name to avoid any potential namespace ambiguity
            SwitchView(new ErzaWinUtility.MVVM.Views.UpdatesView(), "System Updates");
        }
    }
}