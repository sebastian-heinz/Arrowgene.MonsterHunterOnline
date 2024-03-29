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
    /// 敏感操作验证URL通知
    /// </summary>
    public class CSSensitiveVerify : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSSensitiveVerify));

        public CSSensitiveVerify()
        {
            Result = 0;
            VerifyUrlLen = 0;
            VerifyUrl = "";
            TitleLen = 0;
            Title = "";
            TipsLen = 0;
            Tips = "";
        }

        /// <summary>
        /// 查询结果
        /// </summary>
        public byte Result;

        /// <summary>
        /// 验证url长度
        /// </summary>
        public uint VerifyUrlLen;

        /// <summary>
        /// 验证URL
        /// </summary>
        public string VerifyUrl;

        /// <summary>
        /// 标题长度
        /// </summary>
        public uint TitleLen;

        /// <summary>
        /// 标题
        /// </summary>
        public string Title;

        /// <summary>
        /// 提示信息长度
        /// </summary>
        public uint TipsLen;

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Tips;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteByte(Result);
            buffer.WriteUInt32(VerifyUrlLen, Endianness.Big);
            buffer.WriteInt32(VerifyUrl.Length + 1, Endianness.Big);
            buffer.WriteCString(VerifyUrl);
            buffer.WriteUInt32(TitleLen, Endianness.Big);
            buffer.WriteInt32(Title.Length + 1, Endianness.Big);
            buffer.WriteCString(Title);
            buffer.WriteUInt32(TipsLen, Endianness.Big);
            buffer.WriteInt32(Tips.Length + 1, Endianness.Big);
            buffer.WriteCString(Tips);
        }

        public void ReadCs(IBuffer buffer)
        {
            Result = buffer.ReadByte();
            VerifyUrlLen = buffer.ReadUInt32(Endianness.Big);
            int VerifyUrlEntryLen = buffer.ReadInt32(Endianness.Big);
            VerifyUrl = buffer.ReadString(VerifyUrlEntryLen);
            TitleLen = buffer.ReadUInt32(Endianness.Big);
            int TitleEntryLen = buffer.ReadInt32(Endianness.Big);
            Title = buffer.ReadString(TitleEntryLen);
            TipsLen = buffer.ReadUInt32(Endianness.Big);
            int TipsEntryLen = buffer.ReadInt32(Endianness.Big);
            Tips = buffer.ReadString(TipsEntryLen);
        }

    }
}
