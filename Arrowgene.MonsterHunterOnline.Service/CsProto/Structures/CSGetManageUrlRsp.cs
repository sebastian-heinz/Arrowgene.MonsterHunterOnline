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
    /// 敏感操作管理页面URL响应
    /// </summary>
    public class CSGetManageUrlRsp : IStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSGetManageUrlRsp));

        public CSGetManageUrlRsp()
        {
            Type = 0;
            Result = 0;
            Url = "";
        }

        /// <summary>
        /// 管理页面类型
        /// </summary>
        public uint Type;

        /// <summary>
        /// 结果
        /// </summary>
        public uint Result;

        /// <summary>
        /// 管理页面URl
        /// </summary>
        public string Url;

        public void Write(IBuffer buffer)
        {
            buffer.WriteUInt32(Type, Endianness.Big);
            buffer.WriteUInt32(Result, Endianness.Big);
            buffer.WriteInt32(Url.Length + 1, Endianness.Big);
            buffer.WriteCString(Url);
        }

        public void Read(IBuffer buffer)
        {
            Type = buffer.ReadUInt32(Endianness.Big);
            Result = buffer.ReadUInt32(Endianness.Big);
            int UrlEntryLen = buffer.ReadInt32(Endianness.Big);
            Url = buffer.ReadString(UrlEntryLen);
        }

    }
}
