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
    /// 卖出背包中的物品的请求
    /// </summary>
    public class CSNpcShopSellItemReq : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSNpcShopSellItemReq));

        public CSNpcShopSellItemReq()
        {
            Column = 0;
            Grid = 0;
            Count = 0;
            IsUnifiedStore = 0;
        }

        /// <summary>
        /// 物品存放的Column位置
        /// </summary>
        public int Column;

        /// <summary>
        /// 物品存放的Grid位置
        /// </summary>
        public int Grid;

        /// <summary>
        /// 卖出数量
        /// </summary>
        public int Count;

        /// <summary>
        /// 是否统一商店
        /// </summary>
        public int IsUnifiedStore;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt32(Column, Endianness.Big);
            buffer.WriteInt32(Grid, Endianness.Big);
            buffer.WriteInt32(Count, Endianness.Big);
            buffer.WriteInt32(IsUnifiedStore, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            Column = buffer.ReadInt32(Endianness.Big);
            Grid = buffer.ReadInt32(Endianness.Big);
            Count = buffer.ReadInt32(Endianness.Big);
            IsUnifiedStore = buffer.ReadInt32(Endianness.Big);
        }

    }
}
