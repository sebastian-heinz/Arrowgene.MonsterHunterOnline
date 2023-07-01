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
    /// 龙人秘宝奖励通知
    /// </summary>
    public class CSDragonBoxPrizeNtf : IStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSDragonBoxPrizeNtf));

        public CSDragonBoxPrizeNtf()
        {
            Result = 0;
            Id = 0;
            Type = 0;
            ItemList = new List<CSItemBoxItemEntry>();
            BagFull = 0;
        }

        /// <summary>
        /// 结果
        /// </summary>
        public int Result;

        /// <summary>
        /// 秘宝id
        /// </summary>
        public int Id;

        /// <summary>
        /// 奖励类型
        /// </summary>
        public byte Type;

        /// <summary>
        /// 物品列表
        /// </summary>
        public List<CSItemBoxItemEntry> ItemList;

        /// <summary>
        /// 背包是否满 1:满 0 ： 不满
        /// </summary>
        public byte BagFull;

        public void Write(IBuffer buffer)
        {
            buffer.WriteInt32(Result, Endianness.Big);
            buffer.WriteInt32(Id, Endianness.Big);
            buffer.WriteByte(Type);
            byte itemListCount = (byte)ItemList.Count;
            buffer.WriteByte(itemListCount);
            for (int i = 0; i < itemListCount; i++)
            {
                ItemList[i].Write(buffer);
            }
            buffer.WriteByte(BagFull);
        }

        public void Read(IBuffer buffer)
        {
            Result = buffer.ReadInt32(Endianness.Big);
            Id = buffer.ReadInt32(Endianness.Big);
            Type = buffer.ReadByte();
            ItemList.Clear();
            byte itemListCount = buffer.ReadByte();
            for (int i = 0; i < itemListCount; i++)
            {
                CSItemBoxItemEntry ItemListEntry = new CSItemBoxItemEntry();
                ItemListEntry.Read(buffer);
                ItemList.Add(ItemListEntry);
            }
            BagFull = buffer.ReadByte();
        }

    }
}
