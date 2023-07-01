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
    /// 战队队员
    /// </summary>
    public class CSClaner : IStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSClaner));

        public CSClaner()
        {
            Id = new CSRole();
            Note = "";
            Level = 0;
            Line = 0;
            IsOnline = 0;
            OfflineTimeInterval = 0;
            Title = 0;
            Score = 0;
            RewardTag = 0;
            HRLevel = 0;
        }

        /// <summary>
        /// 标识
        /// </summary>
        public CSRole Id;

        /// <summary>
        /// 注释
        /// </summary>
        public string Note;

        /// <summary>
        /// 等级
        /// </summary>
        public int Level;

        /// <summary>
        /// 当前所在线
        /// </summary>
        public int Line;

        /// <summary>
        /// 在线?
        /// </summary>
        public byte IsOnline;

        /// <summary>
        /// 离线时长
        /// </summary>
        public int OfflineTimeInterval;

        /// <summary>
        /// 头衔
        /// </summary>
        public int Title;

        /// <summary>
        /// 个人积分
        /// </summary>
        public int Score;

        /// <summary>
        /// 可领取标识
        /// </summary>
        public int RewardTag;

        /// <summary>
        /// HR等级
        /// </summary>
        public int HRLevel;

        public void Write(IBuffer buffer)
        {
            Id.Write(buffer);
            buffer.WriteInt32(Note.Length + 1, Endianness.Big);
            buffer.WriteCString(Note);
            buffer.WriteInt32(Level, Endianness.Big);
            buffer.WriteInt32(Line, Endianness.Big);
            buffer.WriteByte(IsOnline);
            buffer.WriteInt32(OfflineTimeInterval, Endianness.Big);
            buffer.WriteInt32(Title, Endianness.Big);
            buffer.WriteInt32(Score, Endianness.Big);
            buffer.WriteInt32(RewardTag, Endianness.Big);
            buffer.WriteInt32(HRLevel, Endianness.Big);
        }

        public void Read(IBuffer buffer)
        {
            Id.Read(buffer);
            int NoteEntryLen = buffer.ReadInt32(Endianness.Big);
            Note = buffer.ReadString(NoteEntryLen);
            Level = buffer.ReadInt32(Endianness.Big);
            Line = buffer.ReadInt32(Endianness.Big);
            IsOnline = buffer.ReadByte();
            OfflineTimeInterval = buffer.ReadInt32(Endianness.Big);
            Title = buffer.ReadInt32(Endianness.Big);
            Score = buffer.ReadInt32(Endianness.Big);
            RewardTag = buffer.ReadInt32(Endianness.Big);
            HRLevel = buffer.ReadInt32(Endianness.Big);
        }

    }
}
