using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class RulesInfoNone : Structure, ICsStructure, CSRulesInfo
{
    public void WriteCs(IBuffer buffer)
    {
    }

    public void ReadCs(IBuffer buffer)
    {
    }

    public CS_BATTLE_RULES_TYPE RulesInfoType => CS_BATTLE_RULES_TYPE.BATTLE_RULES_DEFAULT_INFO;
}