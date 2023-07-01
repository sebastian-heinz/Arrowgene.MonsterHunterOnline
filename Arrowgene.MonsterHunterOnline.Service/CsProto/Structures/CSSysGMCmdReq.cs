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
    /// 客户端系统控制方面的GM命令请求
    /// </summary>
    public class CSSysGMCmdReq : IStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSSysGMCmdReq));

        public CSSysGMCmdReq()
        {
            CmdName = "";
            Param1 = "";
            Param2 = "";
            Param3 = "";
        }

        /// <summary>
        /// GM命令名称
        /// </summary>
        public string CmdName;

        /// <summary>
        /// GM参数1
        /// </summary>
        public string Param1;

        /// <summary>
        /// GM参数2
        /// </summary>
        public string Param2;

        /// <summary>
        /// 公告内容
        /// </summary>
        public string Param3;

        public void Write(IBuffer buffer)
        {
            buffer.WriteInt32(CmdName.Length + 1, Endianness.Big);
            buffer.WriteCString(CmdName);
            buffer.WriteInt32(Param1.Length + 1, Endianness.Big);
            buffer.WriteCString(Param1);
            buffer.WriteInt32(Param2.Length + 1, Endianness.Big);
            buffer.WriteCString(Param2);
            buffer.WriteInt32(Param3.Length + 1, Endianness.Big);
            buffer.WriteCString(Param3);
        }

        public void Read(IBuffer buffer)
        {
            int CmdNameEntryLen = buffer.ReadInt32(Endianness.Big);
            CmdName = buffer.ReadString(CmdNameEntryLen);
            int Param1EntryLen = buffer.ReadInt32(Endianness.Big);
            Param1 = buffer.ReadString(Param1EntryLen);
            int Param2EntryLen = buffer.ReadInt32(Endianness.Big);
            Param2 = buffer.ReadString(Param2EntryLen);
            int Param3EntryLen = buffer.ReadInt32(Endianness.Big);
            Param3 = buffer.ReadString(Param3EntryLen);
        }

    }
}
