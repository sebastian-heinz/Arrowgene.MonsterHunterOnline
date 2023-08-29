using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class AttrSyncList : Structure, ICsStructure
{
    public AttrSyncList()
    {
        Attr = new List<AttrSync>();
    }

    public List<AttrSync> Attr { get; }

    public  void WriteCs(IBuffer buffer)
    {
        WriteList(buffer, Attr, CsProtoConstant.CS_ATTR_SYNC_LIST_MAX, WriteInt32, WriteCsStructure);
    }

    public void ReadCs(IBuffer buffer)
    {
        ReadList(buffer, Attr, CsProtoConstant.CS_ATTR_SYNC_LIST_MAX, ReadInt32, ReadCsStructure<AttrSync>);
    }
}