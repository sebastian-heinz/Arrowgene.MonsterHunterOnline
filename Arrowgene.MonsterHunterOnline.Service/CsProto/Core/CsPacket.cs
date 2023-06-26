using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Core
{
    public abstract class CsPacket
    {
        public abstract CS_CMD_ID Cmd { get; }

        public abstract void Write(IBuffer buffer);

        public CsProtoPacket BuildPacket()
        {
            Buffer buffer = new StreamBuffer();
            Write(buffer);
            CsProtoPacket packet = new CsProtoPacket();
            packet.Body = buffer.GetAllBytes();
            packet.Cmd = Cmd;
            return packet;
        }
    }
}