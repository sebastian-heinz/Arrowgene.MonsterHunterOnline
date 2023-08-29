using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class AccountReq : Structure, ICsStructure
{
    public AccountReq()
    {
        RoleIndex = 0;
        Reserved = 0;
    }

    public int RoleIndex { get; set; }
    public int Reserved { get; set; }

    public void WriteCs(IBuffer buffer)
    {
        WriteInt32(buffer, RoleIndex);
        WriteInt32(buffer, Reserved);
    }

    public void ReadCs(IBuffer buffer)
    {
        RoleIndex = ReadInt32(buffer);
        Reserved = ReadInt32(buffer);
    }
}