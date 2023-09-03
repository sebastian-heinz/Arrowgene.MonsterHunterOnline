using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 副本主UI操作进入响应
    /// </summary>
    public class MainInstanceEnterOptRsp : Structure, ICsStructure
    {
        public MainInstanceEnterOptRsp()
        {
            NetId = 0;
            ErrCode = 0;
            LevelId = 0;
            WarningFlag = 0;
            UIInfos = new List<BSMRoomUIPlayerInfo>();
            UseEmploye = 0;
            WeaponTrialLevel = 0;
            WeaponType = 0;
        }

        /// <summary>
        /// 谁的错误码
        /// </summary>
        public int NetId { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public int ErrCode { get; set; }

        /// <summary>
        /// LevelID
        /// </summary>
        public int LevelId { get; set; }

        /// <summary>
        /// Warning标记
        /// </summary>
        public byte WarningFlag { get; set; }

        public List<BSMRoomUIPlayerInfo> UIInfos { get; }

        /// <summary>
        /// 雇佣兵
        /// </summary>
        public int UseEmploye { get; set; }

        /// <summary>
        /// 武器试炼关等级
        /// </summary>
        public int WeaponTrialLevel { get; set; }

        /// <summary>
        /// 武器试炼武器类型
        /// </summary>
        public int WeaponType { get; set; }

        public void WriteCs(IBuffer buffer)
        {
            WriteInt32(buffer, NetId);
            WriteInt32(buffer, ErrCode);
            WriteInt32(buffer, LevelId);
            WriteByte(buffer, WarningFlag);
            WriteList(buffer, UIInfos,
                (short)CsProtoConstant.CS_MAX_BSM_CANDIDATE_COUNT,
                WriteInt16,
                WriteCsStructure
            );
            WriteInt32(buffer, UseEmploye);
            WriteInt32(buffer, WeaponTrialLevel);
            WriteInt32(buffer, WeaponType);
        }

        public void ReadCs(IBuffer buffer)
        {
            NetId = ReadInt32(buffer);
            ErrCode = ReadInt32(buffer);
            LevelId = ReadInt32(buffer);
            WarningFlag = ReadByte(buffer);
            ReadList(buffer,
                UIInfos,
                (short)CsProtoConstant.CS_MAX_BSM_CANDIDATE_COUNT,
                ReadInt16,
                ReadCsStructure<BSMRoomUIPlayerInfo>
            );
            UseEmploye = ReadInt32(buffer);
            WeaponTrialLevel = ReadInt32(buffer);
            WeaponType = ReadInt32(buffer);
        }
    }
}