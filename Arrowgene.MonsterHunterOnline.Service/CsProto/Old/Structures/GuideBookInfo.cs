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
    /// 玩家新手引导书信息 与csproto.xml保持一致
    /// </summary>
    public class GuideBookInfo : ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(GuideBookInfo));

        public GuideBookInfo()
        {
            GuideBookChapterInfos = new List<GuideBookChapterInfo>();
            IsFisrtAutoOpenGuideBook = 0;
            WeaopnId = 0;
            GuideActionInfos = new GuideActionInfo();
        }

        /// <summary>
        /// 完成集
        /// </summary>
        public List<GuideBookChapterInfo> GuideBookChapterInfos;

        /// <summary>
        /// 是否第一次自动打开引导书
        /// </summary>
        public byte IsFisrtAutoOpenGuideBook;

        /// <summary>
        /// 选择的武器ID
        /// </summary>
        public byte WeaopnId;

        /// <summary>
        /// 行为完成信息
        /// </summary>
        public GuideActionInfo GuideActionInfos;

        public void WriteCs(IBuffer buffer)
        {
            int guideBookChapterInfosCount = (int)GuideBookChapterInfos.Count;
            buffer.WriteInt32(guideBookChapterInfosCount, Endianness.Big);
            for (int i = 0; i < guideBookChapterInfosCount; i++)
            {
                GuideBookChapterInfos[i].WriteCs(buffer);
            }
            buffer.WriteByte(IsFisrtAutoOpenGuideBook);
            buffer.WriteByte(WeaopnId);
            GuideActionInfos.WriteCs(buffer);
        }

        public void ReadCs(IBuffer buffer)
        {
            GuideBookChapterInfos.Clear();
            int guideBookChapterInfosCount = buffer.ReadInt32(Endianness.Big);
            for (int i = 0; i < guideBookChapterInfosCount; i++)
            {
                GuideBookChapterInfo GuideBookChapterInfosEntry = new GuideBookChapterInfo();
                GuideBookChapterInfosEntry.ReadCs(buffer);
                GuideBookChapterInfos.Add(GuideBookChapterInfosEntry);
            }
            IsFisrtAutoOpenGuideBook = buffer.ReadByte();
            WeaopnId = buffer.ReadByte();
            GuideActionInfos.ReadCs(buffer);
        }

    }
}
