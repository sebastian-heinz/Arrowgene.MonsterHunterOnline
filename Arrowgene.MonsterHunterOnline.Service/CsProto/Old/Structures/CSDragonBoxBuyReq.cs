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
    /// 小铺购买请求
    /// </summary>
    public class CSDragonBoxBuyReq : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSDragonBoxBuyReq));

        public CSDragonBoxBuyReq()
        {
            BoxID = 0;
            ItemList = new List<CSItemBoxItemEntry>();
        }

        /// <summary>
        /// 箱子类型
        /// </summary>
        public int BoxID;

        /// <summary>
        /// 购买物品列表
        /// </summary>
        public List<CSItemBoxItemEntry> ItemList;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt32(BoxID, Endianness.Big);
            int itemListCount = (int)ItemList.Count;
            buffer.WriteInt32(itemListCount, Endianness.Big);
            for (int i = 0; i < itemListCount; i++)
            {
                ItemList[i].WriteCs(buffer);
            }
        }

        public void ReadCs(IBuffer buffer)
        {
            BoxID = buffer.ReadInt32(Endianness.Big);
            ItemList.Clear();
            int itemListCount = buffer.ReadInt32(Endianness.Big);
            for (int i = 0; i < itemListCount; i++)
            {
                CSItemBoxItemEntry ItemListEntry = new CSItemBoxItemEntry();
                ItemListEntry.ReadCs(buffer);
                ItemList.Add(ItemListEntry);
            }
        }

    }
}
