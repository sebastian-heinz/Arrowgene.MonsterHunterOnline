using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class DeleteRoleReq : Structure
{
    public DeleteRoleReq()
    {
        RoleIndex = 0;
    }

    public int RoleIndex { get; set; }

    public override void Write(IBuffer buffer)
    {
        WriteInt32(buffer, RoleIndex);
    }

    public override void Read(IBuffer buffer)
    {
        RoleIndex = ReadInt32(buffer);
    }
}