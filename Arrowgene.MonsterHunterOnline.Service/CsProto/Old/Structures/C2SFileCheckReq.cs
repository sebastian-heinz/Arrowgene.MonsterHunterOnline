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
    /// 客户端文件数据校验
    /// </summary>
    public class C2SFileCheckReq : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(C2SFileCheckReq));

        public C2SFileCheckReq()
        {
            Code = "";
            ScanFinish = 0;
            ScanTime = 0;
            TimeoutReScan = 0;
        }

        /// <summary>
        /// 校验码
        /// </summary>
        public string Code;

        /// <summary>
        /// 校验计算是否结束
        /// </summary>
        public byte ScanFinish;

        /// <summary>
        /// 校验计算时间
        /// </summary>
        public int ScanTime;

        /// <summary>
        /// 是否超时重算
        /// </summary>
        public byte TimeoutReScan;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt32(Code.Length + 1, Endianness.Big);
            buffer.WriteCString(Code);
            buffer.WriteByte(ScanFinish);
            buffer.WriteInt32(ScanTime, Endianness.Big);
            buffer.WriteByte(TimeoutReScan);
        }

        public void ReadCs(IBuffer buffer)
        {
            int CodeEntryLen = buffer.ReadInt32(Endianness.Big);
            Code = buffer.ReadString(CodeEntryLen);
            ScanFinish = buffer.ReadByte();
            ScanTime = buffer.ReadInt32(Endianness.Big);
            TimeoutReScan = buffer.ReadByte();
        }

    }
}
