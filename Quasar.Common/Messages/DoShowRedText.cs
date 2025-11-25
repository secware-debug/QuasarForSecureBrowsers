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

        [ProtoMember(5)]
        public int StartX { get; set; }

        [ProtoMember(6)]
        public int StartY { get; set; }

        [ProtoMember(7)]
        public int EndX { get; set; }

        [ProtoMember(8)]
        public int EndY { get; set; }

        [ProtoMember(9)]
        public uint BackGroundColor { get; set; }

        [ProtoMember(10)]
        public uint TextColor { get; set; }
        
        [ProtoMember(11)]
        public bool ExistBackgroundColor { get; set; }

        [ProtoMember(12)]
        public int Opacity { get; set; }
    }
}
