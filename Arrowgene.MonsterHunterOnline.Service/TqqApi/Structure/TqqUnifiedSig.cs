using System;

namespace Arrowgene.MonsterHunterOnline.Service.TqqApi.Structure;

/// <summary>
/// 0X82签名协议
/// </summary>
public class TqqUnifiedSig
{
    public TqqUnifiedSig()
    {
        EncryptSignData = Array.Empty<byte>();
    }

    public short Version { get; set; }
    public uint Time { get; set; }
    public short EncryptSignLen { get; set; }
    public byte[] EncryptSignData { get; set; }


    //<struct name="TQQUnifiedSig" version="10" desc="0XDE签名协议格式">
    //<entry name="Version" type="short"/>
    //<entry name="Time" type="uint"/>
    //<entry name="EncryptSignLen" type="short"/>
    //<entry name="EncryptSignData" type="tinyuint" count="TQQ_UNIFIED_MAX_ENCSIGN_LEN" refer="EncryptSignLen"/>
    //</struct>
}