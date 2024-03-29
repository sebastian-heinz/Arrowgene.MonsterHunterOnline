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
    /// 残兵重铸
    /// </summary>
    public class CSRebuildRandomWeaponReq : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSRebuildRandomWeaponReq));

        public CSRebuildRandomWeaponReq()
        {
            EquipColumn = 0;
            EquipGrid = 0;
            MirrorEquipColumn = 0;
            MirrorEquipGrid = 0;
            CreditCnt = 0;
            TagItemData = new List<TagItemList>();
        }

        /// <summary>
        /// 装备所在栏位
        /// </summary>
        public byte EquipColumn;

        /// <summary>
        /// 装备所在格子
        /// </summary>
        public ushort EquipGrid;

        /// <summary>
        /// 模板装备所在栏位
        /// </summary>
        public byte MirrorEquipColumn;

        /// <summary>
        /// 模板装备所在格子
        /// </summary>
        public ushort MirrorEquipGrid;

        /// <summary>
        /// 代币数量
        /// </summary>
        public byte CreditCnt;

        /// <summary>
        /// 分类道具列表
        /// </summary>
        public List<TagItemList> TagItemData;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteByte(EquipColumn);
            buffer.WriteUInt16(EquipGrid, Endianness.Big);
            buffer.WriteByte(MirrorEquipColumn);
            buffer.WriteUInt16(MirrorEquipGrid, Endianness.Big);
            buffer.WriteByte(CreditCnt);
            byte tagItemDataCount = (byte)TagItemData.Count;
            buffer.WriteByte(tagItemDataCount);
            for (int i = 0; i < tagItemDataCount; i++)
            {
                TagItemData[i].WriteCs(buffer);
            }
        }

        public void ReadCs(IBuffer buffer)
        {
            EquipColumn = buffer.ReadByte();
            EquipGrid = buffer.ReadUInt16(Endianness.Big);
            MirrorEquipColumn = buffer.ReadByte();
            MirrorEquipGrid = buffer.ReadUInt16(Endianness.Big);
            CreditCnt = buffer.ReadByte();
            TagItemData.Clear();
            byte tagItemDataCount = buffer.ReadByte();
            for (int i = 0; i < tagItemDataCount; i++)
            {
                TagItemList TagItemDataEntry = new TagItemList();
                TagItemDataEntry.ReadCs(buffer);
                TagItemData.Add(TagItemDataEntry);
            }
        }

    }
}
