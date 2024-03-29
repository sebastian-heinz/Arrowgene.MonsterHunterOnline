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
    /// 快捷补给操作
    /// </summary>
    public class SupplyStep : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(SupplyStep));

        public SupplyStep()
        {
            srcCol = 0;
            srcGrid = 0;
            dstCol = 0;
            dstGrid = 0;
            ItemType = 0;
            moveCnt = 0;
        }

        /// <summary>
        /// 起始栏位
        /// </summary>
        public short srcCol;

        /// <summary>
        /// 起始格子
        /// </summary>
        public int srcGrid;

        /// <summary>
        /// 目标栏位
        /// </summary>
        public short dstCol;

        /// <summary>
        /// 目标格子
        /// </summary>
        public int dstGrid;

        /// <summary>
        /// 道具类型
        /// </summary>
        public int ItemType;

        /// <summary>
        /// 移动数量
        /// </summary>
        public int moveCnt;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt16(srcCol, Endianness.Big);
            buffer.WriteInt32(srcGrid, Endianness.Big);
            buffer.WriteInt16(dstCol, Endianness.Big);
            buffer.WriteInt32(dstGrid, Endianness.Big);
            buffer.WriteInt32(ItemType, Endianness.Big);
            buffer.WriteInt32(moveCnt, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            srcCol = buffer.ReadInt16(Endianness.Big);
            srcGrid = buffer.ReadInt32(Endianness.Big);
            dstCol = buffer.ReadInt16(Endianness.Big);
            dstGrid = buffer.ReadInt32(Endianness.Big);
            ItemType = buffer.ReadInt32(Endianness.Big);
            moveCnt = buffer.ReadInt32(Endianness.Big);
        }

    }
}
