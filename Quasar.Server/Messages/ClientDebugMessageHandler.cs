using Quasar.Common.Messages;
using Quasar.Common.Networking;
using Quasar.Server.Networking;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Quasar.Server.Messages
{
    public class ClientDebugMessageHandler : MessageProcessorBase<object>
    {
        private readonly Client _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientDebugMessageHandler"/> class using the given client.
        /// </summary>
        /// <param name="client">The associated client.</param>
        public ClientDebugMessageHandler(Client client) : base(true)
        {
            _client = client;
        }

        /// <inheritdoc />
        public override bool CanExecute(IMessage message) => message is DoSendDebugMessage;

        /// <inheritdoc />
        public override bool CanExecuteFrom(ISender client) => _client.Equals(client);

        /// <inheritdoc />
        public override void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case DoSendDebugMessage msg:
                    Execute(sender, msg);
                    break;
            }
        }

        private void Execute(ISender client, DoSendDebugMessage message)
        {
            SynchronizationContext.Post(c =>
            {

                MessageBox.Show(message.Message, "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }, client);
        }
    }

}
