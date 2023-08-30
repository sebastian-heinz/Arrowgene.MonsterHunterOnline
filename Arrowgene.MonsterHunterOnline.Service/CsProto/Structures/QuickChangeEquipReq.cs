using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 快速换装
    /// </summary>
    public class QuickChangeEquipReq : Structure, ICsStructure
    {
        public QuickChangeEquipReq()
        {
            EquipParamList = new List<EquipParam>();
        }

        /// <summary>
        /// 装备列表
        /// </summary>
        public List<EquipParam> EquipParamList { get; }

        public void WriteCs(IBuffer buffer)
        {
            WriteList(buffer, EquipParamList, CsProtoConstant.ROLE_EQUIPED_MAX_NETMESSAGE, WriteInt32,
                WriteCsStructure);
        }

        public void ReadCs(IBuffer buffer)
        {
            ReadList(buffer,
                EquipParamList,
                CsProtoConstant.ROLE_EQUIPED_MAX_NETMESSAGE,
                ReadInt32,
                ReadCsStructure<EquipParam>
            );
        }
    }
}