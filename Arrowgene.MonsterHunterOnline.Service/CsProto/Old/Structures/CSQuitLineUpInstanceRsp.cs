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
    /// 退出排副本
    /// </summary>
    public class CSQuitLineUpInstanceRsp : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSQuitLineUpInstanceRsp));

        public CSQuitLineUpInstanceRsp()
        {
            ErrCode = 0;
            NetID = 0;
            Name = "";
            CurRoomMemCount = 0;
            MaxRoomMemCount = 0;
        }

        /// <summary>
        /// 错误码
        /// </summary>
        public int ErrCode;

        /// <summary>
        /// 玩家ID
        /// </summary>
        public uint NetID;

        /// <summary>
        /// 角色名
        /// </summary>
        public string Name;

        /// <summary>
        /// 当前的房间人数
        /// </summary>
        public int CurRoomMemCount;

        /// <summary>
        /// 最大的房间人数
        /// </summary>
        public int MaxRoomMemCount;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt32(ErrCode, Endianness.Big);
            buffer.WriteUInt32(NetID, Endianness.Big);
            buffer.WriteInt32(Name.Length + 1, Endianness.Big);
            buffer.WriteCString(Name);
            buffer.WriteInt32(CurRoomMemCount, Endianness.Big);
            buffer.WriteInt32(MaxRoomMemCount, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            ErrCode = buffer.ReadInt32(Endianness.Big);
            NetID = buffer.ReadUInt32(Endianness.Big);
            int NameEntryLen = buffer.ReadInt32(Endianness.Big);
            Name = buffer.ReadString(NameEntryLen);
            CurRoomMemCount = buffer.ReadInt32(Endianness.Big);
            MaxRoomMemCount = buffer.ReadInt32(Endianness.Big);
        }

    }
}
