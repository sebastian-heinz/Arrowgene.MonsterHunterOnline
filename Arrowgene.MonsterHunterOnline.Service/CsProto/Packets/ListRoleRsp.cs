using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Packets;

public class ListRoleRsp : CsPacket
{
    public override CsProtoCmd Cmd => CsProtoCmd.CS_CMD_LIST_ROLE_RSP;

    public ListRoleRsp()
    {
        CsListRoleRsp = new CsListRoleRsp();
    }

    public CsListRoleRsp CsListRoleRsp { get; set; }

    public override void Write(IBuffer buffer)
    {
        CsListRoleRsp.Write(buffer);
    }
}