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
    /// 方案解锁变更请求
    /// </summary>
    public class CSEquipPlanChangeNtf : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSEquipPlanChangeNtf));

        public CSEquipPlanChangeNtf()
        {
            PlanId = 0;
            ActionFlag = 0;
            FashionFlag = 0;
        }

        /// <summary>
        /// 方案Index
        /// </summary>
        public int PlanId;

        /// <summary>
        /// 1开始, 0结束
        /// </summary>
        public int ActionFlag;

        /// <summary>
        /// 时装脱卸flag
        /// </summary>
        public int FashionFlag;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt32(PlanId, Endianness.Big);
            buffer.WriteInt32(ActionFlag, Endianness.Big);
            buffer.WriteInt32(FashionFlag, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            PlanId = buffer.ReadInt32(Endianness.Big);
            ActionFlag = buffer.ReadInt32(Endianness.Big);
            FashionFlag = buffer.ReadInt32(Endianness.Big);
        }

    }
}
