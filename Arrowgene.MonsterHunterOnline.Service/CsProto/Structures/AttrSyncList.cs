using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class AttrSyncList : Structure
{
    public AttrSyncList()
    {
        Attr = new List<AttrSync>();
    }

    public List<AttrSync> Attr { get; }

    public override void Write(IBuffer buffer)
    {
        WriteList(buffer, Attr, CsProtoConstant.CS_ATTR_SYNC_LIST_MAX, WriteInt32, WriteStructure);
    }

    public override void Read(IBuffer buffer)
    {
        ReadList(buffer, Attr, CsProtoConstant.CS_ATTR_SYNC_LIST_MAX, ReadInt32, ReadStructure<AttrSync>);
    }
}