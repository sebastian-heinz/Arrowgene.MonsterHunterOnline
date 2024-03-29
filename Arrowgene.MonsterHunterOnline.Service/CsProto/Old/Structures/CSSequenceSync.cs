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
    /// Trackview状态同步
    /// </summary>
    public class CSSequenceSync : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSSequenceSync));

        public CSSequenceSync()
        {
            LogicEntity = 0;
            SeqName = "";
            time = 0.0f;
            State = 0;
            Flag = 0;
        }

        /// <summary>
        /// 对应的SeqctrlObj的logicentityID
        /// </summary>
        public uint LogicEntity;

        /// <summary>
        /// Sequence名称
        /// </summary>
        public string SeqName;

        /// <summary>
        /// 当前时间
        /// </summary>
        public float time;

        /// <summary>
        /// Sequence状态
        /// </summary>
        public int State;

        /// <summary>
        /// Sequence flag
        /// </summary>
        public int Flag;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteUInt32(LogicEntity, Endianness.Big);
            buffer.WriteInt32(SeqName.Length + 1, Endianness.Big);
            buffer.WriteCString(SeqName);
            buffer.WriteFloat(time, Endianness.Big);
            buffer.WriteInt32(State, Endianness.Big);
            buffer.WriteInt32(Flag, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            LogicEntity = buffer.ReadUInt32(Endianness.Big);
            int SeqNameEntryLen = buffer.ReadInt32(Endianness.Big);
            SeqName = buffer.ReadString(SeqNameEntryLen);
            time = buffer.ReadFloat(Endianness.Big);
            State = buffer.ReadInt32(Endianness.Big);
            Flag = buffer.ReadInt32(Endianness.Big);
        }

    }
}
