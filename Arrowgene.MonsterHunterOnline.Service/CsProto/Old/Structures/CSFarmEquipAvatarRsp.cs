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

    public class CSFarmEquipAvatarRsp : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSFarmEquipAvatarRsp));

        public CSFarmEquipAvatarRsp()
        {
            WoodNetID = 0;
            WoodIndexID = 0;
            EquipData = new List<byte>();
            Arg1 = 0;
            Arg2 = 0;
            Arg3 = 0;
            Arg4 = 0;
            Arg5 = 0;
            Arg6 = 0;
        }

        /// <summary>
        /// 武器架EntityID
        /// </summary>
        public int WoodNetID;

        /// <summary>
        /// 武器架位置Index
        /// </summary>
        public int WoodIndexID;

        /// <summary>
        /// 装备数据
        /// </summary>
        public List<byte> EquipData;

        /// <summary>
        /// 参数
        /// </summary>
        public int Arg1;

        /// <summary>
        /// 参数
        /// </summary>
        public int Arg2;

        /// <summary>
        /// 参数
        /// </summary>
        public int Arg3;

        /// <summary>
        /// 参数
        /// </summary>
        public int Arg4;

        /// <summary>
        /// 参数
        /// </summary>
        public int Arg5;

        /// <summary>
        /// 参数
        /// </summary>
        public int Arg6;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt32(WoodNetID, Endianness.Big);
            buffer.WriteInt32(WoodIndexID, Endianness.Big);
            int equipDataCount = (int)EquipData.Count;
            buffer.WriteInt32(equipDataCount, Endianness.Big);
            for (int i = 0; i < equipDataCount; i++)
            {
                buffer.WriteByte(EquipData[i]);
            }
            buffer.WriteInt32(Arg1, Endianness.Big);
            buffer.WriteInt32(Arg2, Endianness.Big);
            buffer.WriteInt32(Arg3, Endianness.Big);
            buffer.WriteInt32(Arg4, Endianness.Big);
            buffer.WriteInt32(Arg5, Endianness.Big);
            buffer.WriteInt32(Arg6, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            WoodNetID = buffer.ReadInt32(Endianness.Big);
            WoodIndexID = buffer.ReadInt32(Endianness.Big);
            EquipData.Clear();
            int equipDataCount = buffer.ReadInt32(Endianness.Big);
            for (int i = 0; i < equipDataCount; i++)
            {
                byte EquipDataEntry = buffer.ReadByte();
                EquipData.Add(EquipDataEntry);
            }
            Arg1 = buffer.ReadInt32(Endianness.Big);
            Arg2 = buffer.ReadInt32(Endianness.Big);
            Arg3 = buffer.ReadInt32(Endianness.Big);
            Arg4 = buffer.ReadInt32(Endianness.Big);
            Arg5 = buffer.ReadInt32(Endianness.Big);
            Arg6 = buffer.ReadInt32(Endianness.Big);
        }

    }
}
