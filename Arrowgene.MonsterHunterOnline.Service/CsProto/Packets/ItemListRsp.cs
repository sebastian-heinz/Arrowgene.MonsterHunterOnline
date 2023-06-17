using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Packets;

public class ItemListRsp : CsPacket
{
    public override CsProtoCmd Cmd => CsProtoCmd.CS_CMD_ITEM_LIST_RSP;

    public ItemListRsp()
    {
        CsItemListRsp = new CsItemListRsp();
    }

    public CsItemListRsp CsItemListRsp { get; set; }

    public override void Write(IBuffer buffer)
    {
        CsItemListRsp.Write(buffer);
    }
}