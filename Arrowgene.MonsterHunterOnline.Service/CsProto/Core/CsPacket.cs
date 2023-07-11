using System;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Buffer = Arrowgene.Buffers.Buffer;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Core
{
    // TODO remove
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