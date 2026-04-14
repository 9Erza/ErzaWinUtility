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
    /// Manages automated software deployment using the Windows Package Manager (Winget).
    /// </summary>
    public partial class InstallView : UserControl
    {
        public InstallView()
        {
            InitializeComponent();
        }

        // ============================================================
        // INSTALLATION LOGIC
        // ============================================================

        /// <summary>
        /// Collects selected applications and initiates the Winget installation process.
        /// </summary>
        private async void BtnInstall_Click(object sender, RoutedEventArgs e)
        {
            BtnInstall.IsEnabled = false;
            MainWindow.Log("INSTALLER", "Preparing installation queue...");

            // Mapping UI CheckBoxes to official Winget Package IDs
            var appMappings = new Dictionary<CheckBox, string>
            {
                // Browsers
                { BraveCheck, "Brave.Brave" },
                { ChromeCheck, "Google.Chrome" },
                { FirefoxCheck, "Mozilla.Firefox" },
                { EdgeCheck, "Microsoft.Edge" },

                // Communication
                { DiscordCheck, "Discord.Discord" },
                { TeamSpeakCheck, "TeamSpeakSystems.TeamSpeak" },
                { MumbleCheck, "Mumble.Mumble" },
                { TeamsCheck, "Microsoft.Teams" },
                { SlackCheck, "SlackTechnologies.Slack" },

                // Gaming Launchers
                { PlayniteCheck, "Playnite.Playnite" },
                { SteamCheck, "Valve.Steam" },
                { EpicCheck, "EpicGames.EpicGamesLauncher" },
                { UbisoftCheck, "Ubisoft.Connect" },
                { GogCheck, "GOG.Galaxy" },
                { EaCheck, "ElectronicArts.EADesktop" },
                { BattleNetCheck, "Blizzard.BattleNet" },

                // Multimedia
                { SpotifyCheck, "9P6527LD0L4L" },
                { TidalCheck, "TIDAL.TIDAL" },
                { VlcCheck, "VideoLAN.VLC" },
                { KliteCheck, "CodecGuide.K-LiteCodecPack.Full" },

                // Content Creation
                { ObsCheck, "OBSProject.OBSStudio" },
                { StreamlabsCheck, "Streamlabs.StreamlabsDesktop" },
                { MeldCheck, "MeldStudio.MeldStudio" },
                { MedalCheck, "Medal.Medal" },

                // Development Tools
                { VsCodeCheck, "Microsoft.VisualStudioCode" },
                { NotepadCheck, "Notepad++.Notepad++" },
                { ArduinoCheck, "Arduino.IDE.2" },

                // Microsoft Components
                { CopilotCheck, "Microsoft.Copilot" },
                { MsStoreCheck, "Microsoft.WindowsStore" },
                { XboxCheck, "Microsoft.XboxApp" },

                // System Tools
                { WinrarCheck, "RARLab.WinRAR" },
                { SevenZipCheck, "7zip.7zip" },
                { SignalRgbCheck, "WhirlwindFX.SignalRGB" },

                // Monitoring & Benchmarks
                { HwinfoCheck, "REALiX.HWiNFO" },
                { HwmonitorCheck, "CPUID.HWMonitor" },
                { CpuzCheck, "CPUID.CPU-Z" },
                { GpuzCheck, "TechPowerUp.GPU-Z" },
                { AfterburnerCheck, "MSI.Afterburner" },
                { FurmarkCheck, "Geeks3D.FurMark" },
                { CapframexCheck, "CapFrameX.CapFrameX" }
            };

            int selectedCount = 0;

            try
            {
                foreach (var app in appMappings)
                {
                    // Check if the control is valid and selected by user
                    if (app.Key != null && app.Key.IsChecked == true)
                    {
                        selectedCount++;
                        string appName = app.Key.Content?.ToString() ?? app.Value;

                        MainWindow.Log("WINGET", $"Processing: {appName}");

                        try
                        {
                            // Await the asynchronous installation from WingetService
                            await WingetService.InstallAppAsync(app.Value);
                            MainWindow.Log("SUCCESS", $"{appName} installation finished.");
                        }
                        catch (Exception ex)
                        {
                            MainWindow.Log("ERROR", $"Failed to install {appName}: {ex.Message}");
                        }
                    }
                }

                // Final status reporting
                if (selectedCount == 0)
                {
                    MainWindow.Log("WARNING", "No applications were selected.");
                }
                else
                {
                    MainWindow.Log("INSTALLER", $"Process completed. {selectedCount} applications processed.");
                }
            }
            catch (Exception ex)
            {
                MainWindow.Log("FATAL", $"Installer engine error: {ex.Message}");
            }
            finally
            {
                BtnInstall.IsEnabled = true;
            }
        }
    }
}