using System;
using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem.Constant;

namespace Arrowgene.MonsterHunterOnline.Service.Database.Sql.Core
{
    public abstract partial class CoreSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private static readonly string[] ItemFields = new string[]
        {
            "character_id", "item_id", "column", "grid", "quantity", "created"
        };

        private static readonly string SqlInsertItem =
            $"INSERT INTO `item` ({BuildQueryField(ItemFields)}) VALUES ({BuildQueryInsert(ItemFields)});";

        private static readonly string SqlSelectItemsByCharacterId =
            $"SELECT `id`, {BuildQueryField(ItemFields)} FROM `item` WHERE `character_id`=@character_id;";

        private static readonly string SqlUpdateItem =
            $"UPDATE `item` SET {BuildQueryUpdate(ItemFields)} WHERE `id`=@id;";

        private const string SqlDeleteItem = "DELETE FROM `item` WHERE `id`=@id;";

        public Item CreateItem(
            uint itemId,
            uint characterId,
            ItemColumnType column,
            ushort grid,
            ushort quantity,
            string createdBy)
        {
            Item item = new Item();
            item.ItemId = itemId;
            item.CharacterId = characterId;
            item.PosColumn = column;
            item.PosGrid = grid;
            item.Quantity = quantity;
            item.CreatedBy = createdBy;
            item.Created = DateTime.Now;

            int rowsAffected = ExecuteNonQuery(
                SqlInsertItem,
                command => { AddParameter(command, item); },
                out long autoIncrement
            );
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return null;
            }


            item.Id = (ulong)autoIncrement;
            return item;
        }

        public List<Item> SelectItemsByCharacterId(uint characterId)
        {
            List<Item> items = new List<Item>();
            ExecuteReader(
                SqlSelectItemsByCharacterId,
                command => { AddParameter(command, "@id", characterId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        items.Add(ReadItem(reader));
                    }
                }
            );
            return items;
        }

        public bool UpdateItem(Item item)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateItem, command =>
            {
                AddParameter(command, "@id", item.Id);
                AddParameter(command, item);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteItem(ulong itemId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteItem,
                command => { AddParameter(command, "@id", itemId); });
            return rowsAffected > NoRowsAffected;
        }

        private Item ReadItem(DbDataReader reader)
        {
            Item item = new Item();
            item.Id = GetUInt64(reader, "id");
            item.ItemId = GetUInt32(reader, "item_id");
            item.CharacterId = GetUInt32(reader, "character_id");
            item.PosColumn = GetEnumByte<ItemColumnType>(reader, "column");
            item.PosGrid = GetUInt16(reader, "grid");
            item.Quantity = GetUInt16(reader, "quantity");
            item.CreatedBy = GetString(reader, "created_by");
            item.Created = GetDateTime(reader, "created");
            return item;
        }

        private void AddParameter(TCom command, Item item)
        {
            AddParameter(command, "@item_id", item.ItemId);
            AddParameter(command, "@character_id", item.CharacterId);
            AddParameter(command, "@column", (byte)item.PosColumn);
            AddParameter(command, "@grid", item.PosGrid);
            AddParameter(command, "@quantity", item.Quantity);
            AddParameter(command, "@created_by", item.CreatedBy);
            AddParameter(command, "@created", item.Created);
        }
    }
}