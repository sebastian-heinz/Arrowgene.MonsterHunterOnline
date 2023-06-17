using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Packets;

public class TownSessionStart : CsPacket
{
    public override CsProtoCmd Cmd => CsProtoCmd.CS_CMD_TOWN_SESSION_START;

    public TownSessionStart()
    {
        ErrNo = 0;
    }

    public byte ErrNo { get; set; }
    
    public override void Write(IBuffer buffer)
    {
        buffer.WriteByte(ErrNo);
    }
}