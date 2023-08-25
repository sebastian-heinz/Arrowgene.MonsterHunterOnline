using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class RulesInfoNone : CSRulesInfo
{
    public void Write(IBuffer buffer)
    {
    }

    public void Read(IBuffer buffer)
    {
    }

    public CS_BATTLE_RULES_TYPE RulesInfoType => CS_BATTLE_RULES_TYPE.BATTLE_RULES_DEFAULT_INFO;
}