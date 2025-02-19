using Quasar.Common.Messages;
using Quasar.Common.Networking;
using System;
using System.Threading;

using Quasar.Client.Interception;
using Quasar.Common.Enums;
using Quasar.Client.Helper;
using Quasar.Client.Utilities;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace Quasar.Client.Messages
{
    public class CaptureUnblockHandler : IMessageProcessor
    {
        public bool CanExecute(IMessage message) => message is DoUnblockScreenCapture;

        public bool CanExecuteFrom(ISender sender) => true;

        public void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case DoUnblockScreenCapture msg:
                    Execute(sender, msg);
                    break;
            }
        }

        public void Execute(ISender client, DoUnblockScreenCapture message)
        {
            new Thread(delegate ()
            {
                switch (message.browserType)
                {
                    case BrowserTypeToUnblock.ONVUE:
                        UnblockOnVUE(client, message);
                        break;
                    case BrowserTypeToUnblock.RESPONDUS:
                        UnblockRespondus(client);
                        break;
                    case BrowserTypeToUnblock.ETS:
                        UnblockETS(client, message);
                        break;
                    default:
                        break;
                }
            }).Start();

        }

        private void UnblockOnVUE(ISender client, DoUnblockScreenCapture message)
        {
            if (message.args == "inject_x86")
            {
                Process[] processes = Process.GetProcessesByName("javaw");

                string dllPath = "";
                if (processes.Length == 0)
                {
                    client.Send(new NotifyMessageToServer { Message = "Can't find javaw.exe process" });
                    return;
                }
                byte[] realUnblockScreenshotx86 = Resource1.UnblockScreenshot_OnVue_x86;
                dllPath = Path.Combine(new string[]
                {
                    Path.GetTempPath(),
                    "JavaUpdater_jdk18_x86.dll"
                });
                try
                {
                    File.WriteAllBytes(dllPath, realUnblockScreenshotx86);
                }
                catch (Exception ex5)
                {
                    client.Send(new NotifyMessageToServer { Message = string.Format("{0}:\n\n{1}", "Writing 32bit injection dll error", ex5.Message) });
                }                
                try
                {
                    string[] ss = DllInjectionHelper.InjectManualMap("javaw", dllPath, true);
                    string combinedString = string.Join("\n", ss);
                    client.Send(new NotifyMessageToServer { Message = "javaw injection result:\n" + combinedString });
                }
                catch (Exception ex6)
                {
                    client.Send(new NotifyMessageToServer { Message = string.Format("error injection to javaw: {0}", ex6.Message) });
                }
            }
            else
            {
                string java_binPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "OnVUE",
                    "runtime",
                    "java-runtime",
                    "bin"
                ); 
                string javawPath = Path.Combine(new string[]
                {
                    java_binPath,
                    "javaw.exe"
                });

                if (!File.Exists(javawPath))
                {
                    client.Send(new NotifyMessageToServer { Message = "Can't find browserlock on target machine!\n" + javawPath });
                    return;
                }
                string dllPath = "";
                byte[] versionDLLX64 = Resource1.version_OnVue_x64;
                dllPath = Path.Combine(new string[]
                {
                    java_binPath,
                    "version.dll"
                });
                try
                {
                    File.WriteAllBytes(dllPath, versionDLLX64);
                    client.Send(new NotifyMessageToServer { Message = "Successfully wrote the 64 bit hijack DLL." });
                }
                catch (Exception ex5)
                {
                    client.Send(new NotifyMessageToServer { Message = string.Format("{0}:\n\n{1}", "Writing 64 bit hijack DLL error", ex5.Message) });
                }
            }
            
        }

        private void UnblockRespondus(ISender client)
        {
            string stdStr = ""; 
            bool result = DllInjectionHelper.InjectNormalUnblockDll("LockDownBrowser", "JavaUpdater_jdk20", client, out stdStr);
            if (stdStr == "") stdStr = "OK";
            if (result)
            {
                client.Send(new NotifyMessageToServer { Message = "Injection result: " + stdStr });
            }

            //byte[] realUnblockScreenshot_x3 = Resource1.UnblockScreenshot_x86;
            //string text3 = Path.Combine(new string[]
            //{
            //                Path.GetTempPath(),
            //                "JavaUpdater_jdk20.dll"
            //});
            //bool flag2 = false;
            //try
            //{
            //    File.WriteAllBytes(text3, realUnblockScreenshot_x3);
            //}
            //catch (Exception ex5)
            //{
            //    flag2 = true;
            //    client.Send(new NotifyMessageToServer { Message = string.Format("{0}:{1}\n\n{2}", Misc.__FILE__("D:\\Projects\\Quasar-AllInOne\\Quasar-Modified\\Client\\Core\\Commands\\MiscHandler.cs"), Misc.__LINE__(244), ex5.Message) });
            //}
            //try
            //{
            //    DllInjectionHelper.Inject("LockDownBrowser", text3, client);
            //}
            //catch (Exception ex6)
            //{
            //    flag2 = true;
            //    client.Send(new NotifyMessageToServer { Message = string.Format("{0}:{1}\n\n{2}", Misc.__FILE__("D:\\Projects\\Quasar-AllInOne\\Quasar-Modified\\Client\\Core\\Commands\\MiscHandler.cs"), Misc.__LINE__(253), ex6.Message) });
            //}
            //if (!flag2)
            //{
            //    client.Send(new NotifyMessageToServer { Message = string.Format("{0}:{1}\n\n{2}", Misc.__FILE__("D:\\Projects\\Quasar-AllInOne\\Quasar-Modified\\Client\\Core\\Commands\\MiscHandler.cs"), Misc.__LINE__(257), "Injected! We good") });
            //    return;
            //}
        }

        private void UnblockETS(ISender client, DoUnblockScreenCapture msg)
        {
            string text15 = "C:\\ProgramData\\ETS\\IBT2\\rp\\bin\\onlinetestwks.exe";
            string text16 = "C:\\ProgramData\\ETS\\IBT2\\rp\\bin\\onlinetestwks_o.exe";
            string resultStr;
            if (msg.args == "unblock")
            {
                bool result = false;
                try
                {
                    result = DllInjectionHelper.InjectNormalUnblockDll("onlinetestwks", "JavaUpdater_jdk21", client, out resultStr);
                }
                catch (Exception ex1)
                {
                    client.Send(new NotifyMessageToServer { Message = "Injection failed to onlinetestwks: " + ex1.Message });
                }

                try
                {
                    result = DllInjectionHelper.InjectNormalUnblockDll("etschrome", "JavaUpdater_jdk21_", client, out resultStr);
                }
                catch (Exception ex1)
                {
                    client.Send(new NotifyMessageToServer { Message = "Injection failed to etschrome: " + ex1.Message });
                }

                if (result)
                {
                    client.Send(new NotifyMessageToServer { Message = "Injected Normal Unblock dll to onlinetestwks and etschrome" });
                    return;
                }
            }
            else if(msg.args == "patch")
            {
                if (File.Exists(text15))
                {
                    if (!Misc.CheckSigned(text15))
                    {
                        client.Send(new NotifyMessageToServer { Message = "AutoPatchETS: info: ETS is already PATCHED" });
                        return;
                    }
                    try
                    {
                        File.Copy(text15, text16, true);
                    }
                    catch (Exception ex)
                    {
                        client.Send(new NotifyMessageToServer { Message = "cloning the original onlinetestwks.exe file error: " + ex.Message });
                    }
                    string text17 = Path.Combine(new string[]
                    {
                        Path.GetTempPath(),
                        "ETSP.exe"
                    });
                    try
                    {
                        File.WriteAllBytes(text17, Resource1.AutoPatchETSStatic);
                    }
                    catch (Exception)
                    {
                    }
                    string text18;
                    string text19;
                    Misc.CheckOutput(text17, "\"" + text16 + "\"", out text18, out text19);
                    text18 = text18.Trim();
                    text19 = text19.Trim();
                    if (text18.Length > 0)
                    {
                        client.Send(new NotifyMessageToServer { Message = "AutoPatchETS: stdout:\n" + text18 });
                    }
                    if (text19.Length > 0)
                    {
                        client.Send(new NotifyMessageToServer { Message = "AutoPatchETS: stdout:\n" + text18 });

                        client.Send(new NotifyMessageToServer { Message = "AutoPatchETS:\n-------> If you see this, most likely the patching is failed. Please do manual patching instead\n\n[+] Stderr:\n" + text19 });
                    }
                    try
                    {
                        File.Delete(text17);
                        return;
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
                client.Send(new NotifyMessageToServer { Message = "AutoPatchETS: info: ETS is not installed" });
                return;
            }
            else if(msg.args == "restore")
            {
                if (File.Exists(text16))
                {
                    bool flag9 = false;
                    try
                    {
                        Process[] processesByName = Process.GetProcessesByName("onlinetestwks");
                        for (int i = 0; i < processesByName.Length; i++)
                        {
                            processesByName[i].Kill();
                        }
                        processesByName = Process.GetProcessesByName("etschrome");
                        for (int i = 0; i < processesByName.Length; i++)
                        {
                            processesByName[i].Kill();
                        }
                        Thread.Sleep(2000);
                        File.Delete(text15);
                        File.Move(text16, text15);
                    }
                    catch (Exception ex21)
                    {
                        flag9 = true;
                        client.Send(new NotifyMessageToServer { Message = "AutoPatchETS: error: RESTORE failed. Message: " + ex21.Message });
                    }
                    if (!flag9)
                    {
                        client.Send(new NotifyMessageToServer { Message = "AutoPatchETS: info: RESTORE sucessfully" });
                        return;
                    }
                }
                else
                {
                    if (File.Exists(text15))
                    {
                        client.Send(new NotifyMessageToServer { Message = "AutoPatchETS: info: ETS is already RESTORED" });
                        return;
                    }
                    client.Send(new NotifyMessageToServer { Message = "AutoPatchETS: info: ETS is not installed" });
                    return;
                }
            }
            //else if(msg.args == "unblock_child")
            //{
            //    byte[] realUnblockScreenshot_x3 = Resource1.UnblockScreenshot_ChildWindow_x86;
            //    string text3 = Path.Combine(new string[]
            //    {
            //        Path.GetTempPath(),
            //        "JavaUpdater_jdk21_child_x86.dll"
            //    });
            //    try
            //    {
            //        File.WriteAllBytes(text3, realUnblockScreenshot_x3);
            //    }
            //    catch (Exception ex5)
            //    {
            //        client.Send(new NotifyMessageToServer { Message = string.Format("{0}:{1}\n\n{2}", Misc.__FILE__("D:\\Projects\\Quasar-AllInOne\\Quasar-Modified\\Client\\Core\\Commands\\MiscHandler.cs"), Misc.__LINE__(244), ex5.Message) });
            //    }
            //    try
            //    {
            //        string[] ss = DllInjectionHelper.InjectManualMap("onlinetestwks", text3, true);
            //        string combinedString = string.Join("\n", ss);
            //        client.Send(new NotifyMessageToServer { Message = "onlinetestwks injection result:\n" + combinedString });
            //    }
            //    catch (Exception ex6)
            //    {
            //        client.Send(new NotifyMessageToServer { Message = string.Format("error injection to onlinetestwks: {0}", ex6.Message) });
            //    }
            //    try
            //    {
            //        string[] ss = DllInjectionHelper.InjectManualMap("etschrome", text3, true);
            //        string combinedString = string.Join("\n", ss);
            //        client.Send(new NotifyMessageToServer { Message = "etschrome injection result:\n" + combinedString });
            //    }
            //    catch (Exception ex6)
            //    {
            //        client.Send(new NotifyMessageToServer { Message = string.Format("error injection to etschrome: {0}", ex6.Message) });
            //    }
            //}
            return;
        }
    }
}
