using System.Collections.Generic;

namespace Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;

public class Item
{
    public Item()
    {
        Attributes = new List<ItemAttribute>();
    }

    public long Id { get; set; }
    public int Type { get; set; }
    public ItemColumnType PosColumn { get; set; }
    public short PosGrid { get; set; }
    public short Quantity { get; set; }
    public bool Bind { get; set; }
    public List<ItemAttribute> Attributes { get; }
}