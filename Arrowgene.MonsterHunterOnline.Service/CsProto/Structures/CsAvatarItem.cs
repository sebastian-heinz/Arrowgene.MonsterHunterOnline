using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class CsAvatarItem : IStructure
{
    //Equip  refer="EquipCount"/
    // <struct name="CSAvatarItem" version="1" desc="角色装备显示物品">
    //     <entry name="ItemType" type="int" desc="物品类型"/>
    //     <entry name="PosIndex" type="uint16" desc="物品位置行"/>
    //     <entry name="Rank" type="uint32" desc="Rank级别"/>
    //     <entry name="EnhanceRule" type="uint8" desc="强化规则"/>
    //     <entry name="EnhanceLevel" type="uint8" desc="强化数据"/>
    //     <entry name="SoltCount" type="uint8" desc="插孔数"/>
    //     <entry name="GemID" type="uint32" desc="插孔宝石物品"/>
    //     <entry name="BreakLevel" type="uint8" desc="突破级别"/>
    //     <entry name="ColorIndex" type="uint8" desc="染色效果"/>
    //     <entry name="FakeShow" type="uint32" desc="幻化效果"/>
    //     <entry name="WakeLevel" type="uint8" desc="觉醒层级"/>
    //     </struct>

    public void Write(IBuffer buffer)
    {
    }
}