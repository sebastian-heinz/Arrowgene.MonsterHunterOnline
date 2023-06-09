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
    /// 服务器更新客户端物品，包括更新C2的情况
    /// </summary>
    public class CSUpdateItem : IStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSUpdateItem));

        public CSUpdateItem()
        {
            Reason = 0;
            GeneralItem = new List<CSGeneralItem>();
        }

        /// <summary>
        /// 原因
        /// </summary>
        public ushort Reason;

        /// <summary>
        /// 普通物品
        /// </summary>
        public List<CSGeneralItem> GeneralItem;

        public void Write(IBuffer buffer)
        {
            buffer.WriteUInt16(Reason, Endianness.Big);
            ushort generalItemCount = (ushort)GeneralItem.Count;
            buffer.WriteUInt16(generalItemCount, Endianness.Big);
            for (int i = 0; i < generalItemCount; i++)
            {
                GeneralItem[i].Write(buffer);
            }
        }

        public void Read(IBuffer buffer)
        {
            Reason = buffer.ReadUInt16(Endianness.Big);
            GeneralItem.Clear();
            ushort generalItemCount = buffer.ReadUInt16(Endianness.Big);
            for (int i = 0; i < generalItemCount; i++)
            {
                CSGeneralItem GeneralItemEntry = new CSGeneralItem();
                GeneralItemEntry.Read(buffer);
                GeneralItem.Add(GeneralItemEntry);
            }
        }

    }
}
