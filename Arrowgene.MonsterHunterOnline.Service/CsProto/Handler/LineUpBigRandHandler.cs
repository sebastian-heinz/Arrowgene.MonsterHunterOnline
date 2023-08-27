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
        CsProtoStructurePacket<EnterInstanceCountDown> enterInstanceCountDown = CsProtoResponse.EnterInstanceCountDown;
        enterInstanceCountDown.Structure.Second = 300;
        enterInstanceCountDown.Structure.LevelId = 150301;
       // client.SendCsProtoStructurePacket(enterInstanceCountDown);

       CSInstanceDynamicInfoNtf init = new CSInstanceDynamicInfoNtf(new RulesInfoNone());
       init.CreatePlayerMaxLv = 999;
       init.StartTime = 0;
       init.RemainSeconds = 600;
        init.VipRemainSeconds = 900; 
        init.ItemRemainSeconds = 600;
        init.NormalMaxLimit = 10; 
        init.VipMaxLimit = 10;
        init.ItemMaxLimit = 10;
        init.PlayerNum = 4;
        init.IsHunterOfficer = 0;
        init.IsCrossServerInstance = 0;
        init.IsWarning = 0;
        init.HuntingMode = 1;
        init.ActHuntingMode = 0;
        init.IsBigRand = 1;
        
      // client.SendCsPacket(NewCsPacket.InstanceDynamicInfoNtf(init));
      CSMainInstanceUIRsp ui = new CSMainInstanceUIRsp();
      ui.UIInfoList.UIInfos.Add(new CSMainInstanceUIInfo()
      {
          LevelID = 150301
      });
     // client.SendCsPacket(NewCsPacket.MainInstanceUIRsp(ui));
    }
}