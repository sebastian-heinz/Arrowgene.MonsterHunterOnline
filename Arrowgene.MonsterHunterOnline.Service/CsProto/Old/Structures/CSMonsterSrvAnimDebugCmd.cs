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
    /// 怪物服务器动画调试
    /// </summary>
    public class CSMonsterSrvAnimDebugCmd : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSMonsterSrvAnimDebugCmd));

        public CSMonsterSrvAnimDebugCmd()
        {
            MonsterID = 0;
            DebugMode = 0;
            EnableDebug = 0;
        }

        /// <summary>
        /// 怪物ID
        /// </summary>
        public uint MonsterID;

        /// <summary>
        /// 0: SkeletonPose; 1: PhysicalPart
        /// </summary>
        public byte DebugMode;

        /// <summary>
        /// 打开骨架调试功能
        /// </summary>
        public byte EnableDebug;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteUInt32(MonsterID, Endianness.Big);
            buffer.WriteByte(DebugMode);
            buffer.WriteByte(EnableDebug);
        }

        public void ReadCs(IBuffer buffer)
        {
            MonsterID = buffer.ReadUInt32(Endianness.Big);
            DebugMode = buffer.ReadByte();
            EnableDebug = buffer.ReadByte();
        }

    }
}
