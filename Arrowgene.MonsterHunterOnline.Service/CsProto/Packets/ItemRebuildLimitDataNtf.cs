using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Packets;

public class ItemRebuildLimitDataNtf : CsPacket
{
    public override CsProtoCmd Cmd => CsProtoCmd.CS_CMD_ITEMREBUILD_LIMITDATA_NTF;

    public ItemRebuildLimitDataNtf()
    {
        CsItemRebuildLimitInfo = new CsItemRebuildLimitInfo();
    }

    public CsItemRebuildLimitInfo CsItemRebuildLimitInfo { get; set; }

    public override void Write(IBuffer buffer)
    {
        CsItemRebuildLimitInfo.Write(buffer);
    }
}