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
    /// 购买商店中贩卖的物品的返回
    /// </summary>
    public class CSNpcShopBuyItemRsp : IStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSNpcShopBuyItemRsp));

        public CSNpcShopBuyItemRsp()
        {
            Ret = 0;
            ShopId = 0;
            SaleId = 0;
            Num = 0;
        }

        /// <summary>
        /// 返回值，0为成功
        /// </summary>
        public int Ret;

        /// <summary>
        /// 商店ID
        /// </summary>
        public int ShopId;

        /// <summary>
        /// 商品rowID
        /// </summary>
        public int SaleId;

        /// <summary>
        /// 购买的商品份数
        /// </summary>
        public int Num;

        public void Write(IBuffer buffer)
        {
            buffer.WriteInt32(Ret, Endianness.Big);
            buffer.WriteInt32(ShopId, Endianness.Big);
            buffer.WriteInt32(SaleId, Endianness.Big);
            buffer.WriteInt32(Num, Endianness.Big);
        }

        public void Read(IBuffer buffer)
        {
            Ret = buffer.ReadInt32(Endianness.Big);
            ShopId = buffer.ReadInt32(Endianness.Big);
            SaleId = buffer.ReadInt32(Endianness.Big);
            Num = buffer.ReadInt32(Endianness.Big);
        }

    }
}
