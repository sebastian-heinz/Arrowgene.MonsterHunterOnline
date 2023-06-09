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

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Enums
{
    public enum CS_SCENE_OBJECT_ACTION_TYPE
    {
        /// <summary>
        /// 播放特效
        /// </summary>
        CS_ACTION_TYPE_PLAY_EFFECT = 0,

        /// <summary>
        /// 播放动画
        /// </summary>
        CS_ACTION_TYPE_PLAY_ANIMATION = 1,

        /// <summary>
        /// 状态机切换
        /// </summary>
        CS_ACTION_TYPE_GOTOSTATE = 2,

        /// <summary>
        /// 移动到目标点
        /// </summary>
        CS_ACTION_TYPE_GOTO_TARGETPOS = 3,

        /// <summary>
        /// 绕旋转轴转动一定角度
        /// </summary>
        CS_ACTION_TYPE_ROTATEAXIS = 4,

        /// <summary>
        /// 传送到目标点
        /// </summary>
        CS_ACTION_TYPE_TELEPORT = 5,

        /// <summary>
        /// 基础行为，如隐藏，enable等
        /// </summary>
        CS_ACTION_TYPE_SIMPLE = 6,

        /// <summary>
        /// 物理碰撞，打开，关闭等
        /// </summary>
        CS_ACTION_TYPE_PYSICS_COLLISION = 7,

    }
}
