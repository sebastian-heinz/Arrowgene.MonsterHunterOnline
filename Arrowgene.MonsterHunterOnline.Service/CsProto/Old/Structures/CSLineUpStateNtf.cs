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
    /// 排副本状态变更
    /// </summary>
    public class CSLineUpStateNtf : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSLineUpStateNtf));

        public CSLineUpStateNtf()
        {
            RoomID = 0;
            State = 0;
        }

        /// <summary>
        /// 房间
        /// </summary>
        public uint RoomID;

        /// <summary>
        /// 状态
        /// </summary>
        public int State;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteUInt32(RoomID, Endianness.Big);
            buffer.WriteInt32(State, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            RoomID = buffer.ReadUInt32(Endianness.Big);
            State = buffer.ReadInt32(Endianness.Big);
        }

    }
}
