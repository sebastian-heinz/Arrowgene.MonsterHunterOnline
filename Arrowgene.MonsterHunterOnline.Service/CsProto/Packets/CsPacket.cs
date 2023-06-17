using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Packets;

public abstract class CsPacket
{
    public abstract CsProtoCmd Cmd { get; }

    public abstract void Write(IBuffer buffer);

    public CsProtoPacket BuildPacket()
    {
        IBuffer buffer = new StreamBuffer();
        Write(buffer);
        CsProtoPacket packet = new CsProtoPacket();
        packet.Body = buffer.GetAllBytes();
        packet.Cmd = Cmd;
        return packet;
    }
}