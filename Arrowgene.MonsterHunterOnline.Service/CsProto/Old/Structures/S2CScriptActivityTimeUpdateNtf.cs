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
    /// 服务器通知活动开始结束时间变化
    /// </summary>
    public class S2CScriptActivityTimeUpdateNtf : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(S2CScriptActivityTimeUpdateNtf));

        public S2CScriptActivityTimeUpdateNtf()
        {
            ID = 0;
            WorldSvrID = 0;
            StartTime = 0;
            StopTime = 0;
        }

        /// <summary>
        /// 活动ID
        /// </summary>
        public uint ID;

        /// <summary>
        /// worldsvrid
        /// </summary>
        public uint WorldSvrID;

        /// <summary>
        /// 开始时间
        /// </summary>
        public uint StartTime;

        /// <summary>
        /// 结束时间
        /// </summary>
        public uint StopTime;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteUInt32(ID, Endianness.Big);
            buffer.WriteUInt32(WorldSvrID, Endianness.Big);
            buffer.WriteUInt32(StartTime, Endianness.Big);
            buffer.WriteUInt32(StopTime, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            ID = buffer.ReadUInt32(Endianness.Big);
            WorldSvrID = buffer.ReadUInt32(Endianness.Big);
            StartTime = buffer.ReadUInt32(Endianness.Big);
            StopTime = buffer.ReadUInt32(Endianness.Big);
        }

    }
}
