using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class RoleDataErrorRsp : Structure, ICsStructure
{
    public RoleDataErrorRsp()
    {
        ErrNo = 0;
    }

    /// <summary>
    /// 0为成功
    /// </summary>
    public int ErrNo { get; set; }

    public  void WriteCs(IBuffer buffer)
    {
        WriteInt32(buffer, ErrNo);
    }

    public void ReadCs(IBuffer buffer)
    {
        ErrNo = ReadInt32(buffer);
    }
}