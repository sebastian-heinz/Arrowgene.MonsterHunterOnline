using System;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem.Constant;

namespace Arrowgene.MonsterHunterOnline.Service.System.ClientAssetSystem
{
    public class ItemDataCsv : CsvReaderWriter<ItemInfo>
    {
        protected override int NumExpectedItems => 18;

        protected override ItemInfo CreateInstance(string[] properties)
        {
            if (!TryParse(properties, 0, out uint itemId, uint.TryParse, 0)) return null;
            string name = properties[1];
            if (!TryParse(properties, 8, out ItemClass mainClass, Enum.TryParse, 0)) return null;
            if (!TryParse(properties, 9, out ItemCategory category, Enum.TryParse, 0)) return null;
            if (!TryParse(properties, 11, out uint bindingType, uint.TryParse, 0)) return null;
            if (!TryParse(properties, 13, out uint portableLimit, uint.TryParse, 0)) return null;
            if (!TryParse(properties, 14, out uint stackLimit, uint.TryParse, 0)) return null;
            if (!TryParse(properties, 17, out float price, float.TryParse, 0)) return null;
            if (!TryParse(properties, 18, out float salePrice, float.TryParse, 0)) return null;

            return new ItemInfo
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