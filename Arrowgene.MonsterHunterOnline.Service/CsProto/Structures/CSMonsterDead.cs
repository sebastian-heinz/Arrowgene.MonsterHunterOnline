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
    /// 怪物死亡
    /// </summary>
    public class CSMonsterDead : IStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSMonsterDead));

        public CSMonsterDead()
        {
            SyncTime = 0;
            MonsterID = 0;
            Location = new CSVec3();
            Rotation = new CSQuat();
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
        /// 怪物位置
        /// </summary>
        public CSVec3 Location;

        /// <summary>
        /// 怪物朝向
        /// </summary>
        public CSQuat Rotation;

        public void Write(IBuffer buffer)
        {
            buffer.WriteInt64(SyncTime, Endianness.Big);
            buffer.WriteUInt32(MonsterID, Endianness.Big);
            Location.Write(buffer);
            Rotation.Write(buffer);
        }

        public void Read(IBuffer buffer)
        {
            SyncTime = buffer.ReadInt64(Endianness.Big);
            MonsterID = buffer.ReadUInt32(Endianness.Big);
            Location.Read(buffer);
            Rotation.Read(buffer);
        }

    }
}
