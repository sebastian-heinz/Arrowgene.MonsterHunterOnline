using System.Collections.Generic;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.System.Chat.Command.Commands
{
    /// <summary>
    /// Sends CS_CMD_TOWN_SERVER_INIT_NTF packet
    /// </summary>
    public class TownInitCommand : ChatCommand
    {
        public override AccountType Account => AccountType.User;
        public override string Key => "town_init";
        public override string HelpText => "usage: `/town_init` - Sends CS_CMD_TOWN_SERVER_INIT_NTF packet";

        public override void Execute(string[] command, Client client, ChatMessage message, List<ChatMessage> responses)
        {
            CsProtoStructurePacket<TownInstanceVerifyRsp> townServerInitNtf = CsProtoResponse.TownServerInitNtf;
            TownInstanceVerifyRsp verifyRsp = townServerInitNtf.Structure;
            verifyRsp.ErrNo = 0;
            verifyRsp.LineId = 0;
            verifyRsp.LevelEnterType = 0;

            InstanceInitInfo instanceInitInfo = verifyRsp.InstanceInitInfo;
            instanceInitInfo.BattleGroundId = 0;
            instanceInitInfo.LevelId = client.State.levelId;
            instanceInitInfo.CreateMaxPlayerCount = 4;
            instanceInitInfo.GameMode = GameMode.Town;
            instanceInitInfo.GameMode = GameMode.Story;
            instanceInitInfo.TimeType = TimeType.Noon;
            instanceInitInfo.WeatherType = WeatherType.Sunny;
            instanceInitInfo.Time = 1;
            instanceInitInfo.LevelRandSeed = 1;
            instanceInitInfo.WarningFlag = 0;
            instanceInitInfo.CreatePlayerMaxLv = 99;

            client.SendCsProtoStructurePacket(townServerInitNtf);

            // TODO, perhaps refactor to a change level method somewhere to handle
            client.State.prevLevelId = client.State.levelId;
            client.State.levelId = instanceInitInfo.LevelId;
        }
    }
}