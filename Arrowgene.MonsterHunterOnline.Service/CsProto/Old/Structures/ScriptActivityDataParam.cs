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

    public class ScriptActivityDataParam : ScriptActivityDataUnion
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(ScriptActivityDataParam));

        public ScriptActivityDataParam()
        {
            Param1 = 0;
            Param2 = 0;
            Param3 = 0;
            Param4 = 0;
            Param5 = "";
            Param6 = "";
            Param7 = 0;
            Param8 = 0;
            Param9 = 0;
            Param10 = "";
        }

        public EScriptActivityDataType Type => EScriptActivityDataType.kScriptActivityDataType_Param;

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
        /// 参数5
        /// </summary>
        public string Param5;

        /// <summary>
        /// 参数6
        /// </summary>
        public string Param6;

        /// <summary>
        /// 参数7
        /// </summary>
        public int Param7;

        /// <summary>
        /// 参数8
        /// </summary>
        public int Param8;

        /// <summary>
        /// 参数9
        /// </summary>
        public int Param9;

        /// <summary>
        /// 参数10
        /// </summary>
        public string Param10;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt32(Param1, Endianness.Big);
            buffer.WriteInt32(Param2, Endianness.Big);
            buffer.WriteInt32(Param3, Endianness.Big);
            buffer.WriteInt32(Param4, Endianness.Big);
            buffer.WriteInt32(Param5.Length + 1, Endianness.Big);
            buffer.WriteCString(Param5);
            buffer.WriteInt32(Param6.Length + 1, Endianness.Big);
            buffer.WriteCString(Param6);
            buffer.WriteInt32(Param7, Endianness.Big);
            buffer.WriteInt32(Param8, Endianness.Big);
            buffer.WriteInt32(Param9, Endianness.Big);
            buffer.WriteInt32(Param10.Length + 1, Endianness.Big);
            buffer.WriteCString(Param10);
        }

        public void ReadCs(IBuffer buffer)
        {
            Param1 = buffer.ReadInt32(Endianness.Big);
            Param2 = buffer.ReadInt32(Endianness.Big);
            Param3 = buffer.ReadInt32(Endianness.Big);
            Param4 = buffer.ReadInt32(Endianness.Big);
            int Param5EntryLen = buffer.ReadInt32(Endianness.Big);
            Param5 = buffer.ReadString(Param5EntryLen);
            int Param6EntryLen = buffer.ReadInt32(Endianness.Big);
            Param6 = buffer.ReadString(Param6EntryLen);
            Param7 = buffer.ReadInt32(Endianness.Big);
            Param8 = buffer.ReadInt32(Endianness.Big);
            Param9 = buffer.ReadInt32(Endianness.Big);
            int Param10EntryLen = buffer.ReadInt32(Endianness.Big);
            Param10 = buffer.ReadString(Param10EntryLen);
        }

    }
}
