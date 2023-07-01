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
    /// 对固定速度、加速度的运动Entity位移做同步的消息
    /// </summary>
    public class CSBattleEntitySpeed : IStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSBattleEntitySpeed));

        public CSBattleEntitySpeed()
        {
            NetObjId = 0;
            IsStart = 0;
            InitSpeed = new CSVec3();
            Accelator = new CSVec3();
            InitAngleSpeed = 0.0f;
            AngleAccelator = 0.0f;
        }

        /// <summary>
        /// 需要同步的Actor的NetObjId
        /// </summary>
        public uint NetObjId;

        /// <summary>
        /// 标记运动是开始还是结束
        /// </summary>
        public byte IsStart;

        /// <summary>
        /// 初速度
        /// </summary>
        public CSVec3 InitSpeed;

        /// <summary>
        /// 加速度
        /// </summary>
        public CSVec3 Accelator;

        /// <summary>
        /// 角初速度
        /// </summary>
        public float InitAngleSpeed;

        /// <summary>
        /// 角加速度
        /// </summary>
        public float AngleAccelator;

        public void Write(IBuffer buffer)
        {
            buffer.WriteUInt32(NetObjId, Endianness.Big);
            buffer.WriteByte(IsStart);
            InitSpeed.Write(buffer);
            Accelator.Write(buffer);
            buffer.WriteFloat(InitAngleSpeed, Endianness.Big);
            buffer.WriteFloat(AngleAccelator, Endianness.Big);
        }

        public void Read(IBuffer buffer)
        {
            NetObjId = buffer.ReadUInt32(Endianness.Big);
            IsStart = buffer.ReadByte();
            InitSpeed.Read(buffer);
            Accelator.Read(buffer);
            InitAngleSpeed = buffer.ReadFloat(Endianness.Big);
            AngleAccelator = buffer.ReadFloat(Endianness.Big);
        }

    }
}
