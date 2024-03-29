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

    public class GuildFuncRecordInfo : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(GuildFuncRecordInfo));

        public GuildFuncRecordInfo()
        {
            OperType = 0;
            Executor = "";
            BeExecutored = "";
            args = 0;
            Time = 0;
            args2 = 0;
        }

        /// <summary>
        /// 类型--扩展用 以防策划未来要分类查询
        /// </summary>
        public int OperType;

        /// <summary>
        /// 执行者
        /// </summary>
        public string Executor;

        /// <summary>
        /// 被执行者
        /// </summary>
        public string BeExecutored;

        /// <summary>
        /// 备用参数
        /// </summary>
        public int args;

        /// <summary>
        /// 执行时间
        /// </summary>
        public uint Time;

        /// <summary>
        /// 备用参数2
        /// </summary>
        public int args2;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt32(OperType, Endianness.Big);
            buffer.WriteInt32(Executor.Length + 1, Endianness.Big);
            buffer.WriteCString(Executor);
            buffer.WriteInt32(BeExecutored.Length + 1, Endianness.Big);
            buffer.WriteCString(BeExecutored);
            buffer.WriteInt32(args, Endianness.Big);
            buffer.WriteUInt32(Time, Endianness.Big);
            buffer.WriteInt32(args2, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            OperType = buffer.ReadInt32(Endianness.Big);
            int ExecutorEntryLen = buffer.ReadInt32(Endianness.Big);
            Executor = buffer.ReadString(ExecutorEntryLen);
            int BeExecutoredEntryLen = buffer.ReadInt32(Endianness.Big);
            BeExecutored = buffer.ReadString(BeExecutoredEntryLen);
            args = buffer.ReadInt32(Endianness.Big);
            Time = buffer.ReadUInt32(Endianness.Big);
            args2 = buffer.ReadInt32(Endianness.Big);
        }

    }
}
