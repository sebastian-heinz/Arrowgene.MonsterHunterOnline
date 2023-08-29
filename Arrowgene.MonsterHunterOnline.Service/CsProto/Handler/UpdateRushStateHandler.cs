using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class UpdateRushStateHandler : CsProtoStructureHandler<UpdateRushState>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(UpdateRushStateHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_UPDATE_RUSHSTATE;


    public override void Handle(Client client, UpdateRushState req)
    {
        CsCsProtoStructurePacket<UpdateRushState> actorMoveStateNtf = CsProtoResponse.UpdateRushState;
        actorMoveStateNtf.Structure.Rush = req.Rush;
        actorMoveStateNtf.Structure.Type = req.Type;
    }
}