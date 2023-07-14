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
    public int ErrNo { get; set; }

    public override void Write(IBuffer buffer)
    {
        WriteInt32(buffer, ErrNo);
    }

    public override void Read(IBuffer buffer)
    {
        ErrNo = ReadInt32(buffer);
    }
}