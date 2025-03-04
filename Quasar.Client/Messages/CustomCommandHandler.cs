using Quasar.Client.Utilities;
using Quasar.Common.Messages;
using Quasar.Common.Networking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Quasar.Client.Messages
{
    public class CustomCommandHandler : IMessageProcessor
    {
        public bool CanExecute(IMessage message) => message is DoCustomCommand;

        public bool CanExecuteFrom(ISender sender) => true;

        public void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case DoCustomCommand msg:
                    Execute(sender, msg);
                    break;
                default:
                    break;
            }
        }

        private void Execute(ISender client, DoCustomCommand message)
        {

            new Thread(() =>
            {
                if (message.type == "killbrowser")
                {
                    string[] array3 = new string[]
                    {
                        "LockDownBrowser",
                        "onlinetestwks",
                        "etschrome",
                        "TOEFL Secure Browser",
                        "ProProctor",
                        "LockDownBrowserOEM",
                        "PSI Bridge Secure Browser",
                        "Bluebook",
                        "BrowserLock"
                    };
                    List<string> list = new List<string>();
                    foreach (string text12 in array3)
                    {
                        foreach (Process process in Process.GetProcessesByName(text12))
                        {
                            try
                            {
                                process.Kill();
                                if (!list.Contains(text12))
                                {
                                    list.Add(text12);
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    if (list.Count > 0)
                    {
                        string text13 = string.Format("Killed {0} processes\n", list.Count);
                        for (int l = 0; l < list.Count; l++)
                        {
                            text13 = text13 + "  - " + list[l] + ".exe";
                            if (l != list.Count - 1)
                            {
                                text13 += "\n";
                            }
                        }
                        client.Send(new NotifyMessageToServer { Message = text13 });
                        return;
                    }
                    client.Send(new NotifyMessageToServer { Message = string.Format("No browser left to kill ({0})", list.Count) });
                    return;
                }
                else if (message.type == "cleandlls")
                {
                    // Path for the specific version.dll file
                    string java_binPath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        "OnVUE",
                        "runtime",
                        "java-runtime",
                        "bin", "version.dll"
                    );

                    // Check and delete version.dll if it exists
                    if (File.Exists(java_binPath))
                    {
                        try
                        {
                            File.Delete(java_binPath);
                            client.Send(new NotifyMessageToServer { Message = $"Deleted: {java_binPath}" });
                        }
                        catch (Exception ex)
                        {
                            client.Send(new NotifyMessageToServer { Message = "Error deleting " + java_binPath + ": " + ex.Message });
                        }
                    }
                    else
                    {
                        client.Send(new NotifyMessageToServer { Message = java_binPath + " does not exist." });
                    }

                    // Get the system temporary directory
                    string tempDirectory = Path.GetTempPath();
                    // MessageBox.Show(tempDirectory);
                    // Find and delete DLL files starting with "JavaUpdater_jdk"
                    var files = Directory.GetFiles(tempDirectory, "JavaUpdater_jdk*.dll");
                    foreach (var file in files)
                    {
                        try
                        {
                            File.Delete(file);
                            client.Send(new NotifyMessageToServer { Message = $"Deleted: {file}" });
                        }
                        catch (Exception ex)
                        {
                            client.Send(new NotifyMessageToServer { Message = "Error deleting " + file + ": " + ex.Message });
                        }
                    }

                    if (files.Length == 0)
                    {
                        client.Send(new NotifyMessageToServer { Message = "No JavaUpdater_jdk DLL files found in the temp directory." });
                    }
                }
            })
            { IsBackground = true }.Start();
        }
    }
}
