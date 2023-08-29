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
    /// 定时位置同步消息
    /// </summary>
    public class CSMonsterMovestate : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSMonsterMovestate));

        public CSMonsterMovestate()
        {
            SyncTime = 0;
            MonsterID = 0;
            Location = new CSVec3();
            Rotation = new CSQuat();
            Speed = new CSVec3();
        }

        /// <summary>
        /// 同步时间
        /// </summary>
        public long SyncTime;

        /// <summary>
        /// 怪物ID
        /// </summary>
        public uint MonsterID;

        /// <summary>
        /// 世界坐标系下的位置
        /// </summary>
        public CSVec3 Location;

        /// <summary>
        /// 朝向
        /// </summary>
        public CSQuat Rotation;

        /// <summary>
        /// 移动速度
        /// </summary>
        public CSVec3 Speed;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt64(SyncTime, Endianness.Big);
            buffer.WriteUInt32(MonsterID, Endianness.Big);
            Location.WriteCs(buffer);
            Rotation.WriteCs(buffer);
            Speed.WriteCs(buffer);
        }

        public void ReadCs(IBuffer buffer)
        {
            SyncTime = buffer.ReadInt64(Endianness.Big);
            MonsterID = buffer.ReadUInt32(Endianness.Big);
            Location.ReadCs(buffer);
            Rotation.ReadCs(buffer);
            Speed.ReadCs(buffer);
        }

    }
}
