using ProtoBuf;
using Quasar.Common.Enums;

namespace Quasar.Common.Messages
{
    [ProtoContract]
    public class DoUnblockScreenCapture : IMessage
    {
        [ProtoMember(1)]
        public BrowserTypeToUnblock browserType { get; set; }
    }
}