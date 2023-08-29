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
            "character_id", "item_id", "column", "grid", "quantity", "created", "created_by"
        };

        private static readonly string SqlInsertItem =
            $"INSERT INTO `item` ({BuildQueryField(ItemFields)}) VALUES ({BuildQueryInsert(ItemFields)});";

        private static readonly string SqlSelectItemsByCharacterId =
            $"SELECT `id`, {BuildQueryField(ItemFields)} FROM `item` WHERE `character_id`=@character_id;";

        private static readonly string SqlUpdateItem =
            $"UPDATE `item` SET {BuildQueryUpdate(ItemFields)} WHERE `id`=@id;";

        private const string SqlDeleteItem = "DELETE FROM `item` WHERE `id`=@id;";

        public bool CreateItem(Item item)
        {
            if (item.PosGrid == null)
            {
                return false;
            }

            item.Created = DateTime.Now;

            int rowsAffected = ExecuteNonQuery(
                SqlInsertItem,
                command => { AddParameter(command, item); },
                out long autoIncrement
            );
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            item.Id = (ulong)autoIncrement;
            return true;
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
            if (item.Id == null)
            {
                return false;
            }

            if (item.PosGrid == null)
            {
                return false;
            }

            int rowsAffected = ExecuteNonQuery(SqlUpdateItem, command =>
            {
                AddParameter(command, "@id", item.Id.Value);
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
            if (item.PosGrid == null)
            {
                throw new Exception("item.PosGrid == null");
            }

            AddParameter(command, "@item_id", item.ItemId);
            AddParameter(command, "@character_id", item.CharacterId);
            AddParameter(command, "@column", (byte)item.PosColumn);
            AddParameter(command, "@grid", item.PosGrid.Value);
            AddParameter(command, "@quantity", item.Quantity);
            AddParameter(command, "@created_by", item.CreatedBy);
            AddParameter(command, "@created", item.Created);
        }
    }
}