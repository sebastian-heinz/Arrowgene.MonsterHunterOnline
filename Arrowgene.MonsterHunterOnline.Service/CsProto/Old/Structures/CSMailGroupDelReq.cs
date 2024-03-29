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

    public class CSMailGroupDelReq : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSMailGroupDelReq));

        public CSMailGroupDelReq()
        {
            MailId = new List<ulong>();
        }

        /// <summary>
        /// 批删邮件id
        /// </summary>
        public List<ulong> MailId;

        public void WriteCs(IBuffer buffer)
        {
            int mailIdCount = (int)MailId.Count;
            buffer.WriteInt32(mailIdCount, Endianness.Big);
            for (int i = 0; i < mailIdCount; i++)
            {
                buffer.WriteUInt64(MailId[i], Endianness.Big);
            }
        }

        public void ReadCs(IBuffer buffer)
        {
            MailId.Clear();
            int mailIdCount = buffer.ReadInt32(Endianness.Big);
            for (int i = 0; i < mailIdCount; i++)
            {
                ulong MailIdEntry = buffer.ReadUInt64(Endianness.Big);
                MailId.Add(MailIdEntry);
            }
        }

    }
}
