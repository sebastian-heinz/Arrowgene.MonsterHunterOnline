﻿using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class MultiNetIpInfo : Structure, ICsStructure
{
    public MultiNetIpInfo()
    {
        SelectIp = "";
        DomainName = "";
        DomainAnalyseIp = "";
        PingDomainIp = 0;
        ConfigIp = InitArray(CsProtoConstant.CS_MAX_IP_STRING_COUNT, () => "");
        PingIp = new int[CsProtoConstant.CS_MAX_IP_STRING_COUNT];
        Port = 0;
        Signature = new List<byte>();
        Isp = 0;
        Mode = 0;
        HistoryPingWeight = new int[CsProtoConstant.CS_MAX_IP_STRING_COUNT];
    }

    /// <summary>
    /// 最终选择的IP
    /// </summary>
    public string SelectIp { get; set; }

    /// <summary>
    /// 域名信息
    /// </summary>
    public string DomainName { get; set; }

    /// <summary>
    /// 当前玩家解析域名获得的IP
    /// </summary>
    public string DomainAnalyseIp { get; set; }

    /// <summary>
    /// 解析域名获得的IP的ping值
    /// </summary>
    public int PingDomainIp { get; set; }

    /// <summary>
    /// 配置的IP
    /// </summary>
    public string[] ConfigIp { get; }

    /// <summary>
    /// IP的ping值
    /// </summary>
    public int[] PingIp { get; }

    /// <summary>
    /// 连接服务器的端口
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// 签名数据
    /// </summary>
    public List<byte> Signature { get; }

    /// <summary>
    /// 运营商选择
    /// </summary>
    public int Isp { get; set; }

    /// <summary>
    /// 连接模式
    /// </summary>
    public int Mode { get; set; }

    /// <summary>
    /// 玩家历史ping值
    /// </summary>
    public int[] HistoryPingWeight { get; }

    public  void WriteCs(IBuffer buffer)
    {
        WriteString(buffer, SelectIp);
        WriteString(buffer, DomainName);
        WriteString(buffer, DomainAnalyseIp);
        WriteInt32(buffer, PingDomainIp);
        WriteArray(buffer, ConfigIp, CsProtoConstant.CS_MAX_IP_STRING_COUNT, WriteString);
        WriteArray(buffer, PingIp, CsProtoConstant.CS_MAX_IP_STRING_COUNT, WriteInt32);
        WriteInt32(buffer, Port);
        WriteList(buffer, Signature, CsProtoConstant.CS_MAX_SIGNATURE_LEN, WriteInt32, WriteByte);
        WriteInt32(buffer, Isp);
        WriteInt32(buffer, Mode);
        WriteArray(buffer, HistoryPingWeight, CsProtoConstant.CS_MAX_IP_STRING_COUNT, WriteInt32);
    }

    public void ReadCs(IBuffer buffer)
    {
        SelectIp = ReadString(buffer);
        DomainName = ReadString(buffer);
        DomainAnalyseIp = ReadString(buffer);
        PingDomainIp = ReadInt32(buffer);
        ReadArray(buffer, ConfigIp, CsProtoConstant.CS_MAX_IP_STRING_COUNT, ReadString);
        ReadArray(buffer, PingIp, CsProtoConstant.CS_MAX_IP_STRING_COUNT, ReadInt32);
        Port = ReadInt32(buffer);
        ReadList<int, byte>(buffer, Signature, CsProtoConstant.CS_MAX_SIGNATURE_LEN, ReadInt32, ReadByte);
        Isp = ReadInt32(buffer);
        Mode = ReadInt32(buffer);
        ReadArray(buffer, HistoryPingWeight, CsProtoConstant.CS_MAX_IP_STRING_COUNT, ReadInt32);
    }
}