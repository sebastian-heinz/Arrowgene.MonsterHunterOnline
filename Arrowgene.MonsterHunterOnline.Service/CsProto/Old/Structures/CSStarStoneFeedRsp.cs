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
    /// 星蕴石培养
    /// </summary>
    public class CSStarStoneFeedRsp : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSStarStoneFeedRsp));

        public CSStarStoneFeedRsp()
        {
            RetCode = 0;
            WaterExp = 0;
            FireExp = 0;
            ThunderExp = 0;
            DragonExp = 0;
            IceExp = 0;
        }

        /// <summary>
        /// 状态错误码
        /// </summary>
        public int RetCode;

        /// <summary>
        /// 水属性经验
        /// </summary>
        public int WaterExp;

        /// <summary>
        /// 火属性经验
        /// </summary>
        public int FireExp;

        /// <summary>
        /// 雷属性经验
        /// </summary>
        public int ThunderExp;

        /// <summary>
        /// 龙属性经验
        /// </summary>
        public int DragonExp;

        /// <summary>
        /// 冰属性经验
        /// </summary>
        public int IceExp;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt32(RetCode, Endianness.Big);
            buffer.WriteInt32(WaterExp, Endianness.Big);
            buffer.WriteInt32(FireExp, Endianness.Big);
            buffer.WriteInt32(ThunderExp, Endianness.Big);
            buffer.WriteInt32(DragonExp, Endianness.Big);
            buffer.WriteInt32(IceExp, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            RetCode = buffer.ReadInt32(Endianness.Big);
            WaterExp = buffer.ReadInt32(Endianness.Big);
            FireExp = buffer.ReadInt32(Endianness.Big);
            ThunderExp = buffer.ReadInt32(Endianness.Big);
            DragonExp = buffer.ReadInt32(Endianness.Big);
            IceExp = buffer.ReadInt32(Endianness.Big);
        }

    }
}
