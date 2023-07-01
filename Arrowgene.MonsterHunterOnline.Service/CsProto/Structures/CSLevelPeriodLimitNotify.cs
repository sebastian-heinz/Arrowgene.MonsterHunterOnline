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
    /// 副本周期信息
    /// </summary>
    public class CSLevelPeriodLimitNotify : IStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSLevelPeriodLimitNotify));

        public CSLevelPeriodLimitNotify()
        {
            GroupID = 0;
            SubGroupID = 0;
            LevelID = 0;
            last_finish_tm = 0;
            finish_count = 0;
            opt = 0;
        }

        /// <summary>
        /// 大组ID
        /// </summary>
        public int GroupID;

        /// <summary>
        /// 小组ID
        /// </summary>
        public int SubGroupID;

        /// <summary>
        /// 关卡ID
        /// </summary>
        public int LevelID;

        /// <summary>
        /// 最后一次更新时间
        /// </summary>
        public int last_finish_tm;

        /// <summary>
        /// 完成次数
        /// </summary>
        public int finish_count;

        /// <summary>
        /// 操作类型1:跟新 2:删除
        /// </summary>
        public int opt;

        public void Write(IBuffer buffer)
        {
            buffer.WriteInt32(GroupID, Endianness.Big);
            buffer.WriteInt32(SubGroupID, Endianness.Big);
            buffer.WriteInt32(LevelID, Endianness.Big);
            buffer.WriteInt32(last_finish_tm, Endianness.Big);
            buffer.WriteInt32(finish_count, Endianness.Big);
            buffer.WriteInt32(opt, Endianness.Big);
        }

        public void Read(IBuffer buffer)
        {
            GroupID = buffer.ReadInt32(Endianness.Big);
            SubGroupID = buffer.ReadInt32(Endianness.Big);
            LevelID = buffer.ReadInt32(Endianness.Big);
            last_finish_tm = buffer.ReadInt32(Endianness.Big);
            finish_count = buffer.ReadInt32(Endianness.Big);
            opt = buffer.ReadInt32(Endianness.Big);
        }

    }
}
