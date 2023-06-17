using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Packets;

public class PlayerLevelInitNtf : CsPacket
{
    public override CsProtoCmd Cmd => CsProtoCmd.CS_CMD_PLAYER_LEVEL_INIT_NTF;

    public PlayerLevelInitNtf()
    {
        CsPlayerLevelInitInfo = new CsPlayerLevelInitInfo();
    }

    public CsPlayerLevelInitInfo CsPlayerLevelInitInfo { get; set; }

    public override void Write(IBuffer buffer)
    {
        CsPlayerLevelInitInfo.Write(buffer);
    }
}