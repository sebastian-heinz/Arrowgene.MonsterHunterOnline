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
    /// 酋长奖励通知
    /// </summary>
    public class CSDragonBoxBlackFaceGiftNtf : IStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSDragonBoxBlackFaceGiftNtf));

        public CSDragonBoxBlackFaceGiftNtf()
        {
            OpType = 0;
            DragonBoxShopID = 0;
            OpenCount = 0;
            ItemList = new List<CSItemBoxItemEntry>();
        }

        /// <summary>
        /// 酋长奖励类型
        /// </summary>
        public int OpType;

        /// <summary>
        /// 小铺ID
        /// </summary>
        public int DragonBoxShopID;

        /// <summary>
        /// 开宝箱次数
        /// </summary>
        public int OpenCount;

        /// <summary>
        /// 物品列表
        /// </summary>
        public List<CSItemBoxItemEntry> ItemList;

        public void Write(IBuffer buffer)
        {
            buffer.WriteInt32(OpType, Endianness.Big);
            buffer.WriteInt32(DragonBoxShopID, Endianness.Big);
            buffer.WriteInt32(OpenCount, Endianness.Big);
            int itemListCount = (int)ItemList.Count;
            buffer.WriteInt32(itemListCount, Endianness.Big);
            for (int i = 0; i < itemListCount; i++)
            {
                ItemList[i].Write(buffer);
            }
        }

        public void Read(IBuffer buffer)
        {
            OpType = buffer.ReadInt32(Endianness.Big);
            DragonBoxShopID = buffer.ReadInt32(Endianness.Big);
            OpenCount = buffer.ReadInt32(Endianness.Big);
            ItemList.Clear();
            int itemListCount = buffer.ReadInt32(Endianness.Big);
            for (int i = 0; i < itemListCount; i++)
            {
                CSItemBoxItemEntry ItemListEntry = new CSItemBoxItemEntry();
                ItemListEntry.Read(buffer);
                ItemList.Add(ItemListEntry);
            }
        }

    }
}
