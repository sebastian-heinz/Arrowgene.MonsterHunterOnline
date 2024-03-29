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
    /// 方案快捷保存
    /// </summary>
    public class CSSupplyPlanSaveRsp : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSSupplyPlanSaveRsp));

        public CSSupplyPlanSaveRsp()
        {
            iRetCode = 0;
            PlanID = 0;
            PlanName = "";
            ItemType = new List<int>();
            ItemCount = new List<int>();
            PosGrid = new List<int>();
        }

        /// <summary>
        /// 响应码
        /// </summary>
        public int iRetCode;

        /// <summary>
        /// 方案ID
        /// </summary>
        public int PlanID;

        /// <summary>
        /// 方案名
        /// </summary>
        public string PlanName;

        /// <summary>
        /// 道具类型
        /// </summary>
        public List<int> ItemType;

        /// <summary>
        /// 道具数量
        /// </summary>
        public List<int> ItemCount;

        /// <summary>
        /// 格子位置
        /// </summary>
        public List<int> PosGrid;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt32(iRetCode, Endianness.Big);
            buffer.WriteInt32(PlanID, Endianness.Big);
            buffer.WriteInt32(PlanName.Length + 1, Endianness.Big);
            buffer.WriteCString(PlanName);
            int itemTypeCount = (int)ItemType.Count;
            buffer.WriteInt32(itemTypeCount, Endianness.Big);
            for (int i = 0; i < itemTypeCount; i++)
            {
                buffer.WriteInt32(ItemType[i], Endianness.Big);
            }
            int itemCountCount = (int)ItemCount.Count;
            buffer.WriteInt32(itemCountCount, Endianness.Big);
            for (int i = 0; i < itemCountCount; i++)
            {
                buffer.WriteInt32(ItemCount[i], Endianness.Big);
            }
            int posGridCount = (int)PosGrid.Count;
            buffer.WriteInt32(posGridCount, Endianness.Big);
            for (int i = 0; i < posGridCount; i++)
            {
                buffer.WriteInt32(PosGrid[i], Endianness.Big);
            }
        }

        public void ReadCs(IBuffer buffer)
        {
            iRetCode = buffer.ReadInt32(Endianness.Big);
            PlanID = buffer.ReadInt32(Endianness.Big);
            int PlanNameEntryLen = buffer.ReadInt32(Endianness.Big);
            PlanName = buffer.ReadString(PlanNameEntryLen);
            ItemType.Clear();
            int itemTypeCount = buffer.ReadInt32(Endianness.Big);
            for (int i = 0; i < itemTypeCount; i++)
            {
                int ItemTypeEntry = buffer.ReadInt32(Endianness.Big);
                ItemType.Add(ItemTypeEntry);
            }
            ItemCount.Clear();
            int itemCountCount = buffer.ReadInt32(Endianness.Big);
            for (int i = 0; i < itemCountCount; i++)
            {
                int ItemCountEntry = buffer.ReadInt32(Endianness.Big);
                ItemCount.Add(ItemCountEntry);
            }
            PosGrid.Clear();
            int posGridCount = buffer.ReadInt32(Endianness.Big);
            for (int i = 0; i < posGridCount; i++)
            {
                int PosGridEntry = buffer.ReadInt32(Endianness.Big);
                PosGrid.Add(PosGridEntry);
            }
        }

    }
}
