

using ProtoBuf;

namespace Quasar.Common.Messages
{
    [ProtoContract]
    public class DesktopsInformationResponse : IMessage
    {
        [ProtoMember(1)]
        public string Workstations { get; set; }

        [ProtoMember(2)]
        public string Desktops { get; set; }

        [ProtoMember(3)]
        public string ActiveWorkstation { get; set; }

        [ProtoMember(4)]
        public string ActiveDesktop { get; set; }
    }
}
