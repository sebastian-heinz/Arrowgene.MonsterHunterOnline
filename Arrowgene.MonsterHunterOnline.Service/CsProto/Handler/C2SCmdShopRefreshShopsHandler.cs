using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class C2SCmdShopRefreshShopsHandler : ICsProtoHandler
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(C2SCmdShopRefreshShopsHandler));


    public CS_CMD_ID Cmd => CS_CMD_ID.C2S_CMD_SHOP_REFRESHSHOPS;


    public void Handle(Client client, CsProtoPacket packet)
    {
        client.SendCsPacket(NewCsPacket.S2CRefreshShop(new S2CRefreshShop()));
    }
}