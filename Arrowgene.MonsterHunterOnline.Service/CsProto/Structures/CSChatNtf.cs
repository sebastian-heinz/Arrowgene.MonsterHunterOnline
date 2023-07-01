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
    /// 发送给客户端的其他玩家聊天信息通知
    /// </summary>
    public class CSChatNtf : IStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSChatNtf));

        public CSChatNtf()
        {
            SourceID = 0;
            SrcUin = 0;
            SrcDBID = 0;
            SrcLevelGrpId = 0;
            SourceName = "";
            SrcVipLevel = 0;
            SrcVipCanUse = 0;
            QQMask = 0;
            ChannelType = 0;
            LineID = 0;
            WorldSvrID = 0;
            ShowTime = 0;
            Head = new List<byte>();
            Content = "";
            SendByMe = 0;
            ContainBanWords = 0;
            Items = new CsChatItemsPkg();
            SrcLevel = 0;
            SrcGuildName = "";
            SrcHunterStar = "";
            SrcHRLevel = 0;
        }

        /// <summary>
        /// 源玩家ID
        /// </summary>
        public int SourceID;

        /// <summary>
        /// 源玩家QQ号
        /// </summary>
        public uint SrcUin;

        /// <summary>
        /// 源玩家DBID
        /// </summary>
        public ulong SrcDBID;

        /// <summary>
        /// 源角色所在关卡的逻辑关卡组ID
        /// </summary>
        public int SrcLevelGrpId;

        /// <summary>
        /// 源玩家角色名字
        /// </summary>
        public string SourceName;

        /// <summary>
        /// 源VIP等级
        /// </summary>
        public byte SrcVipLevel;

        /// <summary>
        /// 源VIP是否可用
        /// </summary>
        public byte SrcVipCanUse;

        /// <summary>
        /// 其他合作福利标记
        /// </summary>
        public int QQMask;

        /// <summary>
        /// 频道类型ID
        /// </summary>
        public int ChannelType;

        /// <summary>
        /// 消息发起者的线ID
        /// </summary>
        public int LineID;

        /// <summary>
        /// 消息发送者的WorldSvrID
        /// </summary>
        public int WorldSvrID;

        /// <summary>
        /// 消息展示持续时间秒
        /// </summary>
        public int ShowTime;

        /// <summary>
        /// 头部信息
        /// </summary>
        public List<byte> Head;

        /// <summary>
        /// 聊天内容
        /// </summary>
        public string Content;

        /// <summary>
        /// 是否为自己对单个别人说（1=true或0=false），只有密语频道有用，用于区分是别人对自己说还是自己对别人说。如果是自己对别人说，则SourceID和SourceName分别为目标玩家ID和目标玩家角色名（而非源玩家ID和源玩家角色名）。
        /// </summary>
        public byte SendByMe;

        /// <summary>
        /// 是否包括密语敏感词(1表示包含，0表示不包含)
        /// </summary>
        public byte ContainBanWords;

        /// <summary>
        /// 聊天道具数据
        /// </summary>
        public CsChatItemsPkg Items;

        /// <summary>
        /// 源玩家普通等级, 如果满级的话就是HR等级
        /// </summary>
        public int SrcLevel;

        /// <summary>
        /// 源玩家猎团名字
        /// </summary>
        public string SrcGuildName;

        /// <summary>
        /// 玩家猎人星级
        /// </summary>
        public string SrcHunterStar;

        /// <summary>
        /// 源玩家HR等级
        /// </summary>
        public int SrcHRLevel;

        public void Write(IBuffer buffer)
        {
            buffer.WriteInt32(SourceID, Endianness.Big);
            buffer.WriteUInt32(SrcUin, Endianness.Big);
            buffer.WriteUInt64(SrcDBID, Endianness.Big);
            buffer.WriteInt32(SrcLevelGrpId, Endianness.Big);
            buffer.WriteInt32(SourceName.Length + 1, Endianness.Big);
            buffer.WriteCString(SourceName);
            buffer.WriteByte(SrcVipLevel);
            buffer.WriteByte(SrcVipCanUse);
            buffer.WriteInt32(QQMask, Endianness.Big);
            buffer.WriteInt32(ChannelType, Endianness.Big);
            buffer.WriteInt32(LineID, Endianness.Big);
            buffer.WriteInt32(WorldSvrID, Endianness.Big);
            buffer.WriteInt32(ShowTime, Endianness.Big);
            int headCount = (int)Head.Count;
            buffer.WriteInt32(headCount, Endianness.Big);
            for (int i = 0; i < headCount; i++)
            {
                buffer.WriteByte(Head[i]);
            }
            buffer.WriteInt32(Content.Length + 1, Endianness.Big);
            buffer.WriteCString(Content);
            buffer.WriteByte(SendByMe);
            buffer.WriteByte(ContainBanWords);
            Items.Write(buffer);
            buffer.WriteInt32(SrcLevel, Endianness.Big);
            buffer.WriteInt32(SrcGuildName.Length + 1, Endianness.Big);
            buffer.WriteCString(SrcGuildName);
            buffer.WriteInt32(SrcHunterStar.Length + 1, Endianness.Big);
            buffer.WriteCString(SrcHunterStar);
            buffer.WriteInt32(SrcHRLevel, Endianness.Big);
        }

        public void Read(IBuffer buffer)
        {
            SourceID = buffer.ReadInt32(Endianness.Big);
            SrcUin = buffer.ReadUInt32(Endianness.Big);
            SrcDBID = buffer.ReadUInt64(Endianness.Big);
            SrcLevelGrpId = buffer.ReadInt32(Endianness.Big);
            int SourceNameEntryLen = buffer.ReadInt32(Endianness.Big);
            SourceName = buffer.ReadString(SourceNameEntryLen);
            SrcVipLevel = buffer.ReadByte();
            SrcVipCanUse = buffer.ReadByte();
            QQMask = buffer.ReadInt32(Endianness.Big);
            ChannelType = buffer.ReadInt32(Endianness.Big);
            LineID = buffer.ReadInt32(Endianness.Big);
            WorldSvrID = buffer.ReadInt32(Endianness.Big);
            ShowTime = buffer.ReadInt32(Endianness.Big);
            Head.Clear();
            int headCount = buffer.ReadInt32(Endianness.Big);
            for (int i = 0; i < headCount; i++)
            {
                byte HeadEntry = buffer.ReadByte();
                Head.Add(HeadEntry);
            }
            int ContentEntryLen = buffer.ReadInt32(Endianness.Big);
            Content = buffer.ReadString(ContentEntryLen);
            SendByMe = buffer.ReadByte();
            ContainBanWords = buffer.ReadByte();
            Items.Read(buffer);
            SrcLevel = buffer.ReadInt32(Endianness.Big);
            int SrcGuildNameEntryLen = buffer.ReadInt32(Endianness.Big);
            SrcGuildName = buffer.ReadString(SrcGuildNameEntryLen);
            int SrcHunterStarEntryLen = buffer.ReadInt32(Endianness.Big);
            SrcHunterStar = buffer.ReadString(SrcHunterStarEntryLen);
            SrcHRLevel = buffer.ReadInt32(Endianness.Big);
        }

    }
}
