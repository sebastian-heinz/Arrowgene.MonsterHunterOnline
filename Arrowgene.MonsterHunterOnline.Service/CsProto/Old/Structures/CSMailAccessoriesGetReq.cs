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

    public class CSMailAccessoriesGetReq : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSMailAccessoriesGetReq));

        public CSMailAccessoriesGetReq()
        {
            MailId = 0;
            MailType = 0;
            MailAccessoryIndex = 0;
            AccStoreLocation = 0;
            DelType = 0;
        }

        /// <summary>
        /// 邮件id
        /// </summary>
        public ulong MailId;

        /// <summary>
        /// 0:玩家邮件;1:系统发送的邮件 2:礼物箱;3:索取箱; 0xff:所有类型的邮件
        /// </summary>
        public byte MailType;

        /// <summary>
        /// 邮件附件索引
        /// </summary>
        public int MailAccessoryIndex;

        /// <summary>
        /// 邮件附件存放位置(背包、仓库)
        /// </summary>
        public int AccStoreLocation;

        /// <summary>
        /// 删除方式
        /// </summary>
        public int DelType;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteUInt64(MailId, Endianness.Big);
            buffer.WriteByte(MailType);
            buffer.WriteInt32(MailAccessoryIndex, Endianness.Big);
            buffer.WriteInt32(AccStoreLocation, Endianness.Big);
            buffer.WriteInt32(DelType, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            MailId = buffer.ReadUInt64(Endianness.Big);
            MailType = buffer.ReadByte();
            MailAccessoryIndex = buffer.ReadInt32(Endianness.Big);
            AccStoreLocation = buffer.ReadInt32(Endianness.Big);
            DelType = buffer.ReadInt32(Endianness.Big);
        }

    }
}
