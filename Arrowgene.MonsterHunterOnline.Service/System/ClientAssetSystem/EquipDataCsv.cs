using System;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem.Constant;

namespace Arrowgene.MonsterHunterOnline.Service.System.ClientAssetSystem
{
    public class EquipDataCsv : CsvReaderWriter<ItemInfo>
    {
        protected override int NumExpectedItems => 19;

        protected override ItemInfo CreateInstance(string[] properties)
        {
            if (!TryParse(properties, 0, out uint itemId, uint.TryParse, 0)) return null;
            string name = properties[1];
            if (!TryParse(properties, 6, out int keepCopy, int.TryParse, 0)) return null;
            if (!TryParse(properties, 9, out ItemClass mainClass, Enum.TryParse, 0)) return null;
            if (!TryParse(properties, 10, out ItemCategory category, Enum.TryParse, 0)) return null;
            if (!TryParse(properties, 11, out uint subCategory, uint.TryParse, 0)) return null;
            if (!TryParse(properties, 12, out uint bindingType, uint.TryParse, 0)) return null;
            if (!TryParse(properties, 18, out float price, float.TryParse, 0)) return null;
            if (!TryParse(properties, 19, out float salePrice, float.TryParse, 0)) return null;

            return new ItemInfo
            {
                ItemId = itemId,
                Name = name,
                KeepCopy = keepCopy != 0,
                MainClass = mainClass,
                Category = category,
                SubCategory = subCategory,
                BindingType = bindingType,
                PortableLimit = 0,
                StackLimit = 1,
                Price = price,
                SalePrice = salePrice,
            };
        }
    }
}