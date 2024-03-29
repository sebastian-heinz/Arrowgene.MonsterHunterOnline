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

    public class CSFarmTrainPetReq : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSFarmTrainPetReq));

        public CSFarmTrainPetReq()
        {
            PetID = 0;
            FacilityType = 0;
            FacilityIndex = 0;
            FacilitySlot = 0;
        }

        /// <summary>
        /// 宠物ID
        /// </summary>
        public int PetID;

        /// <summary>
        /// 设施类型
        /// </summary>
        public int FacilityType;

        /// <summary>
        /// 设施索引
        /// </summary>
        public int FacilityIndex;

        /// <summary>
        /// 设施内槽位
        /// </summary>
        public short FacilitySlot;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt32(PetID, Endianness.Big);
            buffer.WriteInt32(FacilityType, Endianness.Big);
            buffer.WriteInt32(FacilityIndex, Endianness.Big);
            buffer.WriteInt16(FacilitySlot, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            PetID = buffer.ReadInt32(Endianness.Big);
            FacilityType = buffer.ReadInt32(Endianness.Big);
            FacilityIndex = buffer.ReadInt32(Endianness.Big);
            FacilitySlot = buffer.ReadInt16(Endianness.Big);
        }

    }
}
