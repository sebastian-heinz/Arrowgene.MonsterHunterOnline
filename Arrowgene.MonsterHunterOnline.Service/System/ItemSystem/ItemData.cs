namespace Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;

/// <summary>
/// Static Item Data
/// </summary>
public class ItemData
{
    public ItemData()
    {
    }

    public int ItemId { get; init; }
    public string Name { get; init; }
    public uint MainClass { get; init; }
    public uint Category { get; init; }
    public uint SubCategory { get; init; }
    public uint BindingType { get; init; }
    public float Price { get; init; }
    public float SalePrice { get; init; }
    public uint PortableLimit { get; init; }
    public uint StackLimit { get; init; }
}