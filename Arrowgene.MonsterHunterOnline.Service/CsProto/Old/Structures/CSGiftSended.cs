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
    /// 礼物成功发送
    /// </summary>
    public class CSGiftSended : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSGiftSended));

        public CSGiftSended()
        {
            goodID = 0;
            num = 0;
            PayChannel = 0;
            recvDBID = 0;
        }

        /// <summary>
        /// 商品ID
        /// </summary>
        public int goodID;

        /// <summary>
        /// 购买数量
        /// </summary>
        public int num;

        /// <summary>
        /// 支付通道0点券1微信
        /// </summary>
        public byte PayChannel;

        /// <summary>
        /// 赠送方DBID
        /// </summary>
        public ulong recvDBID;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt32(goodID, Endianness.Big);
            buffer.WriteInt32(num, Endianness.Big);
            buffer.WriteByte(PayChannel);
            buffer.WriteUInt64(recvDBID, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            goodID = buffer.ReadInt32(Endianness.Big);
            num = buffer.ReadInt32(Endianness.Big);
            PayChannel = buffer.ReadByte();
            recvDBID = buffer.ReadUInt64(Endianness.Big);
        }

    }
}
