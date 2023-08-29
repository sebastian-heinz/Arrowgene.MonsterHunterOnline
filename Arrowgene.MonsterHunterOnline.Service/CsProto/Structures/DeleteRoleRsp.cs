using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class DeleteRoleRsp : Structure, ICsStructure
{
    public DeleteRoleRsp()
    {
        RoleState = 0;
        RoleStateEndLeftTime = 0;
    }

    public int RoleState { get; set; }
    public uint RoleStateEndLeftTime { get; set; }

    public  void WriteCs(IBuffer buffer)
    {
        WriteInt32(buffer, RoleState);
        WriteUInt32(buffer, RoleStateEndLeftTime);
    }

    public void ReadCs(IBuffer buffer)
    {
        RoleState = ReadInt32(buffer);
        RoleStateEndLeftTime = ReadUInt32(buffer);
    }
}