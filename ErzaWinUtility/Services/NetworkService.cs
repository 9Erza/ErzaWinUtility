using System;
using System.Diagnostics;

namespace ErzaWinUtility.Services
{
    /// <summary>
    /// Service responsible for managing network configurations.
    /// Primarily handles DNS server assignment and DNS cache flushing via PowerShell.
    /// </summary>
    public static class NetworkService
    {
        /// <summary>
        /// Updates the DNS server addresses for all active network adapters.
        /// </summary>
        /// <param name="index">The index corresponding to a specific DNS provider (0 for DHCP Reset, 1 for Cloudflare, etc.).</param>
        public static void SetSystemDns(int index)
        {
            try
            {
                // Mapping selected index to specific DNS server pairs
                string? dnsServers = index switch
                {
                    1 => "'1.1.1.1','1.0.0.1'",              // Cloudflare
                    2 => "'94.140.14.14','94.140.15.15'",    // AdGuard
                    3 => "'9.9.9.9','149.112.112.112'",      // Quad9
                    4 => "'8.8.8.8','8.8.4.4'",              // Google
                    _ => null                                // DHCP / Automatic
                };

                string powershellCommand;

                if (dnsServers == null)
                {
                    // Command to reset DNS to automatic (DHCP)
                    powershellCommand = "Get-NetAdapter | Where-Object { $_.Status -eq 'Up' } | Set-DnsClientServerAddress -ResetServerAddresses";
                }
                else
                {
                    // Command to set static DNS servers for all active adapters
                    powershellCommand = $"Get-NetAdapter | Where-Object {{ $_.Status -eq 'Up' }} | Set-DnsClientServerAddress -ServerAddresses ({dnsServers})";
                }

                // Execute the primary command and clear the local DNS resolver cache
                ExecutePowerShell(powershellCommand);
                ExecutePowerShell("ipconfig /flushdns");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] DNS Change Failed: {ex.Message}");
                MainWindow.Log("NETWORK", $"Failed to update DNS: {ex.Message}");
            }
        }

        /// <summary>
        /// Helper method to execute PowerShell commands with elevated privileges.
        /// </summary>
        /// <param name="command">The PowerShell command string to execute.</param>
        private static void ExecutePowerShell(string command)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{command}\"",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = true, // Required for 'runas' verb
                    Verb = "runas"          // Request administrative elevation for network modifications
                };

                Process.Start(psi);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[FATAL] PowerShell execution error: {ex.Message}");
            }
        }
    }
}