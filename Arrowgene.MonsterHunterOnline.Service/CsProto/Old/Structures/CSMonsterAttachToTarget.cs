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
    /// AttachToTarget
    /// </summary>
    public class CSMonsterAttachToTarget : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSMonsterAttachToTarget));

        public CSMonsterAttachToTarget()
        {
            SyncTime = 0;
            MonsterID = 0;
            TargetID = 0;
            AttachOffset = new CSVec3();
            AttachRotation = new CSVec3();
            AttachmentName = "";
        }

        /// <summary>
        /// 同步时间
        /// </summary>
        public long SyncTime;

        /// <summary>
        /// 怪物ID
        /// </summary>
        public uint MonsterID;

        /// <summary>
        /// Target ID
        /// </summary>
        public uint TargetID;

        /// <summary>
        /// Attach Offset
        /// </summary>
        public CSVec3 AttachOffset;

        /// <summary>
        /// Attach Rotation
        /// </summary>
        public CSVec3 AttachRotation;

        /// <summary>
        /// attachment name
        /// </summary>
        public string AttachmentName;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt64(SyncTime, Endianness.Big);
            buffer.WriteUInt32(MonsterID, Endianness.Big);
            buffer.WriteUInt32(TargetID, Endianness.Big);
            AttachOffset.WriteCs(buffer);
            AttachRotation.WriteCs(buffer);
            buffer.WriteInt32(AttachmentName.Length + 1, Endianness.Big);
            buffer.WriteCString(AttachmentName);
        }

        public void ReadCs(IBuffer buffer)
        {
            SyncTime = buffer.ReadInt64(Endianness.Big);
            MonsterID = buffer.ReadUInt32(Endianness.Big);
            TargetID = buffer.ReadUInt32(Endianness.Big);
            AttachOffset.ReadCs(buffer);
            AttachRotation.ReadCs(buffer);
            int AttachmentNameEntryLen = buffer.ReadInt32(Endianness.Big);
            AttachmentName = buffer.ReadString(AttachmentNameEntryLen);
        }

    }
}
