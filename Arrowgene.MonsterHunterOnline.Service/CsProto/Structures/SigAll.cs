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
    /// 原始签名，需要tconnd传过来的数据重组
    /// </summary>
    public class SigAll : IStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(SigAll));

        public SigAll()
        {
            SigVer = 0;
            SigTime = 0;
            SigBuff = new List<byte>();
        }

        /// <summary>
        /// 签名版本
        /// </summary>
        public short SigVer;

        /// <summary>
        /// 签名时间
        /// </summary>
        public uint SigTime;

        /// <summary>
        /// 签名密文
        /// </summary>
        public List<byte> SigBuff;

        public void Write(IBuffer buffer)
        {
            buffer.WriteInt16(SigVer, Endianness.Big);
            buffer.WriteUInt32(SigTime, Endianness.Big);
            short sigBuffCount = (short)SigBuff.Count;
            buffer.WriteInt16(sigBuffCount, Endianness.Big);
            for (int i = 0; i < sigBuffCount; i++)
            {
                buffer.WriteByte(SigBuff[i]);
            }
        }

        public void Read(IBuffer buffer)
        {
            SigVer = buffer.ReadInt16(Endianness.Big);
            SigTime = buffer.ReadUInt32(Endianness.Big);
            SigBuff.Clear();
            short sigBuffCount = buffer.ReadInt16(Endianness.Big);
            for (int i = 0; i < sigBuffCount; i++)
            {
                byte SigBuffEntry = buffer.ReadByte();
                SigBuff.Add(SigBuffEntry);
            }
        }

    }
}
