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
    /// 请求精铸
    /// </summary>
    public class CSItemRebuildFoundRes : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSItemRebuildFoundRes));

        public CSItemRebuildFoundRes()
        {
            EquipColumn = 0;
            EquipGrid = 0;
            EquipId = 0;
            LockAttr = 0;
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
        /// 装备实例ID
        /// </summary>
        public ulong EquipId;

        /// <summary>
        /// 锁定属性
        /// </summary>
        public byte LockAttr;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteByte(EquipColumn);
            buffer.WriteUInt16(EquipGrid, Endianness.Big);
            buffer.WriteUInt64(EquipId, Endianness.Big);
            buffer.WriteByte(LockAttr);
        }

        public void ReadCs(IBuffer buffer)
        {
            EquipColumn = buffer.ReadByte();
            EquipGrid = buffer.ReadUInt16(Endianness.Big);
            EquipId = buffer.ReadUInt64(Endianness.Big);
            LockAttr = buffer.ReadByte();
        }

    }
}
