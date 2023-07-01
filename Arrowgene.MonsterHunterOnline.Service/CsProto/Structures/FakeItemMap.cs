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
    /// 契约模式道具映射关系
    /// </summary>
    public class FakeItemMap : IStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(FakeItemMap));

        public FakeItemMap()
        {
            FakeItemID = new List<int>();
            ActItemID = new List<int>();
        }

        /// <summary>
        /// Fake临时道具ID
        /// </summary>
        public List<int> FakeItemID;

        /// <summary>
        /// 实际道具ID
        /// </summary>
        public List<int> ActItemID;

        public void Write(IBuffer buffer)
        {
            int fakeItemIDCount = (int)FakeItemID.Count;
            buffer.WriteInt32(fakeItemIDCount, Endianness.Big);
            for (int i = 0; i < fakeItemIDCount; i++)
            {
                buffer.WriteInt32(FakeItemID[i], Endianness.Big);
            }
            int actItemIDCount = (int)ActItemID.Count;
            buffer.WriteInt32(actItemIDCount, Endianness.Big);
            for (int i = 0; i < actItemIDCount; i++)
            {
                buffer.WriteInt32(ActItemID[i], Endianness.Big);
            }
        }

        public void Read(IBuffer buffer)
        {
            FakeItemID.Clear();
            int fakeItemIDCount = buffer.ReadInt32(Endianness.Big);
            for (int i = 0; i < fakeItemIDCount; i++)
            {
                int FakeItemIDEntry = buffer.ReadInt32(Endianness.Big);
                FakeItemID.Add(FakeItemIDEntry);
            }
            ActItemID.Clear();
            int actItemIDCount = buffer.ReadInt32(Endianness.Big);
            for (int i = 0; i < actItemIDCount; i++)
            {
                int ActItemIDEntry = buffer.ReadInt32(Endianness.Big);
                ActItemID.Add(ActItemIDEntry);
            }
        }

    }
}
