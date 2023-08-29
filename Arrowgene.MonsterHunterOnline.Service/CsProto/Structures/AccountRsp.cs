using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class AccountRsp : Structure, ICsStructure
{
    public AccountRsp()
    {
        FaceCount = 0;
        ChgSexCount = 0;
        Result = 0;
    }

    public uint FaceCount { get; set; }
    public uint ChgSexCount { get; set; }
    public uint Result { get; set; }

    public void WriteCs(IBuffer buffer)
    {
        WriteUInt32(buffer, FaceCount);
        WriteUInt32(buffer, ChgSexCount);
        WriteUInt32(buffer, Result);
    }

    public void ReadCs(IBuffer buffer)
    {
        FaceCount = ReadUInt32(buffer);
        ChgSexCount = ReadUInt32(buffer);
        Result = ReadUInt32(buffer);
    }
}