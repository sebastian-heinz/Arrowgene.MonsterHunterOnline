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
    /// 兑换实体卡
    /// </summary>
    public class SCExchangePhyCardRsp : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(SCExchangePhyCardRsp));

        public SCExchangePhyCardRsp()
        {
            ErrCode = 0;
            CardGroupId = 0;
            CardType = 0;
        }

        /// <summary>
        /// 错误码
        /// </summary>
        public int ErrCode;

        /// <summary>
        /// 卡组ID
        /// </summary>
        public int CardGroupId;

        /// <summary>
        /// 卡类型
        /// </summary>
        public int CardType;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt32(ErrCode, Endianness.Big);
            buffer.WriteInt32(CardGroupId, Endianness.Big);
            buffer.WriteInt32(CardType, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            ErrCode = buffer.ReadInt32(Endianness.Big);
            CardGroupId = buffer.ReadInt32(Endianness.Big);
            CardType = buffer.ReadInt32(Endianness.Big);
        }

    }
}
