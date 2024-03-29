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
    /// pvp event
    /// </summary>
    public class CSSPVPEvent : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSSPVPEvent));

        public CSSPVPEvent()
        {
            EventType = 0;
            TimeStamp = 0;
            Stage = 0;
            Result = 0;
            KillerID = 0;
            KillerGang = 0;
            DeadID = 0;
            DeadGang = 0;
            Score = 0;
        }

        public int EventType;

        public uint TimeStamp;

        public int Stage;

        public int Result;

        public uint KillerID;

        public int KillerGang;

        public uint DeadID;

        public int DeadGang;

        public int Score;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt32(EventType, Endianness.Big);
            buffer.WriteUInt32(TimeStamp, Endianness.Big);
            buffer.WriteInt32(Stage, Endianness.Big);
            buffer.WriteInt32(Result, Endianness.Big);
            buffer.WriteUInt32(KillerID, Endianness.Big);
            buffer.WriteInt32(KillerGang, Endianness.Big);
            buffer.WriteUInt32(DeadID, Endianness.Big);
            buffer.WriteInt32(DeadGang, Endianness.Big);
            buffer.WriteInt32(Score, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            EventType = buffer.ReadInt32(Endianness.Big);
            TimeStamp = buffer.ReadUInt32(Endianness.Big);
            Stage = buffer.ReadInt32(Endianness.Big);
            Result = buffer.ReadInt32(Endianness.Big);
            KillerID = buffer.ReadUInt32(Endianness.Big);
            KillerGang = buffer.ReadInt32(Endianness.Big);
            DeadID = buffer.ReadUInt32(Endianness.Big);
            DeadGang = buffer.ReadInt32(Endianness.Big);
            Score = buffer.ReadInt32(Endianness.Big);
        }

    }
}
