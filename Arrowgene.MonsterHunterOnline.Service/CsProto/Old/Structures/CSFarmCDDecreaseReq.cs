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

    public class CSFarmCDDecreaseReq : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSFarmCDDecreaseReq));

        public CSFarmCDDecreaseReq()
        {
            FacilityType = 0;
            FacilityIndex = 0;
            PlowLandIndex = 0;
            CDType = 0;
            CDTime = 0;
            CostType = 0;
        }

        /// <summary>
        /// 设施类型
        /// </summary>
        public int FacilityType;

        /// <summary>
        /// 设施索引
        /// </summary>
        public int FacilityIndex;

        /// <summary>
        /// 田地索引
        /// </summary>
        public int PlowLandIndex;

        /// <summary>
        /// CDType
        /// </summary>
        public int CDType;

        /// <summary>
        /// CDTime
        /// </summary>
        public int CDTime;

        /// <summary>
        /// 类型 @EFarmCDCostType
        /// </summary>
        public byte CostType;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt32(FacilityType, Endianness.Big);
            buffer.WriteInt32(FacilityIndex, Endianness.Big);
            buffer.WriteInt32(PlowLandIndex, Endianness.Big);
            buffer.WriteInt32(CDType, Endianness.Big);
            buffer.WriteInt32(CDTime, Endianness.Big);
            buffer.WriteByte(CostType);
        }

        public void ReadCs(IBuffer buffer)
        {
            FacilityType = buffer.ReadInt32(Endianness.Big);
            FacilityIndex = buffer.ReadInt32(Endianness.Big);
            PlowLandIndex = buffer.ReadInt32(Endianness.Big);
            CDType = buffer.ReadInt32(Endianness.Big);
            CDTime = buffer.ReadInt32(Endianness.Big);
            CostType = buffer.ReadByte();
        }

    }
}
