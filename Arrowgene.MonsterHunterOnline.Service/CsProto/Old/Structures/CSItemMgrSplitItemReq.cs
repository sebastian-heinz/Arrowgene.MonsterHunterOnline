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
    /// 请求拆分物品
    /// </summary>
    public class CSItemMgrSplitItemReq : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSItemMgrSplitItemReq));

        public CSItemMgrSplitItemReq()
        {
            ItemID = 0;
            ItemColumn = 0;
            ItemGrid = 0;
            ItemCount = 0;
            DstColumn = 0;
            DstGrid = 0;
        }

        /// <summary>
        /// 物品实例
        /// </summary>
        public ulong ItemID;

        /// <summary>
        /// 物品栏位
        /// </summary>
        public byte ItemColumn;

        /// <summary>
        /// 物品格子
        /// </summary>
        public ushort ItemGrid;

        /// <summary>
        /// 数量
        /// </summary>
        public ushort ItemCount;

        /// <summary>
        /// 目标栏位
        /// </summary>
        public byte DstColumn;

        /// <summary>
        /// 目标格子
        /// </summary>
        public ushort DstGrid;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteUInt64(ItemID, Endianness.Big);
            buffer.WriteByte(ItemColumn);
            buffer.WriteUInt16(ItemGrid, Endianness.Big);
            buffer.WriteUInt16(ItemCount, Endianness.Big);
            buffer.WriteByte(DstColumn);
            buffer.WriteUInt16(DstGrid, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            ItemID = buffer.ReadUInt64(Endianness.Big);
            ItemColumn = buffer.ReadByte();
            ItemGrid = buffer.ReadUInt16(Endianness.Big);
            ItemCount = buffer.ReadUInt16(Endianness.Big);
            DstColumn = buffer.ReadByte();
            DstGrid = buffer.ReadUInt16(Endianness.Big);
        }

    }
}
