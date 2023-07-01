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
    /// pvp mode info
    /// </summary>
    public class CSPVPInfoNtf : IStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSPVPInfoNtf));

        public CSPVPInfoNtf()
        {
            Gang = new CSSPVPGang[CsProtoConstant.CS_MAX_GANG_COUNT];
            for (int i = 0; i < CsProtoConstant.CS_MAX_GANG_COUNT; i++)
            {
                Gang[i] = new CSSPVPGang();
            }
            CountDown = 0;
            Stage = 0;
            Result = 0;
            Goal = 0;
            Faction = 0;
            Event = new CSSPVPEvent();
        }

        public CSSPVPGang[] Gang;

        public int CountDown;

        public int Stage;

        public int Result;

        public int Goal;

        public int Faction;

        public CSSPVPEvent Event;

        public void Write(IBuffer buffer)
        {
            for (int i = 0; i < CsProtoConstant.CS_MAX_GANG_COUNT; i++)
            {
                Gang[i].Write(buffer);
            }
            buffer.WriteInt32(CountDown, Endianness.Big);
            buffer.WriteInt32(Stage, Endianness.Big);
            buffer.WriteInt32(Result, Endianness.Big);
            buffer.WriteInt32(Goal, Endianness.Big);
            buffer.WriteInt32(Faction, Endianness.Big);
            Event.Write(buffer);
        }

        public void Read(IBuffer buffer)
        {
            for (int i = 0; i < CsProtoConstant.CS_MAX_GANG_COUNT; i++)
            {
                Gang[i].Read(buffer);
            }
            CountDown = buffer.ReadInt32(Endianness.Big);
            Stage = buffer.ReadInt32(Endianness.Big);
            Result = buffer.ReadInt32(Endianness.Big);
            Goal = buffer.ReadInt32(Endianness.Big);
            Faction = buffer.ReadInt32(Endianness.Big);
            Event.Read(buffer);
        }

    }
}
