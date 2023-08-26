namespace Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;

public class ItemData
{
    public ItemData()
    {
    }

    public ulong ItemId { get; set; }
    public string Name { get; set; }
    public uint MainClass { get; set; }
    public uint Category { get; set; }
    public uint SubCategory { get; set; }
    public uint BindingType { get; set; }
    public float Price { get; set; }
    public float SalePrice { get; set; }
    public uint PortableLimit { get; set; }
    public uint StackLimit { get; set; }
}