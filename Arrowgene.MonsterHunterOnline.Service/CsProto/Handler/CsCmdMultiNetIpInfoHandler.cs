using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdMultiNetIpInfoHandler : ICsProtoHandler
{
    public CsProtoCmd Cmd => CsProtoCmd.CS_CMD_MULTI_NET_IPINFO;

    public void Handle(Client client, CsProtoPacket packet)
    {
        IBuffer req = new StreamBuffer(packet.Body);
        req.SetPositionStart();
        //  <struct name="CSMultiNetIpinfo" version="1" desc="双线机房时对选择IP的判定数据信息进行收集上报">
        //      <entry name="SelectIP" type="string" size="CS_MAX_IP_STRING_LENGTH" desc="最终选择的IP" sizeinfo="int"/>
        //      <entry name="DomainName" type="string" size="CS_MAX_IP_STRING_LENGTH" desc="域名信息" sizeinfo="int"/>
        //      <entry name="DomainAnalyseIP" type="string" size="CS_MAX_IP_STRING_LENGTH" desc="当前玩家解析域名获得的IP" sizeinfo="int"/>
        //      <entry name="PingDomainIP" type="int" desc="解析域名获得的IP的ping值"/>
        //      <entry name="ConfigIP" type="string" count="CS_MAX_IP_STRING_COUNT" size="CS_MAX_IP_STRING_LENGTH" desc="配置的IP" sizeinfo="int"/>
        //      <entry name="PingIP" type="int" count="CS_MAX_IP_STRING_COUNT" desc="IP的ping值"/>
        //      <entry name="Port" type="int" desc="连接服务器的端口"/>
        //      <entry name="SignatureLen" type="int" desc="签名长度"/>
        //      <entry name="Signature" type="byte" count="CS_MAX_SIGNATURE_LEN" desc="签名数据" refer="SignatureLen"/>
        //      <entry name="Isp" type="int" desc="运营商选择"/>
        //      <entry name="Mode" type="int" desc="连接模式"/>
        //      <entry name="HistoryPingWeight" type="int" count="CS_MAX_IP_STRING_COUNT" desc="玩家历史ping值"/>
        //      </struct>

        int SelectIPLen = req.ReadInt32(Endianness.Big);
        string SelectIP = req.ReadString(SelectIPLen);
        int DomainNameLen = req.ReadInt32(Endianness.Big);
        string DomainName = req.ReadString(DomainNameLen);
        int PingDomainIP = req.ReadInt32(Endianness.Big);
        int ConfigIPLen = req.ReadInt32(Endianness.Big);
        string ConfigIP = req.ReadString(DomainNameLen);
        int PingIP = req.ReadInt32(Endianness.Big);
        int Port = req.ReadInt32(Endianness.Big);
        int SignatureLen = req.ReadInt32(Endianness.Big);
        int Signature = req.ReadByte();
        int Isp = req.ReadInt32(Endianness.Big);
        int Mode = req.ReadInt32(Endianness.Big);
        int HistoryPingWeight = req.ReadInt32(Endianness.Big);


        //  <struct name="CSMultiIspSequenceNtf" version="1" desc="双线机房运营商顺序">
        //      <entry name="Sequence" type="int" desc="顺序"/>
        //      </struct>

        IBuffer res = new StreamBuffer();
        res.WriteInt32(0, Endianness.Big);

        CsProtoPacket resp = new CsProtoPacket();
        resp.Body = res.GetAllBytes();
        resp.Cmd = CsProtoCmd.CS_CMD_MULTI_ISP_SEQUENCE_NTF;
        client.SendCsProto(resp);
    }
}