using System;

namespace Arrowgene.MonsterHunterOnline.Service.TQQApi.Model;

/// <summary>
/// 0X82签名协议
/// </summary>
public class TQQAuthInfo
{
    public TQQAuthInfo()
    {
        SignData = Array.Empty<byte>();
        Sign2Data = Array.Empty<byte>();
    }
    
    public uint Uin { get; set; }
    public byte SignLen { get; set; }
    public byte[] SignData { get; set; }
    public byte Sign2Len { get; set; }
    public byte[] Sign2Data { get; set; } // 一般不用

    // <struct name="TQQAuthInfo" version="10" desc="">
    // <entry name="Uin" type="uint"/>
    // <entry name="SignLen" type="tinyuint"/>
    // <entry name="SignData" type="tinyuint" count="TQQ_MAX_SIGN_LEN" refer="SignLen"/>
    // <entry name="Sign2Len" type="tinyuint"/>
    // <entry name="Sign2Data" type="tinyuint" count="TQQ_MAX_SIGN2_LEN" desc="" refer="Sign2Len"/>
    // </struct>
}