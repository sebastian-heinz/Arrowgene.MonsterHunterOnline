using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.TqqApi.Constant;

namespace Arrowgene.MonsterHunterOnline.Service.TqqApi.Structure;

public class TqqAuthInfo : CsProto.Core.Structure, ITpduExtAuthData
{
    public TqqAuthInfo()
    {
        Uin = 0;
        SignData = new List<byte>();
        Sign2Data = new List<byte>();
    }

    public uint Uin { get; set; }
    public List<byte> SignData { get; set; }
    public List<byte> Sign2Data { get; set; }


    public override void Write(IBuffer buffer)
    {
        WriteUInt32(buffer, Uin);
        WriteList(buffer, SignData, (byte)TqqApiConstant.TQQ_MAX_SIGN_LEN, WriteByte, WriteByte);
        WriteList(buffer, Sign2Data, (byte)TqqApiConstant.TQQ_MAX_SIGN2_LEN, WriteByte, WriteByte);
    }

    public override void Read(IBuffer buffer)
    {
        Uin = ReadUInt32(buffer);
        ReadList(buffer, SignData, (byte)TqqApiConstant.TQQ_MAX_SIGN_LEN, ReadByte, ReadByte);
        ReadList(buffer, Sign2Data, (byte)TqqApiConstant.TQQ_MAX_SIGN2_LEN, ReadByte, ReadByte);
    }
}