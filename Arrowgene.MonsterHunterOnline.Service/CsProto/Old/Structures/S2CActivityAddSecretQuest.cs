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
    /// 红黄对抗添加secretquest
    /// </summary>
    public class S2CActivityAddSecretQuest : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(S2CActivityAddSecretQuest));

        public S2CActivityAddSecretQuest()
        {
            Camp = 0;
            EndTime = 0;
            Levels = new List<int>();
        }

        /// <summary>
        /// 阵营
        /// </summary>
        public int Camp;

        /// <summary>
        /// 结束时间
        /// </summary>
        public uint EndTime;

        /// <summary>
        /// 关卡集
        /// </summary>
        public List<int> Levels;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt32(Camp, Endianness.Big);
            buffer.WriteUInt32(EndTime, Endianness.Big);
            int levelsCount = (int)Levels.Count;
            buffer.WriteInt32(levelsCount, Endianness.Big);
            for (int i = 0; i < levelsCount; i++)
            {
                buffer.WriteInt32(Levels[i], Endianness.Big);
            }
        }

        public void ReadCs(IBuffer buffer)
        {
            Camp = buffer.ReadInt32(Endianness.Big);
            EndTime = buffer.ReadUInt32(Endianness.Big);
            Levels.Clear();
            int levelsCount = buffer.ReadInt32(Endianness.Big);
            for (int i = 0; i < levelsCount; i++)
            {
                int LevelsEntry = buffer.ReadInt32(Endianness.Big);
                Levels.Add(LevelsEntry);
            }
        }

    }
}
