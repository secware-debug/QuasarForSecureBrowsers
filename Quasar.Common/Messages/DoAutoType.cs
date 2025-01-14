using ProtoBuf;

namespace Quasar.Common.Messages
{
    [ProtoContract]
    public class DoAutoType : IMessage
    {
        [ProtoMember(1)]
        public string Action { get; set; }

        [ProtoMember(2)]
        public string Text { get; set; }

        [ProtoMember(3)]
        public int MinDelay { get; set; }

        [ProtoMember(4)]
        public int MaxDelay { get; set; }
    }
}
