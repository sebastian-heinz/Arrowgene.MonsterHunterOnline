namespace Arrowgene.MonsterHunterOnline.Service.System.ClientAssetSystem
{
    public class EquipDataCsv : CsvReaderWriter<EquipDataCsv.Entry>
    {
        public class Entry
        {
            public ulong ItemId { get; set; }
            public string Name { get; set; }
            public uint MainClass { get; set; }
            public uint Category { get; set; }
            public uint SubCategory { get; set; }
            public uint BindingType { get; set; }
            public uint Price { get; set; }
            public uint SalePrice { get; set; }
        }

        protected override int NumExpectedItems => 26;

        protected override Entry CreateInstance(string[] properties)
        {
            if (!ulong.TryParse(properties[0], out ulong itemId)) return null;
            string name = properties[1];
            if (!uint.TryParse(properties[9], out uint mainClass)) return null;
            if (!uint.TryParse(properties[10], out uint category)) return null;
            if (!uint.TryParse(properties[11], out uint subCategory)) return null;
            if (!uint.TryParse(properties[12], out uint bindingType)) return null;
            if (!uint.TryParse(properties[18], out uint price)) return null;
            if (!uint.TryParse(properties[19], out uint salePrice)) return null;

            return new Entry
            {
                ItemId = itemId,
                Name = name,
                MainClass = mainClass,
                Category = category,
                SubCategory = subCategory,
                BindingType = bindingType,
                Price = price,
                SalePrice = salePrice,
            };
        }
    }
}