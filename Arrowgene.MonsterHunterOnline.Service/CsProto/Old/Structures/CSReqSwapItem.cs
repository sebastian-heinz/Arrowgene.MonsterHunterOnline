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
    /// 请求交换2个位置的物品
    /// </summary>
    public class CSReqSwapItem : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSReqSwapItem));

        public CSReqSwapItem()
        {
            SrcColumn = 0;
            SrcIndex = 0;
            DstColumn = 0;
            DstIndex = 0;
            AutoDown = 0;
        }

        /// <summary>
        /// 原栏标志
        /// </summary>
        public byte SrcColumn;

        /// <summary>
        /// 原栏位置
        /// </summary>
        public ushort SrcIndex;

        /// <summary>
        /// 目标栏标志
        /// </summary>
        public byte DstColumn;

        /// <summary>
        /// 目标栏位置
        /// </summary>
        public ushort DstIndex;

        /// <summary>
        /// 远近程不匹配是否自动下掉原有的
        /// </summary>
        public byte AutoDown;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteByte(SrcColumn);
            buffer.WriteUInt16(SrcIndex, Endianness.Big);
            buffer.WriteByte(DstColumn);
            buffer.WriteUInt16(DstIndex, Endianness.Big);
            buffer.WriteByte(AutoDown);
        }

        public void ReadCs(IBuffer buffer)
        {
            SrcColumn = buffer.ReadByte();
            SrcIndex = buffer.ReadUInt16(Endianness.Big);
            DstColumn = buffer.ReadByte();
            DstIndex = buffer.ReadUInt16(Endianness.Big);
            AutoDown = buffer.ReadByte();
        }

    }
}
