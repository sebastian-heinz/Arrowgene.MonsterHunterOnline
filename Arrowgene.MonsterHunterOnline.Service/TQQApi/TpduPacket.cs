using System;
using System.Text;
using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.TQQApi
{
    /// <summary>
    /// Transport Protocol Data Unit
    /// </summary>
    public class TpduPacket
    {
        public const int HeaderLen = 12;
        public const int MAGIC = 85;
        public const int VERSION = 14;

        public TpduPacket()
        {
            Magic = MAGIC;
            Version = VERSION;
            EncHeadLen = 0;
            HeadLen = 0;
            BodyLen = 0;
            Header = Array.Empty<byte>();
            HeaderExt = Array.Empty<byte>();
            EncHead = Array.Empty<byte>();
            Body = Array.Empty<byte>();
        }

        public byte Magic { get; set; }
        public byte Version { get; set; }
        public TpduCmd Cmd { get; set; }
        public byte EncHeadLen { get; set; }
        public int HeadLen { get; set; }
        public int BodyLen { get; set; }
        public byte[] HeaderExt { get; set; }
        public byte[] EncHead { get; set; }
        public byte[] Body { get; set; }
        public byte[] Header { get; set; }

        public string PrintHeader()
        {
            return
                $"Magic:{Magic} Version:{Version} Cmd:{Cmd} EncHeadLen:{EncHeadLen} HeadLen:{HeadLen} BodyLen:{BodyLen}";
        }

        public string PrintData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Header:");
            sb.Append(Environment.NewLine);
            sb.Append(Util.HexDump(Header));
            sb.Append("HeaderExt:");
            sb.Append(Environment.NewLine);
            sb.Append(Util.HexDump(HeaderExt));
            sb.Append("EncHead:");
            sb.Append(Environment.NewLine);
            sb.Append(Util.HexDump(EncHead));
            sb.Append("Body:");
            sb.Append(Environment.NewLine);
            sb.Append(Util.HexDump(Body));
            return sb.ToString();
        }

        public IBuffer NewHeaderExtBuffer()
        {
            IBuffer buf = new StreamBuffer(HeaderExt);
            buf.SetPositionStart();
            return buf;
        }

        public override string ToString()
        {
            return $"{PrintHeader()}{Environment.NewLine}{PrintData()}";
        }
    }
}