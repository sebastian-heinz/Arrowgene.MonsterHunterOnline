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

    public class GuildWarBoatStatus : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(GuildWarBoatStatus));

        public GuildWarBoatStatus()
        {
            BoatId = 0;
            PlayerId = 0;
            StartTimestamp = 0;
            LevelId = 0;
            MinTime = 0;
            GuildId = 0;
            GuildName = "";
            RoleName = "";
            Status = 0;
            OhterGuildName = "";
            OtherRoleName1 = "";
            OtherRoleName2 = "";
            OtherRoleName3 = "";
            OtherRoleName4 = "";
        }

        /// <summary>
        /// 商船ID
        /// </summary>
        public uint BoatId;

        /// <summary>
        /// 玩家ID
        /// </summary>
        public ulong PlayerId;

        /// <summary>
        /// 开始时间戳
        /// </summary>
        public uint StartTimestamp;

        /// <summary>
        /// 关卡id
        /// </summary>
        public uint LevelId;

        /// <summary>
        /// 最短时间
        /// </summary>
        public uint MinTime;

        /// <summary>
        /// 猎团ID
        /// </summary>
        public ulong GuildId;

        /// <summary>
        /// 玩家猎团名字
        /// </summary>
        public string GuildName;

        /// <summary>
        /// 玩家名字1
        /// </summary>
        public string RoleName;

        /// <summary>
        /// 状态
        /// </summary>
        public uint Status;

        /// <summary>
        /// 玩家猎团名字
        /// </summary>
        public string OhterGuildName;

        /// <summary>
        /// 其他相关玩家名字
        /// </summary>
        public string OtherRoleName1;

        /// <summary>
        /// 其他相关玩家名字
        /// </summary>
        public string OtherRoleName2;

        /// <summary>
        /// 其他相关玩家名字
        /// </summary>
        public string OtherRoleName3;

        /// <summary>
        /// 其他相关玩家名字
        /// </summary>
        public string OtherRoleName4;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteUInt32(BoatId, Endianness.Big);
            buffer.WriteUInt64(PlayerId, Endianness.Big);
            buffer.WriteUInt32(StartTimestamp, Endianness.Big);
            buffer.WriteUInt32(LevelId, Endianness.Big);
            buffer.WriteUInt32(MinTime, Endianness.Big);
            buffer.WriteUInt64(GuildId, Endianness.Big);
            buffer.WriteInt32(GuildName.Length + 1, Endianness.Big);
            buffer.WriteCString(GuildName);
            buffer.WriteInt32(RoleName.Length + 1, Endianness.Big);
            buffer.WriteCString(RoleName);
            buffer.WriteUInt32(Status, Endianness.Big);
            buffer.WriteInt32(OhterGuildName.Length + 1, Endianness.Big);
            buffer.WriteCString(OhterGuildName);
            buffer.WriteInt32(OtherRoleName1.Length + 1, Endianness.Big);
            buffer.WriteCString(OtherRoleName1);
            buffer.WriteInt32(OtherRoleName2.Length + 1, Endianness.Big);
            buffer.WriteCString(OtherRoleName2);
            buffer.WriteInt32(OtherRoleName3.Length + 1, Endianness.Big);
            buffer.WriteCString(OtherRoleName3);
            buffer.WriteInt32(OtherRoleName4.Length + 1, Endianness.Big);
            buffer.WriteCString(OtherRoleName4);
        }

        public void ReadCs(IBuffer buffer)
        {
            BoatId = buffer.ReadUInt32(Endianness.Big);
            PlayerId = buffer.ReadUInt64(Endianness.Big);
            StartTimestamp = buffer.ReadUInt32(Endianness.Big);
            LevelId = buffer.ReadUInt32(Endianness.Big);
            MinTime = buffer.ReadUInt32(Endianness.Big);
            GuildId = buffer.ReadUInt64(Endianness.Big);
            int GuildNameEntryLen = buffer.ReadInt32(Endianness.Big);
            GuildName = buffer.ReadString(GuildNameEntryLen);
            int RoleNameEntryLen = buffer.ReadInt32(Endianness.Big);
            RoleName = buffer.ReadString(RoleNameEntryLen);
            Status = buffer.ReadUInt32(Endianness.Big);
            int OhterGuildNameEntryLen = buffer.ReadInt32(Endianness.Big);
            OhterGuildName = buffer.ReadString(OhterGuildNameEntryLen);
            int OtherRoleName1EntryLen = buffer.ReadInt32(Endianness.Big);
            OtherRoleName1 = buffer.ReadString(OtherRoleName1EntryLen);
            int OtherRoleName2EntryLen = buffer.ReadInt32(Endianness.Big);
            OtherRoleName2 = buffer.ReadString(OtherRoleName2EntryLen);
            int OtherRoleName3EntryLen = buffer.ReadInt32(Endianness.Big);
            OtherRoleName3 = buffer.ReadString(OtherRoleName3EntryLen);
            int OtherRoleName4EntryLen = buffer.ReadInt32(Endianness.Big);
            OtherRoleName4 = buffer.ReadString(OtherRoleName4EntryLen);
        }

    }
}
