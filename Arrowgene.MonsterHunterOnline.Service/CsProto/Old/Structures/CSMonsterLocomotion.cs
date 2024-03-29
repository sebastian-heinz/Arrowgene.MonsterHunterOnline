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
    /// 怪物移动同步消息
    /// </summary>
    public class CSMonsterLocomotion : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSMonsterLocomotion));

        public CSMonsterLocomotion()
        {
            SteeringEnabled = 0;
            SyncTime = 0;
            MonsterID = 0;
            AnimSeqName = "";
            PartBoneName = "";
            SkillID = 0;
            SkillLv = 0;
            SyncFlag = 0;
            TargetID = 0;
            TargetSrvID = 0;
            Flag = 0;
            TargetDis = new CSVec3();
            MoveSpeed = new CSVec3();
            TargetRot = new CSVec3();
            RotSpeed = new CSVec3();
            RotSpeedByAnim = 0;
            MonsterPos = new CSVec3();
            MonsterRot = new CSQuat();
            SkillSpeed = 0.0f;
            RestartAnim = 0;
            RotFlag = 0;
            TargetMultiAttackPos = new List<CSVec3>();
            TargetAttackPos = new CSVec3();
            NeedTargetAttackPos = 0;
            AckFlag = 0;
            UserParam1 = 0;
            UserParam2 = 0;
            SetRotate = 0;
            SetPos = 0;
            NoTransferCorrection = 0;
            NeedMoveSpeedAcc = 0;
            MoveSpeedAccelerate = new CSVec3();
            MoveSpeedAccStart = 0.0f;
            MoveSpeedAccEnd = 0.0f;
            MoveSplineScale = new CSVec3();
        }

        /// <summary>
        /// 当前是否处于Steering系统控制状态
        /// </summary>
        public byte SteeringEnabled;

        /// <summary>
        /// 同步时间
        /// </summary>
        public long SyncTime;

        /// <summary>
        /// 怪物ID
        /// </summary>
        public uint MonsterID;

        /// <summary>
        /// 技能name
        /// </summary>
        public string AnimSeqName;

        /// <summary>
        /// 骨骼name
        /// </summary>
        public string PartBoneName;

        /// <summary>
        /// 技能id
        /// </summary>
        public int SkillID;

        /// <summary>
        /// 技能Lv
        /// </summary>
        public int SkillLv;

        /// <summary>
        /// 消息Flag
        /// </summary>
        public uint SyncFlag;

        /// <summary>
        /// 目标ID
        /// </summary>
        public uint TargetID;

        /// <summary>
        /// 目标SrvID
        /// </summary>
        public int TargetSrvID;

        /// <summary>
        /// 消息Flag
        /// </summary>
        public uint Flag;

        /// <summary>
        /// 目标距离
        /// </summary>
        public CSVec3 TargetDis;

        /// <summary>
        /// 移动速度
        /// </summary>
        public CSVec3 MoveSpeed;

        /// <summary>
        /// 目标方向
        /// </summary>
        public CSVec3 TargetRot;

        /// <summary>
        /// 旋转速度
        /// </summary>
        public CSVec3 RotSpeed;

        /// <summary>
        /// 与动画播放速度同步
        /// </summary>
        public byte RotSpeedByAnim;

        /// <summary>
        /// 怪物位置
        /// </summary>
        public CSVec3 MonsterPos;

        /// <summary>
        /// 怪物朝向
        /// </summary>
        public CSQuat MonsterRot;

        /// <summary>
        /// 技能速度
        /// </summary>
        public float SkillSpeed;

        /// <summary>
        /// 重新播放技能
        /// </summary>
        public byte RestartAnim;

        /// <summary>
        /// 旋转类型
        /// </summary>
        public int RotFlag;

        /// <summary>
        /// 攻击目标多个位置
        /// </summary>
        public List<CSVec3> TargetMultiAttackPos;

        /// <summary>
        /// 目标攻击坐标
        /// </summary>
        public CSVec3 TargetAttackPos;

        /// <summary>
        /// 需要追踪目标
        /// </summary>
        public byte NeedTargetAttackPos;

        /// <summary>
        /// Ack Flag
        /// </summary>
        public uint AckFlag;

        /// <summary>
        /// UserParam
        /// </summary>
        public int UserParam1;

        /// <summary>
        /// UserParam
        /// </summary>
        public int UserParam2;

        /// <summary>
        /// 重新设置朝向
        /// </summary>
        public byte SetRotate;

        /// <summary>
        /// 重新设置位置
        /// </summary>
        public byte SetPos;

        /// <summary>
        /// 是否需要Transfer修正
        /// </summary>
        public byte NoTransferCorrection;

        /// <summary>
        /// 是否需要移动加速
        /// </summary>
        public byte NeedMoveSpeedAcc;

        /// <summary>
        /// 移动加速度
        /// </summary>
        public CSVec3 MoveSpeedAccelerate;

        /// <summary>
        /// 移动加速度开始时间
        /// </summary>
        public float MoveSpeedAccStart;

        /// <summary>
        /// 移动加速度结束时间
        /// </summary>
        public float MoveSpeedAccEnd;

        /// <summary>
        /// 运动曲线缩放
        /// </summary>
        public CSVec3 MoveSplineScale;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteByte(SteeringEnabled);
            buffer.WriteInt64(SyncTime, Endianness.Big);
            buffer.WriteUInt32(MonsterID, Endianness.Big);
            buffer.WriteInt32(AnimSeqName.Length + 1, Endianness.Big);
            buffer.WriteCString(AnimSeqName);
            buffer.WriteInt32(PartBoneName.Length + 1, Endianness.Big);
            buffer.WriteCString(PartBoneName);
            buffer.WriteInt32(SkillID, Endianness.Big);
            buffer.WriteInt32(SkillLv, Endianness.Big);
            buffer.WriteUInt32(SyncFlag, Endianness.Big);
            buffer.WriteUInt32(TargetID, Endianness.Big);
            buffer.WriteInt32(TargetSrvID, Endianness.Big);
            buffer.WriteUInt32(Flag, Endianness.Big);
            TargetDis.WriteCs(buffer);
            MoveSpeed.WriteCs(buffer);
            TargetRot.WriteCs(buffer);
            RotSpeed.WriteCs(buffer);
            buffer.WriteByte(RotSpeedByAnim);
            MonsterPos.WriteCs(buffer);
            MonsterRot.WriteCs(buffer);
            buffer.WriteFloat(SkillSpeed, Endianness.Big);
            buffer.WriteByte(RestartAnim);
            buffer.WriteInt32(RotFlag, Endianness.Big);
            int targetMultiAttackPosCount = (int)TargetMultiAttackPos.Count;
            buffer.WriteInt32(targetMultiAttackPosCount, Endianness.Big);
            for (int i = 0; i < targetMultiAttackPosCount; i++)
            {
                TargetMultiAttackPos[i].WriteCs(buffer);
            }
            TargetAttackPos.WriteCs(buffer);
            buffer.WriteByte(NeedTargetAttackPos);
            buffer.WriteUInt32(AckFlag, Endianness.Big);
            buffer.WriteInt32(UserParam1, Endianness.Big);
            buffer.WriteInt32(UserParam2, Endianness.Big);
            buffer.WriteByte(SetRotate);
            buffer.WriteByte(SetPos);
            buffer.WriteByte(NoTransferCorrection);
            buffer.WriteByte(NeedMoveSpeedAcc);
            MoveSpeedAccelerate.WriteCs(buffer);
            buffer.WriteFloat(MoveSpeedAccStart, Endianness.Big);
            buffer.WriteFloat(MoveSpeedAccEnd, Endianness.Big);
            MoveSplineScale.WriteCs(buffer);
        }

        public void ReadCs(IBuffer buffer)
        {
            SteeringEnabled = buffer.ReadByte();
            SyncTime = buffer.ReadInt64(Endianness.Big);
            MonsterID = buffer.ReadUInt32(Endianness.Big);
            int AnimSeqNameEntryLen = buffer.ReadInt32(Endianness.Big);
            AnimSeqName = buffer.ReadString(AnimSeqNameEntryLen);
            int PartBoneNameEntryLen = buffer.ReadInt32(Endianness.Big);
            PartBoneName = buffer.ReadString(PartBoneNameEntryLen);
            SkillID = buffer.ReadInt32(Endianness.Big);
            SkillLv = buffer.ReadInt32(Endianness.Big);
            SyncFlag = buffer.ReadUInt32(Endianness.Big);
            TargetID = buffer.ReadUInt32(Endianness.Big);
            TargetSrvID = buffer.ReadInt32(Endianness.Big);
            Flag = buffer.ReadUInt32(Endianness.Big);
            TargetDis.ReadCs(buffer);
            MoveSpeed.ReadCs(buffer);
            TargetRot.ReadCs(buffer);
            RotSpeed.ReadCs(buffer);
            RotSpeedByAnim = buffer.ReadByte();
            MonsterPos.ReadCs(buffer);
            MonsterRot.ReadCs(buffer);
            SkillSpeed = buffer.ReadFloat(Endianness.Big);
            RestartAnim = buffer.ReadByte();
            RotFlag = buffer.ReadInt32(Endianness.Big);
            TargetMultiAttackPos.Clear();
            int targetMultiAttackPosCount = buffer.ReadInt32(Endianness.Big);
            for (int i = 0; i < targetMultiAttackPosCount; i++)
            {
                CSVec3 TargetMultiAttackPosEntry = new CSVec3();
                TargetMultiAttackPosEntry.ReadCs(buffer);
                TargetMultiAttackPos.Add(TargetMultiAttackPosEntry);
            }
            TargetAttackPos.ReadCs(buffer);
            NeedTargetAttackPos = buffer.ReadByte();
            AckFlag = buffer.ReadUInt32(Endianness.Big);
            UserParam1 = buffer.ReadInt32(Endianness.Big);
            UserParam2 = buffer.ReadInt32(Endianness.Big);
            SetRotate = buffer.ReadByte();
            SetPos = buffer.ReadByte();
            NoTransferCorrection = buffer.ReadByte();
            NeedMoveSpeedAcc = buffer.ReadByte();
            MoveSpeedAccelerate.ReadCs(buffer);
            MoveSpeedAccStart = buffer.ReadFloat(Endianness.Big);
            MoveSpeedAccEnd = buffer.ReadFloat(Endianness.Big);
            MoveSplineScale.ReadCs(buffer);
        }

    }
}
