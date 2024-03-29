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
    /// 日常
    /// </summary>
    public class CSDaily : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSDaily));

        public CSDaily()
        {
            RefreshTime = 0;
            Lib = 0;
            Task = new List<short>();
            CompleteCount = 0;
            RemainCount = 0;
        }

        /// <summary>
        /// 刷新时间
        /// </summary>
        public uint RefreshTime;

        /// <summary>
        /// 库
        /// </summary>
        public int Lib;

        /// <summary>
        /// 任务集
        /// </summary>
        public List<short> Task;

        /// <summary>
        /// 完成数
        /// </summary>
        public int CompleteCount;

        /// <summary>
        /// 剩余可完成数
        /// </summary>
        public int RemainCount;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteUInt32(RefreshTime, Endianness.Big);
            buffer.WriteInt32(Lib, Endianness.Big);
            short taskCount = (short)Task.Count;
            buffer.WriteInt16(taskCount, Endianness.Big);
            for (int i = 0; i < taskCount; i++)
            {
                buffer.WriteInt16(Task[i], Endianness.Big);
            }
            buffer.WriteInt32(CompleteCount, Endianness.Big);
            buffer.WriteInt32(RemainCount, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            RefreshTime = buffer.ReadUInt32(Endianness.Big);
            Lib = buffer.ReadInt32(Endianness.Big);
            Task.Clear();
            short taskCount = buffer.ReadInt16(Endianness.Big);
            for (int i = 0; i < taskCount; i++)
            {
                short TaskEntry = buffer.ReadInt16(Endianness.Big);
                Task.Add(TaskEntry);
            }
            CompleteCount = buffer.ReadInt32(Endianness.Big);
            RemainCount = buffer.ReadInt32(Endianness.Big);
        }

    }
}
