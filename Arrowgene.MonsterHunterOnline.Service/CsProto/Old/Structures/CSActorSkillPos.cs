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
    /// 技能位置同步消息
    /// </summary>
    public class CSActorSkillPos : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSActorSkillPos));

        public CSActorSkillPos()
        {
            NetObjId = 0;
            StateID = 0;
            CurTime = 0.0f;
            Location = new CSVec3();
            Flag = 0;
        }

        /// <summary>
        /// NetObjId
        /// </summary>
        public uint NetObjId;

        /// <summary>
        /// 当前State的ID
        /// </summary>
        public uint StateID;

        /// <summary>
        /// 技能时间
        /// </summary>
        public float CurTime;

        /// <summary>
        /// 世界坐标系下的位置
        /// </summary>
        public CSVec3 Location;

        /// <summary>
        /// Flag
        /// </summary>
        public ushort Flag;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteUInt32(NetObjId, Endianness.Big);
            buffer.WriteUInt32(StateID, Endianness.Big);
            buffer.WriteFloat(CurTime, Endianness.Big);
            Location.WriteCs(buffer);
            buffer.WriteUInt16(Flag, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            NetObjId = buffer.ReadUInt32(Endianness.Big);
            StateID = buffer.ReadUInt32(Endianness.Big);
            CurTime = buffer.ReadFloat(Endianness.Big);
            Location.ReadCs(buffer);
            Flag = buffer.ReadUInt16(Endianness.Big);
        }

    }
}
