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
    /// 请求精粹
    /// </summary>
    public class CSEquipQuenchReq : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSEquipQuenchReq));

        public CSEquipQuenchReq()
        {
            ItemUid = 0;
            EquipColumn = 0;
            EquipGrid = 0;
            DelAttrList = new List<int>();
        }

        /// <summary>
        /// 装备唯一ID
        /// </summary>
        public long ItemUid;

        /// <summary>
        /// 装备所在栏位
        /// </summary>
        public byte EquipColumn;

        /// <summary>
        /// 装备所在格子
        /// </summary>
        public ushort EquipGrid;

        /// <summary>
        /// 锁定属性列表
        /// </summary>
        public List<int> DelAttrList;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt64(ItemUid, Endianness.Big);
            buffer.WriteByte(EquipColumn);
            buffer.WriteUInt16(EquipGrid, Endianness.Big);
            byte delAttrListCount = (byte)DelAttrList.Count;
            buffer.WriteByte(delAttrListCount);
            for (int i = 0; i < delAttrListCount; i++)
            {
                buffer.WriteInt32(DelAttrList[i], Endianness.Big);
            }
        }

        public void ReadCs(IBuffer buffer)
        {
            ItemUid = buffer.ReadInt64(Endianness.Big);
            EquipColumn = buffer.ReadByte();
            EquipGrid = buffer.ReadUInt16(Endianness.Big);
            DelAttrList.Clear();
            byte delAttrListCount = buffer.ReadByte();
            for (int i = 0; i < delAttrListCount; i++)
            {
                int DelAttrListEntry = buffer.ReadInt32(Endianness.Big);
                DelAttrList.Add(DelAttrListEntry);
            }
        }

    }
}
