using System;
using System.Text;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto
{
    public class CsProtoPacket
    {
        public const int HeaderLen = 16;

        public CsProtoPacket()
        {
            HeadLen = 0;
            BodyLen = 0;
            SeqId = 0;
            NoUse = 0;
            Header = Array.Empty<byte>();
            Body = Array.Empty<byte>();
        }

        public CsProtoCmd Cmd { get; set; }
        public ushort HeadLen { get; set; }
        public uint BodyLen { get; set; }
        public uint SeqId { get; set; }
        public uint NoUse { get; set; }
        public byte[] Header { get; set; }
        public byte[] Body { get; set; }

        public string PrintHeader()
        {
            return
                $"Cmd:{Cmd} HeadLen:{HeadLen} BodyLen:{BodyLen} SeqId:{SeqId} NoUse:{NoUse}";
        }

        public string PrintData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Header:");
            sb.Append(Environment.NewLine);
            sb.Append(Util.HexDump(Header));
            sb.Append("Body:");
            sb.Append(Environment.NewLine);
            sb.Append(Util.HexDump(Body));
            return sb.ToString();
        }

        public override string ToString()
        {
            return $"{PrintHeader()}{Environment.NewLine}{PrintData()}";
        }
    }
}