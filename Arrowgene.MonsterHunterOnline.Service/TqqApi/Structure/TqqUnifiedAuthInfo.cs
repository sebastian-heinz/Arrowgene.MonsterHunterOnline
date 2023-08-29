using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.TqqApi.Constant;

namespace Arrowgene.MonsterHunterOnline.Service.TqqApi.Structure;

public class TqqUnifiedAuthInfo : CsProto.Core.Structure, ICsStructure, CSICsTpduExtAuthData
{
    public TqqUnifiedAuthInfo()
    {
        Uin = 0;
        SigInfo = new List<byte>();
    }

    public uint Uin { get; set; }
    public List<byte> SigInfo { get; set; }


    public  void WriteCs(IBuffer buffer)
    {
        WriteUInt32(buffer, Uin);
        WriteList(buffer, SigInfo, (byte)TqqApiConstant.TQQ_MAX_SIGN_LEN, WriteByte, WriteByte);
    }

    public void ReadCs(IBuffer buffer)
    {
        Uin = ReadUInt32(buffer);
        ReadList(buffer, SigInfo, (byte)TqqApiConstant.TQQ_MAX_SIGN_LEN, ReadByte, ReadByte);
    }
}