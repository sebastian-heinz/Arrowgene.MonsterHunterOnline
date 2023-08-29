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
    /// 动态伤害结构
    /// </summary>
    public class CSDMGParam : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSDMGParam));

        public CSDMGParam()
        {
            Param = new List<byte>();
        }

        /// <summary>
        /// 参数数据
        /// </summary>
        public List<byte> Param;

        public void WriteCs(IBuffer buffer)
        {
            int paramCount = (int)Param.Count;
            buffer.WriteInt32(paramCount, Endianness.Big);
            for (int i = 0; i < paramCount; i++)
            {
                buffer.WriteByte(Param[i]);
            }
        }

        public void ReadCs(IBuffer buffer)
        {
            Param.Clear();
            int paramCount = buffer.ReadInt32(Endianness.Big);
            for (int i = 0; i < paramCount; i++)
            {
                byte ParamEntry = buffer.ReadByte();
                Param.Add(ParamEntry);
            }
        }

    }
}
