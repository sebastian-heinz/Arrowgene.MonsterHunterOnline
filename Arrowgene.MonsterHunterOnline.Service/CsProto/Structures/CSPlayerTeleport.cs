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
    /// 玩家传送消息
    /// </summary>
    public class CSPlayerTeleport : IStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSPlayerTeleport));

        public CSPlayerTeleport()
        {
            SyncTime = 0;
            NetObjId = 0;
            Region = 0;
            TargetPos = new CSQuatT();
            ParentGUID = 0;
            InitState = 0;
        }

        /// <summary>
        /// 同步时间
        /// </summary>
        public long SyncTime;

        /// <summary>
        /// Actor的NetObjId
        /// </summary>
        public uint NetObjId;

        /// <summary>
        /// Region
        /// </summary>
        public int Region;

        /// <summary>
        /// 目标点位置和朝向
        /// </summary>
        public CSQuatT TargetPos;

        /// <summary>
        /// parent entityGUID
        /// </summary>
        public long ParentGUID;

        /// <summary>
        /// 是否重置状态
        /// </summary>
        public byte InitState;

        public void Write(IBuffer buffer)
        {
            buffer.WriteInt64(SyncTime, Endianness.Big);
            buffer.WriteUInt32(NetObjId, Endianness.Big);
            buffer.WriteInt32(Region, Endianness.Big);
            TargetPos.Write(buffer);
            buffer.WriteInt64(ParentGUID, Endianness.Big);
            buffer.WriteByte(InitState);
        }

        public void Read(IBuffer buffer)
        {
            SyncTime = buffer.ReadInt64(Endianness.Big);
            NetObjId = buffer.ReadUInt32(Endianness.Big);
            Region = buffer.ReadInt32(Endianness.Big);
            TargetPos.Read(buffer);
            ParentGUID = buffer.ReadInt64(Endianness.Big);
            InitState = buffer.ReadByte();
        }

    }
}
