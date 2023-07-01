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

    public class CSMailListGetResNoerr : CSMailListGetResResult
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSMailListGetResNoerr));

        public CSMailListGetResNoerr()
        {
            MailListPos = 0;
            MailType = 0;
            MailSubType = 0;
            mailCount = 0;
            MailListEntry = new List<CSMailListEntry>();
        }

        /// <summary>
        /// 起始记录
        /// </summary>
        public ushort MailListPos;

        /// <summary>
        /// 邮件类型
        /// </summary>
        public byte MailType;

        /// <summary>
        /// 邮件类型子类型
        /// </summary>
        public byte MailSubType;

        /// <summary>
        /// 邮件总数
        /// </summary>
        public int mailCount;

        /// <summary>
        /// 邮件列表
        /// </summary>
        public List<CSMailListEntry> MailListEntry;

        public void Write(IBuffer buffer)
        {
            buffer.WriteUInt16(MailListPos, Endianness.Big);
            buffer.WriteByte(MailType);
            buffer.WriteByte(MailSubType);
            buffer.WriteInt32(mailCount, Endianness.Big);
            int mailListEntryCount = (int)MailListEntry.Count;
            buffer.WriteInt32(mailListEntryCount, Endianness.Big);
            for (int i = 0; i < mailListEntryCount; i++)
            {
                MailListEntry[i].Write(buffer);
            }
        }

        public void Read(IBuffer buffer)
        {
            MailListPos = buffer.ReadUInt16(Endianness.Big);
            MailType = buffer.ReadByte();
            MailSubType = buffer.ReadByte();
            mailCount = buffer.ReadInt32(Endianness.Big);
            MailListEntry.Clear();
            int mailListEntryCount = buffer.ReadInt32(Endianness.Big);
            for (int i = 0; i < mailListEntryCount; i++)
            {
                CSMailListEntry MailListEntryEntry = new CSMailListEntry();
                MailListEntryEntry.Read(buffer);
                MailListEntry.Add(MailListEntryEntry);
            }
        }

    }
}
