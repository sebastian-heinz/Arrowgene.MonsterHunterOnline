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
    /// 敏感操作验证结果
    /// </summary>
    public class CSSensitiveVerifyResult : IStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSSensitiveVerifyResult));

        public CSSensitiveVerifyResult()
        {
            Result = 0;
            Title = new List<byte>();
            Tips = new List<byte>();
        }

        /// <summary>
        /// 服务器下发敏感操作结果
        /// </summary>
        public uint Result;

        /// <summary>
        /// 标题
        /// </summary>
        public List<byte> Title;

        /// <summary>
        /// 提示信息
        /// </summary>
        public List<byte> Tips;

        public void Write(IBuffer buffer)
        {
            buffer.WriteUInt32(Result, Endianness.Big);
            uint titleCount = (uint)Title.Count;
            buffer.WriteUInt32(titleCount, Endianness.Big);
            for (int i = 0; i < titleCount; i++)
            {
                buffer.WriteByte(Title[i]);
            }
            uint tipsCount = (uint)Tips.Count;
            buffer.WriteUInt32(tipsCount, Endianness.Big);
            for (int i = 0; i < tipsCount; i++)
            {
                buffer.WriteByte(Tips[i]);
            }
        }

        public void Read(IBuffer buffer)
        {
            Result = buffer.ReadUInt32(Endianness.Big);
            Title.Clear();
            uint titleCount = buffer.ReadUInt32(Endianness.Big);
            for (int i = 0; i < titleCount; i++)
            {
                byte TitleEntry = buffer.ReadByte();
                Title.Add(TitleEntry);
            }
            Tips.Clear();
            uint tipsCount = buffer.ReadUInt32(Endianness.Big);
            for (int i = 0; i < tipsCount; i++)
            {
                byte TipsEntry = buffer.ReadByte();
                Tips.Add(TipsEntry);
            }
        }

    }
}
