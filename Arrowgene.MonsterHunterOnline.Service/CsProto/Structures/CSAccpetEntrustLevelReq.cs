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
    /// 接取委托关卡
    /// </summary>
    public class CSAccpetEntrustLevelReq : IStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSAccpetEntrustLevelReq));

        public CSAccpetEntrustLevelReq()
        {
            GroupID = 0;
            SubGroupID = 0;
            LevelID = 0;
            NpcID = 0;
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
        /// NPCID
        /// </summary>
        public int NpcID;

        public void Write(IBuffer buffer)
        {
            buffer.WriteInt32(GroupID, Endianness.Big);
            buffer.WriteInt32(SubGroupID, Endianness.Big);
            buffer.WriteInt32(LevelID, Endianness.Big);
            buffer.WriteInt32(NpcID, Endianness.Big);
        }

        public void Read(IBuffer buffer)
        {
            GroupID = buffer.ReadInt32(Endianness.Big);
            SubGroupID = buffer.ReadInt32(Endianness.Big);
            LevelID = buffer.ReadInt32(Endianness.Big);
            NpcID = buffer.ReadInt32(Endianness.Big);
        }

    }
}
