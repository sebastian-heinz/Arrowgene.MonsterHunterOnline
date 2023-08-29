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
    /// 物体绑定同步
    /// </summary>
    public class CSEntityAttach : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSEntityAttach));

        public CSEntityAttach()
        {
            ParentEntNetId = 0;
            ChildEntNetId = 0;
            AttachmentName = "";
            BoneName = "";
            Offset = new CSVec3();
            Rotation = new CSVec3();
            Attach = 0;
        }

        /// <summary>
        /// 绑定的父entity
        /// </summary>
        public uint ParentEntNetId;

        /// <summary>
        /// 绑定的child entity
        /// </summary>
        public uint ChildEntNetId;

        /// <summary>
        /// 绑定Attachment名字
        /// </summary>
        public string AttachmentName;

        /// <summary>
        /// 绑定骨骼点名字
        /// </summary>
        public string BoneName;

        /// <summary>
        /// 绑定点偏移值
        /// </summary>
        public CSVec3 Offset;

        /// <summary>
        /// 绑定点旋转值
        /// </summary>
        public CSVec3 Rotation;

        /// <summary>
        /// 是否绑定还是解除绑定
        /// </summary>
        public byte Attach;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteUInt32(ParentEntNetId, Endianness.Big);
            buffer.WriteUInt32(ChildEntNetId, Endianness.Big);
            buffer.WriteInt32(AttachmentName.Length + 1, Endianness.Big);
            buffer.WriteCString(AttachmentName);
            buffer.WriteInt32(BoneName.Length + 1, Endianness.Big);
            buffer.WriteCString(BoneName);
            Offset.WriteCs(buffer);
            Rotation.WriteCs(buffer);
            buffer.WriteByte(Attach);
        }

        public void ReadCs(IBuffer buffer)
        {
            ParentEntNetId = buffer.ReadUInt32(Endianness.Big);
            ChildEntNetId = buffer.ReadUInt32(Endianness.Big);
            int AttachmentNameEntryLen = buffer.ReadInt32(Endianness.Big);
            AttachmentName = buffer.ReadString(AttachmentNameEntryLen);
            int BoneNameEntryLen = buffer.ReadInt32(Endianness.Big);
            BoneName = buffer.ReadString(BoneNameEntryLen);
            Offset.ReadCs(buffer);
            Rotation.ReadCs(buffer);
            Attach = buffer.ReadByte();
        }

    }
}
