using System.Collections.Generic;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem.Constant;

namespace Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;

public class Item
{
    public Item(ItemData itemData)
    {
        Attributes = new List<ItemAttribute>();
        Data = itemData;
    }

    public ItemData Data { get; }

    /// <summary>
    /// Unique Id to keep track of item
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Id to reference static item data
    /// </summary>
    public int ItemId { get; set; }

    public ItemColumnType PosColumn { get; set; }
    public short PosGrid { get; set; }
    public short Quantity { get; set; }
    public bool Bind { get; set; }
    public List<ItemAttribute> Attributes { get; }
}