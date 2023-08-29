using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class DeleteRoleReq : Structure, ICsStructure
{
    public DeleteRoleReq()
    {
        RoleIndex = 0;
    }

    public int RoleIndex { get; set; }

    public  void WriteCs(IBuffer buffer)
    {
        WriteInt32(buffer, RoleIndex);
    }

    public void ReadCs(IBuffer buffer)
    {
        RoleIndex = ReadInt32(buffer);
    }
}