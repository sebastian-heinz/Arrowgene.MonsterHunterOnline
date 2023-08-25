using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class LineUpBigRandHandler : CsProtoStructureHandler<LineUpBigRand>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(LineUpBigRandHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_LINE_UP_BIGRAND;

    public LineUpBigRandHandler()
    {
    }

    public override void Handle(Client client, LineUpBigRand req)
    {
        // _characterManager.SyncAllAttr(client);
    }
}