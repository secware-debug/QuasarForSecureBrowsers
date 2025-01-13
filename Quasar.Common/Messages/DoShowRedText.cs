using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quasar.Common.Messages
{
    [ProtoContract]
    public class DoShowRedText : IMessage
    {
        [ProtoMember(1)]
        public string Action { get; set; }

        [ProtoMember(2)]
        public string Text { get; set; }

        [ProtoMember(3)]
        public string WindowClassName { get; set; }

        [ProtoMember(4)]
        public int FontSize { get; set; }
    }
}
