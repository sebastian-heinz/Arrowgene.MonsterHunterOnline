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
    /// 解锁宠物技能栏位
    /// </summary>
    public class C2SPetUnlockSkillSlot : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(C2SPetUnlockSkillSlot));

        public C2SPetUnlockSkillSlot()
        {
            Idx = 0;
            UID = 0;
            Type = 0;
            Index = 0;
        }

        /// <summary>
        /// pet index
        /// </summary>
        public int Idx;

        /// <summary>
        /// pet UID
        /// </summary>
        public int UID;

        /// <summary>
        /// 技能类型
        /// </summary>
        public byte Type;

        /// <summary>
        /// 栏位标识
        /// </summary>
        public byte Index;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt32(Idx, Endianness.Big);
            buffer.WriteInt32(UID, Endianness.Big);
            buffer.WriteByte(Type);
            buffer.WriteByte(Index);
        }

        public void ReadCs(IBuffer buffer)
        {
            Idx = buffer.ReadInt32(Endianness.Big);
            UID = buffer.ReadInt32(Endianness.Big);
            Type = buffer.ReadByte();
            Index = buffer.ReadByte();
        }

    }
}
