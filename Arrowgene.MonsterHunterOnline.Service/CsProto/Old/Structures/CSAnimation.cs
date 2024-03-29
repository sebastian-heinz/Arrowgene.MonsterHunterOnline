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
    /// 动画播放信息
    /// </summary>
    public class CSAnimation : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSAnimation));

        public CSAnimation()
        {
            Parametric = new CSParametric();
            AnimCRC = 0;
            SegmentCounter = 0;
            AnimTime = 0.0f;
            TransitionWeight = 0.0f;
            Flags = 0;
        }

        /// <summary>
        /// LMG参数
        /// </summary>
        public CSParametric Parametric;

        /// <summary>
        /// 动画CRC
        /// </summary>
        public uint AnimCRC;

        /// <summary>
        /// 动画分段数
        /// </summary>
        public short SegmentCounter;

        /// <summary>
        /// 当前动画播放时间（%）
        /// </summary>
        public float AnimTime;

        /// <summary>
        /// 该动画播放权重（%）
        /// </summary>
        public float TransitionWeight;

        /// <summary>
        /// 动画播放标记，与CryCharAnimationParams相对应
        /// </summary>
        public uint Flags;

        public void WriteCs(IBuffer buffer)
        {
            Parametric.WriteCs(buffer);
            buffer.WriteUInt32(AnimCRC, Endianness.Big);
            buffer.WriteInt16(SegmentCounter, Endianness.Big);
            buffer.WriteFloat(AnimTime, Endianness.Big);
            buffer.WriteFloat(TransitionWeight, Endianness.Big);
            buffer.WriteUInt32(Flags, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            Parametric.ReadCs(buffer);
            AnimCRC = buffer.ReadUInt32(Endianness.Big);
            SegmentCounter = buffer.ReadInt16(Endianness.Big);
            AnimTime = buffer.ReadFloat(Endianness.Big);
            TransitionWeight = buffer.ReadFloat(Endianness.Big);
            Flags = buffer.ReadUInt32(Endianness.Big);
        }

    }
}
