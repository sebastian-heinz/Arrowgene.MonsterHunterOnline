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

    public class ScriptActivityParams : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(ScriptActivityParams));

        public ScriptActivityParams()
        {
            Param1 = 0;
            Param2 = 0;
            Param3 = 0;
            Param4 = 0;
            Param5 = 0;
            Param6 = 0;
            Str1 = "";
            Str2 = "";
        }

        /// <summary>
        /// 参数1
        /// </summary>
        public int Param1;

        /// <summary>
        /// 参数2
        /// </summary>
        public int Param2;

        /// <summary>
        /// 参数3
        /// </summary>
        public int Param3;

        /// <summary>
        /// 参数4
        /// </summary>
        public int Param4;

        /// <summary>
        /// 参数7
        /// </summary>
        public int Param5;

        /// <summary>
        /// 参数7
        /// </summary>
        public int Param6;

        /// <summary>
        /// 参数5
        /// </summary>
        public string Str1;

        /// <summary>
        /// 参数6
        /// </summary>
        public string Str2;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt32(Param1, Endianness.Big);
            buffer.WriteInt32(Param2, Endianness.Big);
            buffer.WriteInt32(Param3, Endianness.Big);
            buffer.WriteInt32(Param4, Endianness.Big);
            buffer.WriteInt32(Param5, Endianness.Big);
            buffer.WriteInt32(Param6, Endianness.Big);
            buffer.WriteInt32(Str1.Length + 1, Endianness.Big);
            buffer.WriteCString(Str1);
            buffer.WriteInt32(Str2.Length + 1, Endianness.Big);
            buffer.WriteCString(Str2);
        }

        public void ReadCs(IBuffer buffer)
        {
            Param1 = buffer.ReadInt32(Endianness.Big);
            Param2 = buffer.ReadInt32(Endianness.Big);
            Param3 = buffer.ReadInt32(Endianness.Big);
            Param4 = buffer.ReadInt32(Endianness.Big);
            Param5 = buffer.ReadInt32(Endianness.Big);
            Param6 = buffer.ReadInt32(Endianness.Big);
            int Str1EntryLen = buffer.ReadInt32(Endianness.Big);
            Str1 = buffer.ReadString(Str1EntryLen);
            int Str2EntryLen = buffer.ReadInt32(Endianness.Big);
            Str2 = buffer.ReadString(Str2EntryLen);
        }

    }
}
