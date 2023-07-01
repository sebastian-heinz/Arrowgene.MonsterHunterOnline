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

    public class ScriptActivityDataLevelControl : ScriptActivityDataUnion
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(ScriptActivityDataLevelControl));

        public ScriptActivityDataLevelControl()
        {
            ControlType = 0;
            Id = 0;
            DateType = 0;
            DateInterval = 0;
            DateDays = "";
            DateTime = "";
            BeginTime = 0;
            Dates = new List<ScriptActivityDate>();
        }

        public EScriptActivityDataType Type => EScriptActivityDataType.kScriptActivityDataType_LevelControl;

        /// <summary>
        /// 控制对象类型
        /// </summary>
        public byte ControlType;

        /// <summary>
        /// 控制对象
        /// </summary>
        public int Id;

        /// <summary>
        /// 控制对象时间类型 @EOperationActivityPeriodType
        /// </summary>
        public int DateType;

        /// <summary>
        /// 时间间隔
        /// </summary>
        public int DateInterval;

        /// <summary>
        /// 每周开放时间（1~7）或者每月开放时间（1~31）
        /// </summary>
        public string DateDays;

        /// <summary>
        /// 每日开放时间段格式：HH:mm~HH:mm 没有就空
        /// </summary>
        public string DateTime;

        /// <summary>
        /// 开始时间
        /// </summary>
        public uint BeginTime;

        /// <summary>
        /// 起止时间集
        /// </summary>
        public List<ScriptActivityDate> Dates;

        public void Write(IBuffer buffer)
        {
            buffer.WriteByte(ControlType);
            buffer.WriteInt32(Id, Endianness.Big);
            buffer.WriteInt32(DateType, Endianness.Big);
            buffer.WriteInt32(DateInterval, Endianness.Big);
            buffer.WriteInt32(DateDays.Length + 1, Endianness.Big);
            buffer.WriteCString(DateDays);
            buffer.WriteInt32(DateTime.Length + 1, Endianness.Big);
            buffer.WriteCString(DateTime);
            buffer.WriteUInt32(BeginTime, Endianness.Big);
            byte datesCount = (byte)Dates.Count;
            buffer.WriteByte(datesCount);
            for (int i = 0; i < datesCount; i++)
            {
                Dates[i].Write(buffer);
            }
        }

        public void Read(IBuffer buffer)
        {
            ControlType = buffer.ReadByte();
            Id = buffer.ReadInt32(Endianness.Big);
            DateType = buffer.ReadInt32(Endianness.Big);
            DateInterval = buffer.ReadInt32(Endianness.Big);
            int DateDaysEntryLen = buffer.ReadInt32(Endianness.Big);
            DateDays = buffer.ReadString(DateDaysEntryLen);
            int DateTimeEntryLen = buffer.ReadInt32(Endianness.Big);
            DateTime = buffer.ReadString(DateTimeEntryLen);
            BeginTime = buffer.ReadUInt32(Endianness.Big);
            Dates.Clear();
            byte datesCount = buffer.ReadByte();
            for (int i = 0; i < datesCount; i++)
            {
                ScriptActivityDate DatesEntry = new ScriptActivityDate();
                DatesEntry.Read(buffer);
                Dates.Add(DatesEntry);
            }
        }

    }
}
