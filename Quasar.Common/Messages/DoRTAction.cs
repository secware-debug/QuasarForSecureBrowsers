using ProtoBuf;

namespace Quasar.Common.Messages
{
    [ProtoContract]
    public class DoRTAction : IMessage
    {
        [ProtoMember(1)]
        public string Action { get; set; }
    }
}
