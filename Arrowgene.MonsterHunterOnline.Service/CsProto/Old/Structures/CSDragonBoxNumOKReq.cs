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
    /// 刷新数字确认
    /// </summary>
    public class CSDragonBoxNumOKReq : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSDragonBoxNumOKReq));

        public CSDragonBoxNumOKReq()
        {
            BoxID = 0;
            BitNumber = 0;
            TenNumber = 0;
        }

        /// <summary>
        /// 箱子ID
        /// </summary>
        public int BoxID;

        /// <summary>
        /// 个位数字确认
        /// </summary>
        public int BitNumber;

        /// <summary>
        /// 十位数字确认
        /// </summary>
        public int TenNumber;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt32(BoxID, Endianness.Big);
            buffer.WriteInt32(BitNumber, Endianness.Big);
            buffer.WriteInt32(TenNumber, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            BoxID = buffer.ReadInt32(Endianness.Big);
            BitNumber = buffer.ReadInt32(Endianness.Big);
            TenNumber = buffer.ReadInt32(Endianness.Big);
        }

    }
}
