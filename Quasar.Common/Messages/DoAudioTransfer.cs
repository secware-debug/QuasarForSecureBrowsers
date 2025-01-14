using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quasar.Common.Messages
{
    [ProtoContract]
    public class DoAudioTransfer : IMessage
    {
        [ProtoMember(1)]
        public string Action { get; set; }

        [ProtoMember(2)]
        public string IpAddress { get; set; }

        [ProtoMember(3)]
        public int Port { get; set; }

        [ProtoMember(4)]
        public int SampleRate { get; set; }

        [ProtoMember(5)]
        public int BitDepth { get; set; }

        [ProtoMember(6)]
        public int Channels { get; set; }
    }
}
