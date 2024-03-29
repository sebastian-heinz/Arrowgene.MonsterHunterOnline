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
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;
using Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{

    /// <summary>
    /// 通知客户端添加道具
    /// </summary>
    public class CSTradeAddNormalNtf : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSTradeAddNormalNtf));

        public CSTradeAddNormalNtf()
        {
            PlayerId = 0;
            DstGrid = 0;
            ItemId = 0;
            Count = 0;
            NormalItem = new Item();
        }

        /// <summary>
        /// 角色ID
        /// </summary>
        public int PlayerId;

        /// <summary>
        /// 目标位置
        /// </summary>
        public ushort DstGrid;

        /// <summary>
        /// 道具ID
        /// </summary>
        public int ItemId;

        /// <summary>
        /// 物品数量
        /// </summary>
        public ushort Count;

        /// <summary>
        /// 道具数据
        /// </summary>
        public Item NormalItem;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt32(PlayerId, Endianness.Big);
            buffer.WriteUInt16(DstGrid, Endianness.Big);
            buffer.WriteInt32(ItemId, Endianness.Big);
            buffer.WriteUInt16(Count, Endianness.Big);
            NormalItem.WriteTlv(buffer);
        }

        public void ReadCs(IBuffer buffer)
        {
            PlayerId = buffer.ReadInt32(Endianness.Big);
            DstGrid = buffer.ReadUInt16(Endianness.Big);
            ItemId = buffer.ReadInt32(Endianness.Big);
            Count = buffer.ReadUInt16(Endianness.Big);
            NormalItem.ReadTlv(buffer);
        }

    }
}
