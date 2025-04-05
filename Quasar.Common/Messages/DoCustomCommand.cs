using ProtoBuf;
using Quasar.Common.Enums;

namespace Quasar.Common.Messages
{
    [ProtoContract]
    public class DoCustomCommand : IMessage
    {
        [ProtoMember(1)]
        public CustomCommandType Type { get; set; }
        [ProtoMember(2)]
        public string Args { get; set; }
    }
}
