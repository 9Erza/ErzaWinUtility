using System;
using System.Windows;
using ErzaWinUtility.MVVM.Views;

namespace ErzaWinUtility
{
    public partial class MainWindow : Window
    {
        private static MainWindow _instance;

        public MainWindow()
        {
            InitializeComponent();
            _instance = this;
            MainFrame.Content = new OptimizeView(); // Widok startowy
            Log("SYSTEM", "New project core initialized.");
        }

        public static void Log(string tag, string message)
        {
            _instance?.Dispatcher.Invoke(() => {
                _instance.LogTerminal.AppendText($"[{DateTime.Now:HH:mm:ss}] [{tag}] {message}\n");
                _instance.LogTerminal.ScrollToEnd();
            });
        }

        private void NavOptimize_Click(object sender, RoutedEventArgs e) => MainFrame.Content = new OptimizeView();
        private void NavConfig_Click(object sender, RoutedEventArgs e) => MainFrame.Content = new ConfigView();
        private void NavInstall_Click(object sender, RoutedEventArgs e) => MainFrame.Content = new InstallView();
        private void Exit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
    }
}