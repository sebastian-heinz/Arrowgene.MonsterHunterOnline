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
    /// 组队错误
    /// </summary>
    public class CSTeamErr : IStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSTeamErr));

        public CSTeamErr()
        {
            OpType = 0;
            _Err = 0;
            Name = "";
        }

        /// <summary>
        /// 操作
        /// </summary>
        public int OpType;

        /// <summary>
        /// 错误码
        /// </summary>
        public int _Err;

        /// <summary>
        /// 参数1
        /// </summary>
        public string Name;

        public void Write(IBuffer buffer)
        {
            buffer.WriteInt32(OpType, Endianness.Big);
            buffer.WriteInt32(_Err, Endianness.Big);
            buffer.WriteInt32(Name.Length + 1, Endianness.Big);
            buffer.WriteCString(Name);
        }

        public void Read(IBuffer buffer)
        {
            OpType = buffer.ReadInt32(Endianness.Big);
            _Err = buffer.ReadInt32(Endianness.Big);
            int NameEntryLen = buffer.ReadInt32(Endianness.Big);
            Name = buffer.ReadString(NameEntryLen);
        }

    }
}
