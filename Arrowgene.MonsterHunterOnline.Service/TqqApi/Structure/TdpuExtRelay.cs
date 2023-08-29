using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.TqqApi.Constant;

namespace Arrowgene.MonsterHunterOnline.Service.TqqApi.Structure;

public class TdpuExtRelay : CsProto.Core.Structure, ICsStructure
{
    /// <summary>
    /// 重连请求包
    /// </summary>
    public TdpuExtRelay()
    {
        EncMethod = TConnSecEnc.TCONN_SEC_NONE;
        RelayType = RelayType.TPDU_JUMP_SERVER_RELAY;
        OldPos = 0;
        EncryptIdent = new List<byte>();
    }

    /// <summary>
    /// 通信加解密类型
    /// </summary>
    public TConnSecEnc EncMethod { get; set; }

    /// <summary>
    /// 重连类型
    /// </summary>
    public RelayType RelayType { get; set; }

    /// <summary>
    /// 占位连接索引
    /// </summary>
    public int OldPos { get; set; }

    public List<byte> EncryptIdent { get; set; }


    public void WriteCs(IBuffer buffer)
    {
        WriteInt32(buffer, (int)EncMethod);
        WriteInt32(buffer, (int)RelayType);
        WriteInt32(buffer, OldPos);
        WriteList(buffer, EncryptIdent, TqqApiConstant.TPDU_MAX_ENCRYPTIDENT_LEN, WriteInt32, WriteByte);
    }

    public void ReadCs(IBuffer buffer)
    {
        EncMethod = (TConnSecEnc)ReadInt32(buffer);
        RelayType = (RelayType)ReadInt32(buffer);
        OldPos = ReadInt32(buffer);
        ReadList(buffer, EncryptIdent, TqqApiConstant.TPDU_MAX_ENCRYPTIDENT_LEN, ReadInt32, ReadByte);
    }
}