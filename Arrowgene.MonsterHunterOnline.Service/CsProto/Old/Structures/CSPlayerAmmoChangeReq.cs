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
    /// 同步玩家换子弹行为
    /// </summary>
    public class CSPlayerAmmoChangeReq : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSPlayerAmmoChangeReq));

        public CSPlayerAmmoChangeReq()
        {
            NextAmmoID = 0;
            SubAmmoID = 0;
        }

        /// <summary>
        /// 激活子弹的道具ID
        /// </summary>
        public int NextAmmoID;

        /// <summary>
        /// 副子弹的道具ID
        /// </summary>
        public int SubAmmoID;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt32(NextAmmoID, Endianness.Big);
            buffer.WriteInt32(SubAmmoID, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            NextAmmoID = buffer.ReadInt32(Endianness.Big);
            SubAmmoID = buffer.ReadInt32(Endianness.Big);
        }

    }
}
