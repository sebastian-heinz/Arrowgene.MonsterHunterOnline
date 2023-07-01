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
    /// 客户端玩家复活死亡相关信息
    /// </summary>
    public class ClientReviveState : IStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(ClientReviveState));

        public ClientReviveState()
        {
            DeadState = 0;
            DeadTime = 0;
            SvrTime = 0;
            SysTime = 0;
            FreeRevive = 0;
            TicketRevive = 0;
        }

        /// <summary>
        /// 是否死亡
        /// </summary>
        public int DeadState;

        /// <summary>
        /// 死亡时间
        /// </summary>
        public uint DeadTime;

        /// <summary>
        /// 客户端服务器时间
        /// </summary>
        public uint SvrTime;

        /// <summary>
        /// 客户端系统时间
        /// </summary>
        public uint SysTime;

        /// <summary>
        /// 能否免费复活
        /// </summary>
        public int FreeRevive;

        /// <summary>
        /// 能否用票复活
        /// </summary>
        public int TicketRevive;

        public void Write(IBuffer buffer)
        {
            buffer.WriteInt32(DeadState, Endianness.Big);
            buffer.WriteUInt32(DeadTime, Endianness.Big);
            buffer.WriteUInt32(SvrTime, Endianness.Big);
            buffer.WriteUInt32(SysTime, Endianness.Big);
            buffer.WriteInt32(FreeRevive, Endianness.Big);
            buffer.WriteInt32(TicketRevive, Endianness.Big);
        }

        public void Read(IBuffer buffer)
        {
            DeadState = buffer.ReadInt32(Endianness.Big);
            DeadTime = buffer.ReadUInt32(Endianness.Big);
            SvrTime = buffer.ReadUInt32(Endianness.Big);
            SysTime = buffer.ReadUInt32(Endianness.Big);
            FreeRevive = buffer.ReadInt32(Endianness.Big);
            TicketRevive = buffer.ReadInt32(Endianness.Big);
        }

    }
}
