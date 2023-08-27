using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;

namespace Arrowgene.MonsterHunterOnline.Service.System.ClientAssetSystem
{
    public class ItemDataCsv : CsvReaderWriter<ItemData>
    {
        protected override int NumExpectedItems => 18;

        protected override ItemData CreateInstance(string[] properties)
        {
            if (!TryParse(properties, 0, out int itemId, int.TryParse, 0)) return null;
            string name = properties[1];
            if (!TryParse(properties, 8, out uint mainClass, uint.TryParse, 0)) return null;
            if (!TryParse(properties, 9, out uint category, uint.TryParse, 0)) return null;
            if (!TryParse(properties, 11, out uint bindingType, uint.TryParse, 0)) return null;
            if (!TryParse(properties, 13, out uint portableLimit, uint.TryParse, 0)) return null;
            if (!TryParse(properties, 14, out uint stackLimit, uint.TryParse, 0)) return null;
            if (!TryParse(properties, 17, out float price, float.TryParse, 0)) return null;
            if (!TryParse(properties, 18, out float salePrice, float.TryParse, 0)) return null;

            return new ItemData
            {
                ItemId = itemId,
                Name = name,
                MainClass = mainClass,
                Category = category,
                SubCategory = 0,
                BindingType = bindingType,
                PortableLimit = portableLimit,
                StackLimit = stackLimit,
                Price = price,
                SalePrice = salePrice
            };
        }
    }
}