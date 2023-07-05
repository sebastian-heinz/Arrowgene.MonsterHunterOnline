using System;
using System.Text;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Core
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
            Source = PacketSource.Unknown;
        }

        public CS_CMD_ID Cmd { get; set; }
        public PacketSource Source { get; set; }
        public ushort HeadLen { get; set; }
        public uint BodyLen { get; set; }
        public uint SeqId { get; set; }
        public uint NoUse { get; set; }
        public byte[] Header { get; set; }
        public byte[] Body { get; set; }

        public bool WrappedCrypto { get; set; }

        public string PrintHeader()
        {
            return
                $"Source:{Source} Cmd:{Cmd} HeadLen:{HeadLen} BodyLen:{BodyLen} SeqId:{SeqId} NoUse:{NoUse}";
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

        public IBuffer NewBuffer()
        {
            IBuffer buf = new StreamBuffer(Body);
            buf.SetPositionStart();
            return buf;
        }
    }
}