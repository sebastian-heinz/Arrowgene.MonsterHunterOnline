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
    /// 角色基本信息
    /// </summary>
    public class CSRoleBaseInfo : IStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSRoleBaseInfo));

        public CSRoleBaseInfo()
        {
            RoleID = 0;
            RoleIndex = 0;
            Name = "";
            Gender = 0;
            Level = 0;
            RoleState = 0;
            RoleStateEndLeftTime = 0;
            AvatarSetID = 0;
            FaceId = 0;
            HairId = 0;
            UnderclothesId = 0;
            SkinColor = 0;
            HairColor = 0;
            InnerColor = 0;
            EyeBall = 0;
            EyeColor = 0;
            FaceTattooIndex = 0;
            FaceTattooColor = 0;
            Equip = new List<CSAvatarItem>();
            HideHelm = 0;
            HideFashion = 0;
            HideSuite = 0;
            FacialInfo = new short[CsProtoConstant.CS_MAX_FACIALINFO_COUNT];
            StarLevel = "";
            HRLevel = 0;
            SoulStoneLv = 0;
        }

        /// <summary>
        /// 角色ID
        /// </summary>
        public ulong RoleID;

        /// <summary>
        /// 角色Index
        /// </summary>
        public int RoleIndex;

        /// <summary>
        /// 角色名
        /// </summary>
        public string Name;

        /// <summary>
        /// 角色性别
        /// </summary>
        public byte Gender;

        /// <summary>
        /// 角色等级
        /// </summary>
        public int Level;

        /// <summary>
        /// 角色状态
        /// </summary>
        public int RoleState;

        /// <summary>
        /// 角色状态结束的剩余时间
        /// </summary>
        public uint RoleStateEndLeftTime;

        /// <summary>
        /// Avatar Set
        /// </summary>
        public byte AvatarSetID;

        /// <summary>
        /// 脸型
        /// </summary>
        public ushort FaceId;

        /// <summary>
        /// 发型
        /// </summary>
        public ushort HairId;

        /// <summary>
        /// 内衣
        /// </summary>
        public ushort UnderclothesId;

        /// <summary>
        /// 皮肤颜色
        /// </summary>
        public int SkinColor;

        /// <summary>
        /// 头发颜色
        /// </summary>
        public int HairColor;

        /// <summary>
        /// 内衣颜色
        /// </summary>
        public int InnerColor;

        /// <summary>
        /// 眼睛
        /// </summary>
        public int EyeBall;

        /// <summary>
        /// 眼睛颜色
        /// </summary>
        public int EyeColor;

        /// <summary>
        /// 脸部贴花
        /// </summary>
        public int FaceTattooIndex;

        /// <summary>
        /// 脸部贴花颜色
        /// </summary>
        public int FaceTattooColor;

        /// <summary>
        /// 装备物品
        /// </summary>
        public List<CSAvatarItem> Equip;

        /// <summary>
        /// 是否隐藏头盔
        /// </summary>
        public byte HideHelm;

        /// <summary>
        /// 是否隐藏时装
        /// </summary>
        public byte HideFashion;

        /// <summary>
        /// 是否隐藏套件
        /// </summary>
        public byte HideSuite;

        /// <summary>
        /// 捏脸数据集合
        /// </summary>
        public short[] FacialInfo;

        /// <summary>
        /// 星级
        /// </summary>
        public string StarLevel;

        /// <summary>
        /// 角色HR等级
        /// </summary>
        public int HRLevel;

        /// <summary>
        /// 兽魂石等级
        /// </summary>
        public int SoulStoneLv;

        public void Write(IBuffer buffer)
        {
            buffer.WriteUInt64(RoleID, Endianness.Big);
            buffer.WriteInt32(RoleIndex, Endianness.Big);
            buffer.WriteInt32(Name.Length + 1, Endianness.Big);
            buffer.WriteCString(Name);
            buffer.WriteByte(Gender);
            buffer.WriteInt32(Level, Endianness.Big);
            buffer.WriteInt32(RoleState, Endianness.Big);
            buffer.WriteUInt32(RoleStateEndLeftTime, Endianness.Big);
            buffer.WriteByte(AvatarSetID);
            buffer.WriteUInt16(FaceId, Endianness.Big);
            buffer.WriteUInt16(HairId, Endianness.Big);
            buffer.WriteUInt16(UnderclothesId, Endianness.Big);
            buffer.WriteInt32(SkinColor, Endianness.Big);
            buffer.WriteInt32(HairColor, Endianness.Big);
            buffer.WriteInt32(InnerColor, Endianness.Big);
            buffer.WriteInt32(EyeBall, Endianness.Big);
            buffer.WriteInt32(EyeColor, Endianness.Big);
            buffer.WriteInt32(FaceTattooIndex, Endianness.Big);
            buffer.WriteInt32(FaceTattooColor, Endianness.Big);
            ushort equipCount = (ushort)Equip.Count;
            buffer.WriteUInt16(equipCount, Endianness.Big);
            for (int i = 0; i < equipCount; i++)
            {
                Equip[i].Write(buffer);
            }
            buffer.WriteByte(HideHelm);
            buffer.WriteByte(HideFashion);
            buffer.WriteByte(HideSuite);
            for (int i = 0; i < CsProtoConstant.CS_MAX_FACIALINFO_COUNT; i++)
            {
                buffer.WriteInt16(FacialInfo[i], Endianness.Big);
            }
            buffer.WriteInt32(StarLevel.Length + 1, Endianness.Big);
            buffer.WriteCString(StarLevel);
            buffer.WriteInt32(HRLevel, Endianness.Big);
            buffer.WriteInt32(SoulStoneLv, Endianness.Big);
        }

        public void Read(IBuffer buffer)
        {
            RoleID = buffer.ReadUInt64(Endianness.Big);
            RoleIndex = buffer.ReadInt32(Endianness.Big);
            int NameEntryLen = buffer.ReadInt32(Endianness.Big);
            Name = buffer.ReadString(NameEntryLen);
            Gender = buffer.ReadByte();
            Level = buffer.ReadInt32(Endianness.Big);
            RoleState = buffer.ReadInt32(Endianness.Big);
            RoleStateEndLeftTime = buffer.ReadUInt32(Endianness.Big);
            AvatarSetID = buffer.ReadByte();
            FaceId = buffer.ReadUInt16(Endianness.Big);
            HairId = buffer.ReadUInt16(Endianness.Big);
            UnderclothesId = buffer.ReadUInt16(Endianness.Big);
            SkinColor = buffer.ReadInt32(Endianness.Big);
            HairColor = buffer.ReadInt32(Endianness.Big);
            InnerColor = buffer.ReadInt32(Endianness.Big);
            EyeBall = buffer.ReadInt32(Endianness.Big);
            EyeColor = buffer.ReadInt32(Endianness.Big);
            FaceTattooIndex = buffer.ReadInt32(Endianness.Big);
            FaceTattooColor = buffer.ReadInt32(Endianness.Big);
            Equip.Clear();
            ushort equipCount = buffer.ReadUInt16(Endianness.Big);
            for (int i = 0; i < equipCount; i++)
            {
                CSAvatarItem EquipEntry = new CSAvatarItem();
                EquipEntry.Read(buffer);
                Equip.Add(EquipEntry);
            }
            HideHelm = buffer.ReadByte();
            HideFashion = buffer.ReadByte();
            HideSuite = buffer.ReadByte();
            for (int i = 0; i < CsProtoConstant.CS_MAX_FACIALINFO_COUNT; i++)
            {
                FacialInfo[i] = buffer.ReadInt16(Endianness.Big);
            }
            int StarLevelEntryLen = buffer.ReadInt32(Endianness.Big);
            StarLevel = buffer.ReadString(StarLevelEntryLen);
            HRLevel = buffer.ReadInt32(Endianness.Big);
            SoulStoneLv = buffer.ReadInt32(Endianness.Big);
        }

    }
}
