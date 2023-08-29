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
    /// 猎团战信息
    /// </summary>
    public class CSGuilderWarInfo : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSGuilderWarInfo));

        public CSGuilderWarInfo()
        {
            ChallengeTimes = 0;
            BuyGrabTimes = 0;
            RefreshTimestamp = 0;
            StartBoatTimes = 0;
            BuyStartBoatTimes = 0;
            OhterGuildNewsInfo = new List<CSOhterGuildNews>();
            CommerceBoatInfo = new CSCommerceBoatInfo();
            ContributeResPoint = 0;
        }

        /// <summary>
        /// 猎团战挑战次数
        /// </summary>
        public byte ChallengeTimes;

        /// <summary>
        /// 猎团战购买抢夺次数
        /// </summary>
        public byte BuyGrabTimes;

        /// <summary>
        /// 刷新时间
        /// </summary>
        public int RefreshTimestamp;

        /// <summary>
        /// 开船次数
        /// </summary>
        public int StartBoatTimes;

        /// <summary>
        /// 猎团战购买开船次数
        /// </summary>
        public byte BuyStartBoatTimes;

        /// <summary>
        /// 打探的猎团信息
        /// </summary>
        public List<CSOhterGuildNews> OhterGuildNewsInfo;

        /// <summary>
        /// 当前商船信息
        /// </summary>
        public CSCommerceBoatInfo CommerceBoatInfo;

        /// <summary>
        /// 贡献资源点
        /// </summary>
        public uint ContributeResPoint;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteByte(ChallengeTimes);
            buffer.WriteByte(BuyGrabTimes);
            buffer.WriteInt32(RefreshTimestamp, Endianness.Big);
            buffer.WriteInt32(StartBoatTimes, Endianness.Big);
            buffer.WriteByte(BuyStartBoatTimes);
            int ohterGuildNewsInfoCount = (int)OhterGuildNewsInfo.Count;
            buffer.WriteInt32(ohterGuildNewsInfoCount, Endianness.Big);
            for (int i = 0; i < ohterGuildNewsInfoCount; i++)
            {
                OhterGuildNewsInfo[i].WriteCs(buffer);
            }
            CommerceBoatInfo.WriteCs(buffer);
            buffer.WriteUInt32(ContributeResPoint, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            ChallengeTimes = buffer.ReadByte();
            BuyGrabTimes = buffer.ReadByte();
            RefreshTimestamp = buffer.ReadInt32(Endianness.Big);
            StartBoatTimes = buffer.ReadInt32(Endianness.Big);
            BuyStartBoatTimes = buffer.ReadByte();
            OhterGuildNewsInfo.Clear();
            int ohterGuildNewsInfoCount = buffer.ReadInt32(Endianness.Big);
            for (int i = 0; i < ohterGuildNewsInfoCount; i++)
            {
                CSOhterGuildNews OhterGuildNewsInfoEntry = new CSOhterGuildNews();
                OhterGuildNewsInfoEntry.ReadCs(buffer);
                OhterGuildNewsInfo.Add(OhterGuildNewsInfoEntry);
            }
            CommerceBoatInfo.ReadCs(buffer);
            ContributeResPoint = buffer.ReadUInt32(Endianness.Big);
        }

    }
}
