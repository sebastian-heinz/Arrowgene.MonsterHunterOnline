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

    public class CSXRankUserInfo : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSXRankUserInfo));

        public CSXRankUserInfo()
        {
            Uid = 0;
            score = 0;
            timeStamp = 0;
            BestRank = 0;
            RankType = 0;
            RankValue = 0.0f;
        }

        /// <summary>
        /// 玩家uid
        /// </summary>
        public ulong Uid;

        /// <summary>
        /// 玩家分数
        /// </summary>
        public int score;

        /// <summary>
        /// 玩家拿到当前分数的时间
        /// </summary>
        public ulong timeStamp;

        /// <summary>
        /// 玩家历史最佳排名
        /// </summary>
        public uint BestRank;

        /// <summary>
        /// 排名类型、位置或者百分比范围
        /// </summary>
        public byte RankType;

        /// <summary>
        /// 玩家排名位置，或百分比，视rankType而定
        /// </summary>
        public float RankValue;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteUInt64(Uid, Endianness.Big);
            buffer.WriteInt32(score, Endianness.Big);
            buffer.WriteUInt64(timeStamp, Endianness.Big);
            buffer.WriteUInt32(BestRank, Endianness.Big);
            buffer.WriteByte(RankType);
            buffer.WriteFloat(RankValue, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            Uid = buffer.ReadUInt64(Endianness.Big);
            score = buffer.ReadInt32(Endianness.Big);
            timeStamp = buffer.ReadUInt64(Endianness.Big);
            BestRank = buffer.ReadUInt32(Endianness.Big);
            RankType = buffer.ReadByte();
            RankValue = buffer.ReadFloat(Endianness.Big);
        }

    }
}
