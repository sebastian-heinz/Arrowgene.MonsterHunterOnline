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
    /// 新手引导通知
    /// </summary>
    public class CSGuideNotify : CSPlayerExtNotifyData
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CSGuideNotify));

        public CSGuideNotify(CSGuideNotifyData __Notify)
        {
            _Notify = __Notify;
        }

        public CS_PLAYER_EXT_TYPE ExtType => CS_PLAYER_EXT_TYPE.CS_PLAYER_EXT_GUIDE;

        /// <summary>
        /// 交互通知类型
        /// </summary>

        /// <summary>
        /// 通知数据
        /// </summary>
        public CSGuideNotifyData _Notify;

        public void WriteCs(IBuffer buffer)
        {
            buffer.WriteInt32((int)_Notify.NotifyType, Endianness.Big);
            _Notify.WriteCs(buffer);
        }

        public void ReadCs(IBuffer buffer)
        {
            GUIDE_NOTIFY_TYPE CSGuideNotifyData_NotifyType = (GUIDE_NOTIFY_TYPE)buffer.ReadInt32(Endianness.Big);
            switch (CSGuideNotifyData_NotifyType)
            {
                case GUIDE_NOTIFY_TYPE.GUIDE_NOTIFY_TYPE_SET_STEP:
                    _Notify = new CSGuideStep();
                    break;
            }
            if (_Notify != null) {
                _Notify.ReadCs(buffer);
            }
            else {
                Logger.Error("Failed to create '_Notify' instance of type 'CSGuideNotifyData'");
            }
        }

    }
}
