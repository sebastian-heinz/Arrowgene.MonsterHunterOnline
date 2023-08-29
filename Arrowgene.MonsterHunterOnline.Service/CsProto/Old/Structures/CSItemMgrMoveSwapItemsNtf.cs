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
    /// 通知移动交换多个物品
    /// </summary>
    public class CSItemMgrMoveSwapItemsNtf : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSItemMgrMoveSwapItemsNtf));

        public CSItemMgrMoveSwapItemsNtf()
        {
            MoveSwapItemsData = new List<MoveSwapItemInfo>();
        }

        /// <summary>
        /// 移动移动道具信息
        /// </summary>
        public List<MoveSwapItemInfo> MoveSwapItemsData;

        public void WriteCs(IBuffer buffer)
        {
            byte moveSwapItemsDataCount = (byte)MoveSwapItemsData.Count;
            buffer.WriteByte(moveSwapItemsDataCount);
            for (int i = 0; i < moveSwapItemsDataCount; i++)
            {
                MoveSwapItemsData[i].WriteCs(buffer);
            }
        }

        public void ReadCs(IBuffer buffer)
        {
            MoveSwapItemsData.Clear();
            byte moveSwapItemsDataCount = buffer.ReadByte();
            for (int i = 0; i < moveSwapItemsDataCount; i++)
            {
                MoveSwapItemInfo MoveSwapItemsDataEntry = new MoveSwapItemInfo();
                MoveSwapItemsDataEntry.ReadCs(buffer);
                MoveSwapItemsData.Add(MoveSwapItemsDataEntry);
            }
        }

    }
}
