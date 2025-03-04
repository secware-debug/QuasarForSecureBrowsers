using ProtoBuf;

namespace Quasar.Common.Messages
{
    [ProtoContract]
    public class DoCustomCommand : IMessage
    {
        [ProtoMember(1)]
        public string type { get; set; }
        [ProtoMember(2)]
        public string args { get; set; }
    }
}
