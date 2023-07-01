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

    public class S2CLevelStatEvent : IStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(S2CLevelStatEvent));

        public S2CLevelStatEvent()
        {
            EventType = 0;
            EventTm = 0;
            ArgsList = new List<int>();
        }

        /// <summary>
        /// 事件类型
        /// </summary>
        public int EventType;

        /// <summary>
        /// 事件时间
        /// </summary>
        public uint EventTm;

        /// <summary>
        /// 参数列表
        /// </summary>
        public List<int> ArgsList;

        public void Write(IBuffer buffer)
        {
            buffer.WriteInt32(EventType, Endianness.Big);
            buffer.WriteUInt32(EventTm, Endianness.Big);
            int argsListCount = (int)ArgsList.Count;
            buffer.WriteInt32(argsListCount, Endianness.Big);
            for (int i = 0; i < argsListCount; i++)
            {
                buffer.WriteInt32(ArgsList[i], Endianness.Big);
            }
        }

        public void Read(IBuffer buffer)
        {
            EventType = buffer.ReadInt32(Endianness.Big);
            EventTm = buffer.ReadUInt32(Endianness.Big);
            ArgsList.Clear();
            int argsListCount = buffer.ReadInt32(Endianness.Big);
            for (int i = 0; i < argsListCount; i++)
            {
                int ArgsListEntry = buffer.ReadInt32(Endianness.Big);
                ArgsList.Add(ArgsListEntry);
            }
        }

    }
}
