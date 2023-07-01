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
    /// 个人武器Style数据
    /// </summary>
    public class S2CWeaponStyleInfo : IStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(S2CWeaponStyleInfo));

        public S2CWeaponStyleInfo()
        {
            WeaponStyleData = new int[CsProtoConstant.MAX_WEAPOSTYLE_TYPE];
        }

        /// <summary>
        /// 各类武器的Style
        /// </summary>
        public int[] WeaponStyleData;

        public void Write(IBuffer buffer)
        {
            for (int i = 0; i < CsProtoConstant.MAX_WEAPOSTYLE_TYPE; i++)
            {
                buffer.WriteInt32(WeaponStyleData[i], Endianness.Big);
            }
        }

        public void Read(IBuffer buffer)
        {
            for (int i = 0; i < CsProtoConstant.MAX_WEAPOSTYLE_TYPE; i++)
            {
                WeaponStyleData[i] = buffer.ReadInt32(Endianness.Big);
            }
        }

    }
}
