using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class EnterLevelNtfHandler : CsProtoStructureHandler<EnterLevelNtf>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(EnterLevelNtfHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_ENTER_LEVEL_NTF;

    private readonly CharacterManager _characterManager;

    public EnterLevelNtfHandler(CharacterManager characterManager)
    {
        _characterManager = characterManager;
    }

    public override void Handle(Client client, EnterLevelNtf req)
    {
        _characterManager.SyncAllAttr(client);

        if (!client.Character.IsSync)
        {

            // TODO hack to get visuals working
            CsProtoStructurePacket<TownInstanceVerifyRsp> townServerInitNtf = CsProtoResponse.TownServerInitNtf;
            TownInstanceVerifyRsp verifyRsp = townServerInitNtf.Structure;
            verifyRsp.ErrNo = 0;
            verifyRsp.LineId = 0;
            verifyRsp.LevelEnterType = 0;

            InstanceInitInfo instanceInitInfo = verifyRsp.InstanceInitInfo;
            instanceInitInfo.BattleGroundId = 0;
            instanceInitInfo.LevelId = 150301;
            instanceInitInfo.CreateMaxPlayerCount = 4;
            instanceInitInfo.GameMode = GameMode.Town;
            instanceInitInfo.TimeType = TimeType.Noon;
            instanceInitInfo.WeatherType = WeatherType.Sunny;
            instanceInitInfo.Time = 1;
            instanceInitInfo.LevelRandSeed = 1;
            instanceInitInfo.WarningFlag = 0;
            instanceInitInfo.CreatePlayerMaxLv = 99;

            client.SendCsProtoStructurePacket(townServerInitNtf);
            client.Character.IsSync = true;
        }
    }
}