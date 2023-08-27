using NotImplementedException = System.NotImplementedException;

namespace Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;

public class Inventory
{
    private Item[][] _item;
    private Item[][] _store;
    private Item[][] _box;
    private Item[] _equipment;
    private Item[][] _quest;
    
    public Inventory()
    {
        
    }


    public bool Add(Item item)
    {
        return true;
    }
}