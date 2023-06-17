using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Packets;

/// <summary>
/// 玩家初始化
/// </summary>
public class PlayerInitNtf : CsPacket
{
    public override CsProtoCmd Cmd => CsProtoCmd.CS_CMD_PLAYER_INIT_NTF;

    public PlayerInitNtf()
    {
        CSPlayerInitInfo = new CsPlayerInitInfo();
    }

    public CsPlayerInitInfo CSPlayerInitInfo { get; set; }

    public override void Write(IBuffer buffer)
    {
        CSPlayerInitInfo.Write(buffer);
    }
}