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

    public class CSTokenSyncEntry : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSTokenSyncEntry));

        public CSTokenSyncEntry(CSTokenSyncVar _Value)
        {
            Name = "";
            Value = _Value;
        }

        /// <summary>
        /// Name of Token
        /// </summary>
        public string Name;

        /// <summary>
        /// 类型枚举
        /// </summary>

        /// <summary>
        /// 变量值
        /// </summary>
        public CSTokenSyncVar Value;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt32(Name.Length + 1, Endianness.Big);
            buffer.WriteCString(Name);
            buffer.WriteUInt16((ushort)Value.Type, Endianness.Big);
            Value.WriteCs(buffer);
        }

        public void ReadCs(IBuffer buffer)
        {
            int NameEntryLen = buffer.ReadInt32(Endianness.Big);
            Name = buffer.ReadString(NameEntryLen);
            CS_TOKEN_SYNC_TYPE CSTokenSyncVar_Type = (CS_TOKEN_SYNC_TYPE)buffer.ReadUInt16(Endianness.Big);
            Value.ReadCs(buffer);
        }

    }
}
