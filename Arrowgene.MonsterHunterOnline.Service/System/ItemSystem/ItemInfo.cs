using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem.Constant;

namespace Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;

/// <summary>
/// Static Item Data
/// </summary>
public class ItemInfo
{
    public ItemInfo()
    {
    }

    public uint ItemId { get; init; }
    public string Name { get; init; }
    public ItemClass MainClass { get; init; }
    public ItemCategory Category { get; init; }
    public uint SubCategory { get; init; }
    public uint BindingType { get; init; }
    public float Price { get; init; }
    public float SalePrice { get; init; }
    public uint PortableLimit { get; init; }
    public uint StackLimit { get; init; }
    
    /// <summary>
    /// Items with this flag use ItemColumnType 8(equip) and 9(box)
    /// and are not kept with normal inventory
    /// </summary>
    public bool KeepCopy { get; init; }

    public override string ToString()
    {
        return $"{nameof(ItemId)}: {ItemId}, {nameof(Name)}: {Name}";
    }
}