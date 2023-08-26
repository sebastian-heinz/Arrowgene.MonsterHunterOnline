namespace Arrowgene.MonsterHunterOnline.Service.System.ClientAssetSystem
{
    public class ItemDataCsv : CsvReaderWriter<ItemDataCsv.Entry>
    {
        public class Entry
        {
            public ulong ItemId { get; set; }
            public string Name { get; set; }
            public uint MainClass { get; set; }
            public uint Category { get; set; }
            public uint BindingType { get; set; }
            public uint PortableLimit { get; set; }
            public uint StackLimit { get; set; }
            public float Price { get; set; }
            public float SalePrice { get; set; }
        }

        protected override int NumExpectedItems => 26;

        protected override Entry CreateInstance(string[] properties)
        {
            if (!TryParse(properties, 0, out ulong itemId, ulong.TryParse, 0)) return null;
            string name = properties[1];
            if (!TryParse(properties, 8, out uint mainClass, uint.TryParse, 0)) return null;
            if (!TryParse(properties, 9, out uint category, uint.TryParse, 0)) return null;
            if (!TryParse(properties, 11, out uint bindingType, uint.TryParse, 0)) return null;
            if (!TryParse(properties, 13, out uint portableLimit, uint.TryParse, 0)) return null;
            if (!TryParse(properties, 14, out uint stackLimit, uint.TryParse, 0)) return null;
            if (!TryParse(properties, 17, out float price, float.TryParse, 0)) return null;
            if (!TryParse(properties, 18, out float salePrice, float.TryParse, 0)) return null;

            return new Entry
            {
                ItemId = itemId,
                Name = name,
                MainClass = mainClass,
                Category = category,
                BindingType = bindingType,
                PortableLimit = portableLimit,
                StackLimit = stackLimit,
                Price = price,
                SalePrice = salePrice
            };
        }
    }
}