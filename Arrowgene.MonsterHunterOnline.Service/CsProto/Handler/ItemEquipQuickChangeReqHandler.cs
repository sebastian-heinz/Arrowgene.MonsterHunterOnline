using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class ItemEquipQuickChangeReqHandler : CsProtoStructureHandler<ActorIdleMove>
{
    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_ITEM_EQUIP_QUICK_CHANGE_REQ;


    public override void Handle(Client client, ActorIdleMove req)
    {
        // TODO
    }
}