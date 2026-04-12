using System;
using System.Windows;
using System.Windows.Input;
using ErzaWinUtility.MVVM.Views;

namespace ErzaWinUtility
{
    /// <summary>
    /// Logic for the main application window.
    /// Manages navigation, window state, and global logging system.
    /// </summary>
    public partial class MainWindow : Window
    {
        // Static instance allows global access to the UI logger from any service or view.
        private static MainWindow? _instance;

        public MainWindow()
        {
            InitializeComponent();
            _instance = this;

            // Initialize window control buttons
            InitializeWindowControls();

            // Set the default startup view
            ViewContainer.Content = new OptimizeView();

            Log("SYSTEM", "Erza Utility Engine initialized successfully.");
        }

        /// <summary>
        /// Registers event handlers for custom title bar and system buttons.
        /// </summary>
        private void InitializeWindowControls()
        {
            if (BtnClose != null)
                BtnClose.Click += (s, e) => Application.Current.Shutdown();

            if (BtnMinimize != null)
                BtnMinimize.Click += (s, e) => this.WindowState = WindowState.Minimized;

            if (TitleBar != null)
            {
                TitleBar.MouseLeftButtonDown += (s, e) =>
                {
                    if (e.LeftButton == MouseButtonState.Pressed) DragMove();
                };
            }
        }

        /// <summary>
        /// Global logging method to display messages in the UI terminal.
        /// Thread-safe: can be called from background tasks.
        /// </summary>
        /// <param name="category">The source or type of the message (e.g., SYSTEM, NETWORK).</param>
        /// <param name="message">The actual information to log.</param>
        public static void Log(string category, string message)
        {
            _instance?.Dispatcher.Invoke(() =>
            {
                if (_instance.StatusLog != null)
                {
                    string timestamp = DateTime.Now.ToString("HH:mm:ss");
                    _instance.StatusLog.Text += $"\n[{timestamp}] [{category.ToUpper()}] {message}";

                    // Automatically scroll to the latest log entry
                    _instance.LogScrollViewer?.ScrollToEnd();
                }
            });
        }

        // ============================================================
        // NAVIGATION HANDLERS
        // ============================================================

        private void NavInstall_Click(object sender, RoutedEventArgs e)
        {
            ViewContainer.Content = new InstallView();
            Log("NAV", "Switched to App Installer");
        }

        private void NavOptimize_Click(object sender, RoutedEventArgs e)
        {
            ViewContainer.Content = new OptimizeView();
            Log("NAV", "Switched to System Optimization");
        }

        private void NavConfig_Click(object sender, RoutedEventArgs e)
        {
            ViewContainer.Content = new ConfigView();
            Log("NAV", "Switched to Additional Configuration");
        }
    }
}