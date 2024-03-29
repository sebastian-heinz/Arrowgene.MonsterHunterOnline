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
    /// 飞行道具发射
    /// </summary>
    public class CSProjectileLaunchNtf : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSProjectileLaunchNtf));

        public CSProjectileLaunchNtf()
        {
            SyncTime = 0;
            NetID = 0;
            LauncherID = 0;
            VehicleID = 0;
            TypeID = 0;
            pos = new CSVec3();
            dir = new CSVec3();
            additiveVel = new CSVec3();
            skillId = 0;
            itemId = 0;
            delay = 0.0f;
            speedScale = 0.0f;
            damageScale = 0.0f;
            overrideTrail = 0;
            acc = new CSVec3();
            vel = new CSVec3();
            radius = 0.0f;
            gravityChangeTime = 0.0f;
            additiveGravity = 0.0f;
            launchType = 0;
            additiveAccXYZMode = 0;
            additiveAccXYZ = new List<CSVec3>();
            additiveAccTime = new List<float>();
        }

        /// <summary>
        /// 同步时间
        /// </summary>
        public long SyncTime;

        /// <summary>
        /// 飞行道具id
        /// </summary>
        public int NetID;

        /// <summary>
        /// 释放飞行道具者id
        /// </summary>
        public int LauncherID;

        /// <summary>
        /// 用于发射的载具id
        /// </summary>
        public uint VehicleID;

        /// <summary>
        /// 飞行道具类型id
        /// </summary>
        public int TypeID;

        /// <summary>
        /// pose init
        /// </summary>
        public CSVec3 pos;

        /// <summary>
        /// direction init
        /// </summary>
        public CSVec3 dir;

        /// <summary>
        /// 外界附加速度
        /// </summary>
        public CSVec3 additiveVel;

        /// <summary>
        /// 发射技能索引
        /// </summary>
        public int skillId;

        /// <summary>
        /// 生成时使用的道具Id（如果有）
        /// </summary>
        public int itemId;

        /// <summary>
        /// the time between spawn and launch
        /// </summary>
        public float delay;

        /// <summary>
        /// speed scale on static data speed
        /// </summary>
        public float speedScale;

        /// <summary>
        /// damage scale to calculate damage
        /// </summary>
        public float damageScale;

        /// <summary>
        /// bool变量，是否用以下参数覆盖excel中对应参数
        /// </summary>
        public int overrideTrail;

        /// <summary>
        /// world加速度（未附加z轴旋转）
        /// </summary>
        public CSVec3 acc;

        /// <summary>
        /// 初速度
        /// </summary>
        public CSVec3 vel;

        /// <summary>
        /// 判定半径大小
        /// </summary>
        public float radius;

        /// <summary>
        /// 重力从该时刻后改变（仅弓用）
        /// </summary>
        public float gravityChangeTime;

        /// <summary>
        /// 重力改变附加值
        /// </summary>
        public float additiveGravity;

        /// <summary>
        /// 发射方式（用于验证和同步，详见ProjLaunchParams.h）
        /// </summary>
        public int launchType;

        /// <summary>
        /// bool变量，是否用XYZ轴的额外加速度
        /// </summary>
        public int additiveAccXYZMode;

        public List<CSVec3> additiveAccXYZ;

        public List<float> additiveAccTime;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt64(SyncTime, Endianness.Big);
            buffer.WriteInt32(NetID, Endianness.Big);
            buffer.WriteInt32(LauncherID, Endianness.Big);
            buffer.WriteUInt32(VehicleID, Endianness.Big);
            buffer.WriteInt32(TypeID, Endianness.Big);
            pos.WriteCs(buffer);
            dir.WriteCs(buffer);
            additiveVel.WriteCs(buffer);
            buffer.WriteInt32(skillId, Endianness.Big);
            buffer.WriteInt32(itemId, Endianness.Big);
            buffer.WriteFloat(delay, Endianness.Big);
            buffer.WriteFloat(speedScale, Endianness.Big);
            buffer.WriteFloat(damageScale, Endianness.Big);
            buffer.WriteInt32(overrideTrail, Endianness.Big);
            acc.WriteCs(buffer);
            vel.WriteCs(buffer);
            buffer.WriteFloat(radius, Endianness.Big);
            buffer.WriteFloat(gravityChangeTime, Endianness.Big);
            buffer.WriteFloat(additiveGravity, Endianness.Big);
            buffer.WriteInt32(launchType, Endianness.Big);
            buffer.WriteInt32(additiveAccXYZMode, Endianness.Big);
            int additiveAccXYZCount = (int)additiveAccXYZ.Count;
            buffer.WriteInt32(additiveAccXYZCount, Endianness.Big);
            for (int i = 0; i < additiveAccXYZCount; i++)
            {
                additiveAccXYZ[i].WriteCs(buffer);
            }
            int additiveAccTimeCount = (int)additiveAccTime.Count;
            buffer.WriteInt32(additiveAccTimeCount, Endianness.Big);
            for (int i = 0; i < additiveAccTimeCount; i++)
            {
                buffer.WriteFloat(additiveAccTime[i], Endianness.Big);
            }
        }

        public void ReadCs(IBuffer buffer)
        {
            SyncTime = buffer.ReadInt64(Endianness.Big);
            NetID = buffer.ReadInt32(Endianness.Big);
            LauncherID = buffer.ReadInt32(Endianness.Big);
            VehicleID = buffer.ReadUInt32(Endianness.Big);
            TypeID = buffer.ReadInt32(Endianness.Big);
            pos.ReadCs(buffer);
            dir.ReadCs(buffer);
            additiveVel.ReadCs(buffer);
            skillId = buffer.ReadInt32(Endianness.Big);
            itemId = buffer.ReadInt32(Endianness.Big);
            delay = buffer.ReadFloat(Endianness.Big);
            speedScale = buffer.ReadFloat(Endianness.Big);
            damageScale = buffer.ReadFloat(Endianness.Big);
            overrideTrail = buffer.ReadInt32(Endianness.Big);
            acc.ReadCs(buffer);
            vel.ReadCs(buffer);
            radius = buffer.ReadFloat(Endianness.Big);
            gravityChangeTime = buffer.ReadFloat(Endianness.Big);
            additiveGravity = buffer.ReadFloat(Endianness.Big);
            launchType = buffer.ReadInt32(Endianness.Big);
            additiveAccXYZMode = buffer.ReadInt32(Endianness.Big);
            additiveAccXYZ.Clear();
            int additiveAccXYZCount = buffer.ReadInt32(Endianness.Big);
            for (int i = 0; i < additiveAccXYZCount; i++)
            {
                CSVec3 additiveAccXYZEntry = new CSVec3();
                additiveAccXYZEntry.ReadCs(buffer);
                additiveAccXYZ.Add(additiveAccXYZEntry);
            }
            additiveAccTime.Clear();
            int additiveAccTimeCount = buffer.ReadInt32(Endianness.Big);
            for (int i = 0; i < additiveAccTimeCount; i++)
            {
                float additiveAccTimeEntry = buffer.ReadFloat(Endianness.Big);
                additiveAccTime.Add(additiveAccTimeEntry);
            }
        }

    }
}
