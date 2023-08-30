namespace Arrowgene.MonsterHunterOnline.Service.System.ItemSystem.Constant;

public enum ItemColumnType : byte
{
    Item = 0,
    Store = 1,
    
    /// <summary>
    /// 'Box/Equip'-Tab of inventory menu (i)
    /// </summary>
    BoxEquip = 2,
    
    /// <summary>
    /// Paperdoll 'Equip'-Tab of character menu
    /// </summary>
    Equipment = 3,
    Quest = 5,
}