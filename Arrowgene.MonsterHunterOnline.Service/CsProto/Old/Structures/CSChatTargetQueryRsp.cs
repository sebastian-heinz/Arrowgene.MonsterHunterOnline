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
    /// 客户端聊天查询对方玩家信息响应
    /// </summary>
    public class CSChatTargetQueryRsp : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSChatTargetQueryRsp));

        public CSChatTargetQueryRsp()
        {
            Name = "";
            Level = 0;
            GuildName = "";
            HunterStar = "";
            HRLevel = 0;
            LineID = 0;
            VipLevel = 0;
            VipCanUse = 0;
        }

        /// <summary>
        /// 玩家角色名字
        /// </summary>
        public string Name;

        /// <summary>
        /// 玩家普通等级
        /// </summary>
        public int Level;

        /// <summary>
        /// 玩家猎团名字
        /// </summary>
        public string GuildName;

        /// <summary>
        /// 玩家猎人星级
        /// </summary>
        public string HunterStar;

        /// <summary>
        /// 玩家HR等级
        /// </summary>
        public int HRLevel;

        /// <summary>
        /// 目标玩家的线ID
        /// </summary>
        public int LineID;

        /// <summary>
        /// VIP等级
        /// </summary>
        public byte VipLevel;

        /// <summary>
        /// VIP是否可用
        /// </summary>
        public byte VipCanUse;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt32(Name.Length + 1, Endianness.Big);
            buffer.WriteCString(Name);
            buffer.WriteInt32(Level, Endianness.Big);
            buffer.WriteInt32(GuildName.Length + 1, Endianness.Big);
            buffer.WriteCString(GuildName);
            buffer.WriteInt32(HunterStar.Length + 1, Endianness.Big);
            buffer.WriteCString(HunterStar);
            buffer.WriteInt32(HRLevel, Endianness.Big);
            buffer.WriteInt32(LineID, Endianness.Big);
            buffer.WriteByte(VipLevel);
            buffer.WriteByte(VipCanUse);
        }

        public void ReadCs(IBuffer buffer)
        {
            int NameEntryLen = buffer.ReadInt32(Endianness.Big);
            Name = buffer.ReadString(NameEntryLen);
            Level = buffer.ReadInt32(Endianness.Big);
            int GuildNameEntryLen = buffer.ReadInt32(Endianness.Big);
            GuildName = buffer.ReadString(GuildNameEntryLen);
            int HunterStarEntryLen = buffer.ReadInt32(Endianness.Big);
            HunterStar = buffer.ReadString(HunterStarEntryLen);
            HRLevel = buffer.ReadInt32(Endianness.Big);
            LineID = buffer.ReadInt32(Endianness.Big);
            VipLevel = buffer.ReadByte();
            VipCanUse = buffer.ReadByte();
        }

    }
}
