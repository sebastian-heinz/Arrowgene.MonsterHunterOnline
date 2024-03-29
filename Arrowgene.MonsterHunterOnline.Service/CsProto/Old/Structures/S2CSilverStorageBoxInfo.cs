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
    /// 银币收纳箱
    /// </summary>
    public class S2CSilverStorageBoxInfo : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(S2CSilverStorageBoxInfo));

        public S2CSilverStorageBoxInfo()
        {
            SilverCount = 0;
            WeekFreeFetchTimes = 0;
            WeekBuyFetchTimes = 0;
            EnlargeTimes = 0;
        }

        /// <summary>
        /// 银币收纳箱中的当前银币量
        /// </summary>
        public int SilverCount;

        /// <summary>
        /// 银币收纳箱本周已免费提取银币次数
        /// </summary>
        public int WeekFreeFetchTimes;

        /// <summary>
        /// 银币收纳箱本周已付费提取银币次数
        /// </summary>
        public int WeekBuyFetchTimes;

        /// <summary>
        /// 累计扩容次数
        /// </summary>
        public int EnlargeTimes;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt32(SilverCount, Endianness.Big);
            buffer.WriteInt32(WeekFreeFetchTimes, Endianness.Big);
            buffer.WriteInt32(WeekBuyFetchTimes, Endianness.Big);
            buffer.WriteInt32(EnlargeTimes, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            SilverCount = buffer.ReadInt32(Endianness.Big);
            WeekFreeFetchTimes = buffer.ReadInt32(Endianness.Big);
            WeekBuyFetchTimes = buffer.ReadInt32(Endianness.Big);
            EnlargeTimes = buffer.ReadInt32(Endianness.Big);
        }

    }
}
