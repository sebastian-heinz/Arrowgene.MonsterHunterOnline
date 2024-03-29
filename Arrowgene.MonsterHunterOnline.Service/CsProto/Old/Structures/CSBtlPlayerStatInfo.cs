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
    /// 结算节本统计信息
    /// </summary>
    public class CSBtlPlayerStatInfo : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSBtlPlayerStatInfo));

        public CSBtlPlayerStatInfo()
        {
            StatDataType = new List<int>();
            StatDataValue = new List<int>();
        }

        /// <summary>
        /// 关卡统计数据类型
        /// </summary>
        public List<int> StatDataType;

        /// <summary>
        /// 关卡统计数据值
        /// </summary>
        public List<int> StatDataValue;

        public void WriteCs(IBuffer buffer)
        {
            int statDataTypeCount = (int)StatDataType.Count;
            buffer.WriteInt32(statDataTypeCount, Endianness.Big);
            for (int i = 0; i < statDataTypeCount; i++)
            {
                buffer.WriteInt32(StatDataType[i], Endianness.Big);
            }
            int statDataValueCount = (int)StatDataValue.Count;
            buffer.WriteInt32(statDataValueCount, Endianness.Big);
            for (int i = 0; i < statDataValueCount; i++)
            {
                buffer.WriteInt32(StatDataValue[i], Endianness.Big);
            }
        }

        public void ReadCs(IBuffer buffer)
        {
            StatDataType.Clear();
            int statDataTypeCount = buffer.ReadInt32(Endianness.Big);
            for (int i = 0; i < statDataTypeCount; i++)
            {
                int StatDataTypeEntry = buffer.ReadInt32(Endianness.Big);
                StatDataType.Add(StatDataTypeEntry);
            }
            StatDataValue.Clear();
            int statDataValueCount = buffer.ReadInt32(Endianness.Big);
            for (int i = 0; i < statDataValueCount; i++)
            {
                int StatDataValueEntry = buffer.ReadInt32(Endianness.Big);
                StatDataValue.Add(StatDataValueEntry);
            }
        }

    }
}
