/*
* This file is part of Arrowgene.MonsterHunterOnline
*
* Arrowgene.MonsterHunterOnline is a server implementation for the game "Monster Hunter Online".
* Copyright (C) 2023-2024 "all contributors"
*
* Github: https://github.com/sebastian-heinz/Arrowgene.MonsterHunterOnline
*
*  This program is free software: you can redistribute it and/or modify
*  it under the terms of the GNU Affero General Public License as published
*  by the Free Software Foundation, either version 3 of the License, or
*  (at your option) any later version.
*
*  This program is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU Affero General Public License for more details.
*
*  You should have received a copy of the GNU Affero General Public License
*  along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

/* THIS IS AN AUTOGENERATED FILE. DO NOT EDIT THIS FILE DIRECTLY. */

using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{

    /// <summary>
    /// 双线机房时对选择IP的判定数据信息进行收集上报
    /// </summary>
    public class CSMultiNetIpinfo : IStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSMultiNetIpinfo));

        public CSMultiNetIpinfo()
        {
            SelectIP = "";
            DomainName = "";
            DomainAnalyseIP = "";
            PingDomainIP = 0;
            ConfigIP = new string[CsProtoConstant.CS_MAX_IP_STRING_COUNT];
            for (int i = 0; i < CsProtoConstant.CS_MAX_IP_STRING_COUNT; i++)
            {
                ConfigIP[i] = "";
            }
            PingIP = new int[CsProtoConstant.CS_MAX_IP_STRING_COUNT];
            Port = 0;
            Signature = new List<byte>();
            Isp = 0;
            Mode = 0;
            HistoryPingWeight = new int[CsProtoConstant.CS_MAX_IP_STRING_COUNT];
        }

        /// <summary>
        /// 最终选择的IP
        /// </summary>
        public string SelectIP;

        /// <summary>
        /// 域名信息
        /// </summary>
        public string DomainName;

        /// <summary>
        /// 当前玩家解析域名获得的IP
        /// </summary>
        public string DomainAnalyseIP;

        /// <summary>
        /// 解析域名获得的IP的ping值
        /// </summary>
        public int PingDomainIP;

        /// <summary>
        /// 配置的IP
        /// </summary>
        public string[] ConfigIP;

        /// <summary>
        /// IP的ping值
        /// </summary>
        public int[] PingIP;

        /// <summary>
        /// 连接服务器的端口
        /// </summary>
        public int Port;

        /// <summary>
        /// 签名数据
        /// </summary>
        public List<byte> Signature;

        /// <summary>
        /// 运营商选择
        /// </summary>
        public int Isp;

        /// <summary>
        /// 连接模式
        /// </summary>
        public int Mode;

        /// <summary>
        /// 玩家历史ping值
        /// </summary>
        public int[] HistoryPingWeight;

        public void Write(IBuffer buffer)
        {
            buffer.WriteInt32(SelectIP.Length + 1, Endianness.Big);
            buffer.WriteCString(SelectIP);
            buffer.WriteInt32(DomainName.Length + 1, Endianness.Big);
            buffer.WriteCString(DomainName);
            buffer.WriteInt32(DomainAnalyseIP.Length + 1, Endianness.Big);
            buffer.WriteCString(DomainAnalyseIP);
            buffer.WriteInt32(PingDomainIP, Endianness.Big);
            int ConfigIPLen = ConfigIP.Length;
            buffer.WriteInt32(ConfigIPLen, Endianness.Big);
            for (int i = 0; i < ConfigIPLen; i++)
            {
                buffer.WriteInt32(ConfigIP[i].Length + 1, Endianness.Big);
                buffer.WriteCString(ConfigIP[i]);
            }
            for (int i = 0; i < CsProtoConstant.CS_MAX_IP_STRING_COUNT; i++)
            {
                buffer.WriteInt32(PingIP[i], Endianness.Big);
            }
            buffer.WriteInt32(Port, Endianness.Big);
            int signatureCount = (int)Signature.Count;
            buffer.WriteInt32(signatureCount, Endianness.Big);
            for (int i = 0; i < signatureCount; i++)
            {
                buffer.WriteByte(Signature[i]);
            }
            buffer.WriteInt32(Isp, Endianness.Big);
            buffer.WriteInt32(Mode, Endianness.Big);
            for (int i = 0; i < CsProtoConstant.CS_MAX_IP_STRING_COUNT; i++)
            {
                buffer.WriteInt32(HistoryPingWeight[i], Endianness.Big);
            }
        }

        public void Read(IBuffer buffer)
        {
            int SelectIPEntryLen = buffer.ReadInt32(Endianness.Big);
            SelectIP = buffer.ReadString(SelectIPEntryLen);
            int DomainNameEntryLen = buffer.ReadInt32(Endianness.Big);
            DomainName = buffer.ReadString(DomainNameEntryLen);
            int DomainAnalyseIPEntryLen = buffer.ReadInt32(Endianness.Big);
            DomainAnalyseIP = buffer.ReadString(DomainAnalyseIPEntryLen);
            PingDomainIP = buffer.ReadInt32(Endianness.Big);
            int ConfigIPLen = buffer.ReadInt32();
            for (int i = 0; i < ConfigIPLen; i++)
            {
                int ConfigIPEntryLen = buffer.ReadInt32(Endianness.Big);
                ConfigIP[i] = buffer.ReadString(ConfigIPEntryLen);
            }
            for (int i = 0; i < CsProtoConstant.CS_MAX_IP_STRING_COUNT; i++)
            {
                PingIP[i] = buffer.ReadInt32(Endianness.Big);
            }
            Port = buffer.ReadInt32(Endianness.Big);
            Signature.Clear();
            int signatureCount = buffer.ReadInt32(Endianness.Big);
            for (int i = 0; i < signatureCount; i++)
            {
                byte SignatureEntry = buffer.ReadByte();
                Signature.Add(SignatureEntry);
            }
            Isp = buffer.ReadInt32(Endianness.Big);
            Mode = buffer.ReadInt32(Endianness.Big);
            for (int i = 0; i < CsProtoConstant.CS_MAX_IP_STRING_COUNT; i++)
            {
                HistoryPingWeight[i] = buffer.ReadInt32(Endianness.Big);
            }
        }

    }
}
