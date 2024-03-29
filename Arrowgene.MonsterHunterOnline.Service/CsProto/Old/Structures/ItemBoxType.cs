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
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{

    /// <summary>
    /// 机密研究院 抽奖盒子信息
    /// </summary>
    public class ItemBoxType : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(ItemBoxType));

        public ItemBoxType()
        {
            BoxId = 0;
            LotteryItemList = new LotteryItemType[CsProtoConstant.SECRET_RESEARCH_LAB_LOTTERY_ITEM_MAX_LEN];
            for (int i = 0; i < CsProtoConstant.SECRET_RESEARCH_LAB_LOTTERY_ITEM_MAX_LEN; i++)
            {
                LotteryItemList[i] = new LotteryItemType();
            }
            VipRefrshCount = 0;
            RefreshCount = 0;
            ResearchCount = 0;
        }

        /// <summary>
        /// 盒子ID
        /// </summary>
        public int BoxId;

        /// <summary>
        /// 奖励槽信息
        /// </summary>
        public LotteryItemType[] LotteryItemList;

        /// <summary>
        /// VIP刷新次数
        /// </summary>
        public int VipRefrshCount;

        /// <summary>
        /// 刷新次数
        /// </summary>
        public int RefreshCount;

        /// <summary>
        /// 研究次数
        /// </summary>
        public int ResearchCount;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt32(BoxId, Endianness.Big);
            for (int i = 0; i < CsProtoConstant.SECRET_RESEARCH_LAB_LOTTERY_ITEM_MAX_LEN; i++)
            {
                LotteryItemList[i].WriteCs(buffer);
            }
            buffer.WriteInt32(VipRefrshCount, Endianness.Big);
            buffer.WriteInt32(RefreshCount, Endianness.Big);
            buffer.WriteInt32(ResearchCount, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            BoxId = buffer.ReadInt32(Endianness.Big);
            for (int i = 0; i < CsProtoConstant.SECRET_RESEARCH_LAB_LOTTERY_ITEM_MAX_LEN; i++)
            {
                LotteryItemList[i].ReadCs(buffer);
            }
            VipRefrshCount = buffer.ReadInt32(Endianness.Big);
            RefreshCount = buffer.ReadInt32(Endianness.Big);
            ResearchCount = buffer.ReadInt32(Endianness.Big);
        }

    }
}
