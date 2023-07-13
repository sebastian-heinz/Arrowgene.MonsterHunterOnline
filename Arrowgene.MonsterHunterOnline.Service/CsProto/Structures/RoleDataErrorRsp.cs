using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class RoleDataErrorRsp : Structure
{
    public RoleDataErrorRsp()
    {
        ErrNo = 0;
    }

    /// <summary>
    /// 0为成功
    /// </summary>
    public int ErrNo;

    public override void Write(IBuffer buffer)
    {
        buffer.WriteInt32(ErrNo, Endianness.Big);
    }

    public override void Read(IBuffer buffer)
    {
        ErrNo = buffer.ReadInt32(Endianness.Big);
    }
}