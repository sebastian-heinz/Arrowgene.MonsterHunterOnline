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
    /// 换装参数
    /// </summary>
    public class EquipParam : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(EquipParam));

        public EquipParam()
        {
            SrcColumn = 0;
            SrcGrid = 0;
            itemId = 0;
            DstColumn = 0;
            DstGrid = 0;
        }

        /// <summary>
        /// 栏位
        /// </summary>
        public ushort SrcColumn;

        /// <summary>
        /// 格子id
        /// </summary>
        public int SrcGrid;

        /// <summary>
        /// 物品id
        /// </summary>
        public ulong itemId;

        /// <summary>
        /// 栏位
        /// </summary>
        public ushort DstColumn;

        /// <summary>
        /// 格子id
        /// </summary>
        public int DstGrid;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteUInt16(SrcColumn, Endianness.Big);
            buffer.WriteInt32(SrcGrid, Endianness.Big);
            buffer.WriteUInt64(itemId, Endianness.Big);
            buffer.WriteUInt16(DstColumn, Endianness.Big);
            buffer.WriteInt32(DstGrid, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            SrcColumn = buffer.ReadUInt16(Endianness.Big);
            SrcGrid = buffer.ReadInt32(Endianness.Big);
            itemId = buffer.ReadUInt64(Endianness.Big);
            DstColumn = buffer.ReadUInt16(Endianness.Big);
            DstGrid = buffer.ReadInt32(Endianness.Big);
        }

    }
}
