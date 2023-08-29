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
    /// 猎团战预选赛排序列表
    /// </summary>
    public class CSGuildMatchQualifierResultListNtf : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSGuildMatchQualifierResultListNtf));

        public CSGuildMatchQualifierResultListNtf()
        {
            SignUpTeams = new List<CSGuildMatchSignUpTeam>();
        }

        /// <summary>
        /// 最大报名人数
        /// </summary>
        public List<CSGuildMatchSignUpTeam> SignUpTeams;

        public void WriteCs(IBuffer buffer)
        {
            uint signUpTeamsCount = (uint)SignUpTeams.Count;
            buffer.WriteUInt32(signUpTeamsCount, Endianness.Big);
            for (int i = 0; i < signUpTeamsCount; i++)
            {
                SignUpTeams[i].WriteCs(buffer);
            }
        }

        public void ReadCs(IBuffer buffer)
        {
            SignUpTeams.Clear();
            uint signUpTeamsCount = buffer.ReadUInt32(Endianness.Big);
            for (int i = 0; i < signUpTeamsCount; i++)
            {
                CSGuildMatchSignUpTeam SignUpTeamsEntry = new CSGuildMatchSignUpTeam();
                SignUpTeamsEntry.ReadCs(buffer);
                SignUpTeams.Add(SignUpTeamsEntry);
            }
        }

    }
}
