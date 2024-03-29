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
    /// 赛制周期信息
    /// </summary>
    public class CSLevelTournamentPeriodLimitNotify : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSLevelTournamentPeriodLimitNotify));

        public CSLevelTournamentPeriodLimitNotify()
        {
            Season = 0;
            Round = 0;
            SubGroupID = 0;
            finish_count = 0;
        }

        /// <summary>
        /// 赛季
        /// </summary>
        public int Season;

        /// <summary>
        /// 轮次
        /// </summary>
        public int Round;

        /// <summary>
        /// 小组ID
        /// </summary>
        public int SubGroupID;

        /// <summary>
        /// 完成次数
        /// </summary>
        public int finish_count;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt32(Season, Endianness.Big);
            buffer.WriteInt32(Round, Endianness.Big);
            buffer.WriteInt32(SubGroupID, Endianness.Big);
            buffer.WriteInt32(finish_count, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            Season = buffer.ReadInt32(Endianness.Big);
            Round = buffer.ReadInt32(Endianness.Big);
            SubGroupID = buffer.ReadInt32(Endianness.Big);
            finish_count = buffer.ReadInt32(Endianness.Big);
        }

    }
}
