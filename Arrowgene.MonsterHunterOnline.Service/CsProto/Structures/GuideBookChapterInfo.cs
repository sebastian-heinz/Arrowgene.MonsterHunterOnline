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
    /// 玩家新手引导书章节信息 与csproto.xml保持一致
    /// </summary>
    public class GuideBookChapterInfo : IStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(GuideBookChapterInfo));

        public GuideBookChapterInfo()
        {
            ChapterId = 0;
            GetRewardCount = 0;
            IsNotNew = 0;
        }

        /// <summary>
        /// 章节id
        /// </summary>
        public int ChapterId;

        /// <summary>
        /// 领取奖励数
        /// </summary>
        public int GetRewardCount;

        /// <summary>
        /// 是否没有打开过
        /// </summary>
        public byte IsNotNew;

        public void Write(IBuffer buffer)
        {
            buffer.WriteInt32(ChapterId, Endianness.Big);
            buffer.WriteInt32(GetRewardCount, Endianness.Big);
            buffer.WriteByte(IsNotNew);
        }

        public void Read(IBuffer buffer)
        {
            ChapterId = buffer.ReadInt32(Endianness.Big);
            GetRewardCount = buffer.ReadInt32(Endianness.Big);
            IsNotNew = buffer.ReadByte();
        }

    }
}
