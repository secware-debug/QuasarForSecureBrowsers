using ProtoBuf;

namespace Quasar.Common.Messages
{
    [ProtoContract]
    public class DoSendDebugMessage : IMessage
    {
        [ProtoMember(1)]
        public string Message { get; set; }
    }
}
