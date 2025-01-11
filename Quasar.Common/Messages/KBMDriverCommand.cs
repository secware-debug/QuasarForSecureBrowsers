using ProtoBuf;

namespace Quasar.Common.Messages
{
    [ProtoContract]
    public class KBMDriverCommand : IMessage
    {
        [ProtoMember(1)]
        public string action { get; set; }
    }
}
