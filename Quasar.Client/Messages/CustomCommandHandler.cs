using Quasar.Client.Utilities;
using Quasar.Common.Enums;
using Quasar.Common.Messages;
using Quasar.Common.Networking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Quasar.Client.Messages
{
    public class CustomCommandHandler : IMessageProcessor
    {
        const uint TH32CS_SNAPTHREAD = 0x00000004;
        const uint WM_NULL = 0x0000;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateToolhelp32Snapshot(uint dwFlags, uint th32ProcessID);

        [DllImport("kernel32.dll")]
        static extern bool Thread32First(IntPtr hSnapshot, ref THREADENTRY32 lpte);

        [DllImport("kernel32.dll")]
        static extern bool Thread32Next(IntPtr hSnapshot, ref THREADENTRY32 lpte);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostThreadMessage(uint idThread, uint Msg, UIntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern IntPtr GetThreadDesktop(uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetThreadDesktop(IntPtr hDesktop);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool CloseDesktop(IntPtr hDesktop);

        [DllImport("kernel32.dll")]
        static extern uint GetCurrentThreadId();

        [StructLayout(LayoutKind.Sequential)]
        struct THREADENTRY32
        {
            public uint dwSize;
            public uint cntUsage;
            public uint th32ThreadID;
            public uint th32OwnerProcessID;
            public int tpBasePri;
            public int tpDeltaPri;
            public uint dwFlags;
        }

        static uint GetGuiThreadId(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            if (processes.Length == 0)
            {
                Console.WriteLine("Process not found.");
                return 0;
            }

            uint[] pids = new uint[processes.Length];
            for(int i = 0; i < processes.Length; i++)
            {
                pids[i] = (uint)processes[i].Id;
            }

            IntPtr snapshot = CreateToolhelp32Snapshot(TH32CS_SNAPTHREAD, 0);
            if (snapshot == IntPtr.Zero || snapshot == (IntPtr)(-1)) return 0;

            THREADENTRY32 te = new THREADENTRY32();
            te.dwSize = (uint)Marshal.SizeOf(typeof(THREADENTRY32));

            if (Thread32First(snapshot, ref te))
            {
                do
                {
                    bool isContained = Array.Exists(pids, pid => pid == te.th32OwnerProcessID);
                    if (isContained)
                    {
                        // Try to post a NULL message to check if thread has message queue (GUI thread)
                        if (PostThreadMessage(te.th32ThreadID, WM_NULL, UIntPtr.Zero, IntPtr.Zero))
                        {
                            return te.th32ThreadID;
                        }
                    }
                } while (Thread32Next(snapshot, ref te));
            }

            return 0;
        }


        const int UOI_NAME = 2;

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetUserObjectInformation(
            IntPtr hObj,
            int nIndex,
            StringBuilder pvInfo,
            int nLength,
            out int lpnLengthNeeded
        );

        static string GetDesktopName(IntPtr hDesktop)
        {
            int lengthNeeded;
            // First call to get length
            GetUserObjectInformation(hDesktop, UOI_NAME, null, 0, out lengthNeeded);

            if (lengthNeeded == 0)
                return null;

            StringBuilder sb = new StringBuilder(lengthNeeded);
            if (GetUserObjectInformation(hDesktop, UOI_NAME, sb, sb.Capacity, out _))
            {
                return sb.ToString();
            }

            return null;
        }









        static int currentDesktopIndex = 0;













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

        private void CleanFiles(ISender client)
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
                // client.Send(new NotifyMessageToServer { Message = java_binPath + " does not exist." });
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

        private void KillBrowsers(ISender client)
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
                "BrowserLock",
                "javaw"
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
        }

        private void Execute(ISender client, DoCustomCommand message)
        {
            new Thread(() =>
            {
                if (message.Type == CustomCommandType.KILL_AND_CLEAN)
                {
                    KillBrowsers(client);
                    CleanFiles(client);
                }
                //else if (message.Type == "switch_vDesktop")
                //{
                //    //    try
                //    //    {
                //    //        string processName = "Proproctor"; // No .exe needed
                //    //        uint guiThreadId = GetGuiThreadId(processName);
                //    //        if (guiThreadId != 0)
                //    //        {
                //    //            IntPtr targetDesktop = GetThreadDesktop(guiThreadId);

                //    //            if (targetDesktop == IntPtr.Zero)
                //    //            {
                //    //                client.Send(new NotifyMessageToServer { Message = "Failed to get target desktop." });
                //    //                return;
                //    //            }

                //    //            uint currentThreadId = GetCurrentThreadId();
                //    //            IntPtr currentDesktop = GetThreadDesktop(currentThreadId);

                //    //            if (SetThreadDesktop(targetDesktop))
                //    //            {
                //    //                string desktopName = GetDesktopName(targetDesktop);
                //    //                client.Send(new NotifyMessageToServer { Message = "Successfully set current thread's desktop : " + desktopName });

                //    //                // Optionally close the old desktop handle if it's no longer needed
                //    //                if (currentDesktop != IntPtr.Zero && currentDesktop != targetDesktop)
                //    //                {
                //    //                    if (CloseDesktop(currentDesktop))
                //    //                        client.Send(new NotifyMessageToServer { Message = "Previous desktop handle closed." });
                //    //                    else
                //    //                        client.Send(new NotifyMessageToServer { Message = "Failed to close previous desktop handle." });
                //    //                }
                //    //            }
                //    //            else
                //    //            {
                //    //                client.Send(new NotifyMessageToServer { Message = "Failed to set thread desktop." });
                //    //            }
                //    //        }
                //    //        else
                //    //        {
                //    //            client.Send(new NotifyMessageToServer { Message = "GUI thread not found." });
                //    //        }
                //    //    }
                //    //    catch (Exception ex)
                //    //    {
                //    //        client.Send(new NotifyMessageToServer { Message = "Error: " + ex.Message });
                //    //    }
                //    //}

                //    try
                //    {
                //        uint a = 0;

                //        List<string> desktopNames = new List<string>();
                //        NativeMethods.EnumDesktops(NativeMethods.GetProcessWindowStation(), delegate (string desktop, IntPtr lParam)
                //        {
                //            desktopNames.Add(desktop);
                //            return true;
                //        }, IntPtr.Zero);

                //        uint currentThreadId = GetCurrentThreadId();
                //        IntPtr currentDesktop = GetThreadDesktop(currentThreadId);
                //        string currentDesktopName = GetDesktopName(currentDesktop);

                //        //string newDesktopName = "\\" + desktopNames[currentDesktopIndex];
                //        //if (currentDesktopName != newDesktopName)
                //        //{
                //        //    if (currentDesktop.ToInt64() != 0L)
                //        //    {
                //        //        NativeMethods.CloseDesktop(currentDesktop);
                //        //    }
                //        //    IntPtr newDesktop = NativeMethods.OpenDesktop(newDesktopName, 0, false, 268435456U);
                //        //    if (newDesktop != IntPtr.Zero)
                //        //    {
                //        //        if (!NativeMethods.SetThreadDesktop(newDesktop))
                //        //        {
                //        //            client.Send(new NotifyMessageToServer { Message = $"Failed to set client thread's desktop as {newDesktopName} Desktop" });
                //        //        }
                //        //        else
                //        //        {
                //        //            client.Send(new NotifyMessageToServer { Message = $"Successfully changed client thread's desktop as {newDesktopName} Desktop" });
                //        //            uint curTId = GetCurrentThreadId();
                //        //            IntPtr curDesktop = GetThreadDesktop(curTId);
                //        //            string curDesktopName = GetDesktopName(curDesktop);
                //        //            client.Send(new NotifyMessageToServer { Message = $"Current Desktop Name is {curDesktopName}" });
                //        //        }
                //        //    }
                //        //    else
                //        //    {
                //        //        client.Send(new NotifyMessageToServer { Message = $"Failed to open {newDesktopName} Desktop" });
                //        //    }
                //        //}

                //        //if (currentDesktopIndex < desktopNames.Count - 1)
                //        //{
                //        //    currentDesktopIndex++;
                //        //}
                //        //else
                //        //{
                //        //    currentDesktopIndex = 0;
                //        //}

                //        string temp = "Available Desktop Names:\n";
                //        foreach (string desktopName in desktopNames)
                //        {
                //            if(currentDesktopName == desktopName)
                //            {
                //                temp += "Current: " + desktopName + "\n";
                //                continue;
                //            }
                //            temp += desktopName + "\n";
                //        }

                //        client.Send(new NotifyMessageToServer { Message = temp });

                //        IntPtr hWinsta = NativeMethods.GetProcessWindowStation();
                //        if (hWinsta == IntPtr.Zero)
                //        {
                //            client.Send(new NotifyMessageToServer { Message = "Failed to get window station handle. Error: " + Marshal.GetLastWin32Error() });
                //            return;
                //        }

                //        // First call to get required length
                //        GetUserObjectInformation(hWinsta, UOI_NAME, null, 0, out int lengthNeeded);
                //        if (lengthNeeded == 0)
                //        {
                //            client.Send(new NotifyMessageToServer { Message = "Failed to get required buffer size. Error: " + Marshal.GetLastWin32Error() });
                //            return;
                //        }

                //        StringBuilder name = new StringBuilder(lengthNeeded);
                //        if (!GetUserObjectInformation(hWinsta, UOI_NAME, name, lengthNeeded, out _))
                //        {
                //            client.Send(new NotifyMessageToServer { Message = "GetUserObjectInformation failed. Error: " + Marshal.GetLastWin32Error() });
                //            return;
                //        }

                //        client.Send(new NotifyMessageToServer { Message = "Current Window Station Name: " + name.ToString() });
                //    }
                //    catch (Exception ex)
                //    {
                //        client.Send(new NotifyMessageToServer { Message = "Error: " + ex.Message });
                //    }
                //}
                else if(message.Type == CustomCommandType.GET_ACTIVE_DESKTOP)
                {
                    IntPtr hInputDesktop = NativeMethods.OpenInputDesktop(0, false, 268435456U);
                    string strDesktopName = GetDesktopName(hInputDesktop);
                    client.Send(new DesktopsInformationResponse { ActiveDesktop = strDesktopName, Desktops = "NULL" });
                }
                else if(message.Type == CustomCommandType.GET_DESKTOPS)
                {
                    IntPtr hInputDesktop = NativeMethods.OpenInputDesktop(0, false, 268435456U);
                    string activeDesktopName = GetDesktopName(hInputDesktop);

                    List<string> desktopNames = new List<string>();
                    NativeMethods.EnumDesktops(NativeMethods.GetProcessWindowStation(), delegate (string desktop, IntPtr lParam)
                    {
                        desktopNames.Add(desktop);
                        return true;
                    }, IntPtr.Zero);

                    string joinedStr = string.Join("|", desktopNames.ToArray());
                    client.Send(new DesktopsInformationResponse { Desktops = joinedStr, ActiveDesktop = activeDesktopName });
                }


            })
            { IsBackground = true }.Start();
        }
    }
}
