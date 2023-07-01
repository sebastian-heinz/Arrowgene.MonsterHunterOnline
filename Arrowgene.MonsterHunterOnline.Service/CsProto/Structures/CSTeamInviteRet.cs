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
    /// 回复组队邀请
    /// </summary>
    public class CSTeamInviteRet : IStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSTeamInviteRet));

        public CSTeamInviteRet()
        {
            SrcPlayerId = 0;
            SrcTeamId = 0;
            InviteRet = 0;
            InviteTime = 0;
            VerifyCode = 0;
        }

        /// <summary>
        /// 邀请组队者
        /// </summary>
        public uint SrcPlayerId;

        /// <summary>
        /// 邀请者所在队伍
        /// </summary>
        public uint SrcTeamId;

        /// <summary>
        /// 回复0-同意，1-不同意
        /// </summary>
        public int InviteRet;

        /// <summary>
        /// 邀请时间
        /// </summary>
        public uint InviteTime;

        /// <summary>
        /// 验证码
        /// </summary>
        public uint VerifyCode;

        public void Write(IBuffer buffer)
        {
            buffer.WriteUInt32(SrcPlayerId, Endianness.Big);
            buffer.WriteUInt32(SrcTeamId, Endianness.Big);
            buffer.WriteInt32(InviteRet, Endianness.Big);
            buffer.WriteUInt32(InviteTime, Endianness.Big);
            buffer.WriteUInt32(VerifyCode, Endianness.Big);
        }

        public void Read(IBuffer buffer)
        {
            SrcPlayerId = buffer.ReadUInt32(Endianness.Big);
            SrcTeamId = buffer.ReadUInt32(Endianness.Big);
            InviteRet = buffer.ReadInt32(Endianness.Big);
            InviteTime = buffer.ReadUInt32(Endianness.Big);
            VerifyCode = buffer.ReadUInt32(Endianness.Big);
        }

    }
}
