using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Protocol.Constant;
using Arrowgene.MonsterHunterOnline.Protocol.Old.Structures;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class SessionTssTimeNtfHandler : CsProtoStructureHandler<CSessionTSSTimeNotify>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(SessionTssTimeNtfHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_SESSION_TSS_TIME_NTF;

    public override void Handle(Client client, CSessionTSSTimeNotify req)
    {
        Logger.Info(client, $"TSS Time: total={req.TotalTime} max={req.MaxTime} pkgCount={req.PkgCount}");

        CsCsProtoStructurePacket<CSessionTSSTimeNotify> resp =
            new(CS_CMD_ID.CS_CMD_SESSION_TSS_TIME_NTF);
        resp.Structure.TotalTime = req.TotalTime;
        resp.Structure.MaxTime = req.MaxTime;
        resp.Structure.PkgCount = req.PkgCount;
        client.SendCsProtoStructurePacket(resp);
    }
}
