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

    public class CSRotateAxisParam : CSObjectActionParam
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSRotateAxisParam));

        public CSRotateAxisParam()
        {
            axis = new CSVec3();
            rotateAngle = 0.0f;
            speed = 0.0f;
            acceleration = 0.0f;
        }

        public CS_SCENE_OBJECT_ACTION_TYPE Action => CS_SCENE_OBJECT_ACTION_TYPE.CS_ACTION_TYPE_ROTATEAXIS;

        /// <summary>
        /// 旋转轴
        /// </summary>
        public CSVec3 axis;

        /// <summary>
        /// 旋转角度（角度制）
        /// </summary>
        public float rotateAngle;

        /// <summary>
        /// 旋转速度（角度制）
        /// </summary>
        public float speed;

        /// <summary>
        /// 旋转加速度（角度制）
        /// </summary>
        public float acceleration;

        public void WriteCs(IBuffer buffer)
        {
            axis.WriteCs(buffer);
            buffer.WriteFloat(rotateAngle, Endianness.Big);
            buffer.WriteFloat(speed, Endianness.Big);
            buffer.WriteFloat(acceleration, Endianness.Big);
        }

        public void ReadCs(IBuffer buffer)
        {
            axis.ReadCs(buffer);
            rotateAngle = buffer.ReadFloat(Endianness.Big);
            speed = buffer.ReadFloat(Endianness.Big);
            acceleration = buffer.ReadFloat(Endianness.Big);
        }

    }
}
