using System;
using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.MonsterHunterOnline.Service.System;

namespace Arrowgene.MonsterHunterOnline.Service.Database.Sql.Core
{
    public abstract partial class CoreSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private static readonly string[] CharacterFields = new string[]
        {
            "account_id", "name", "created"
        };

        private readonly string SqlInsertCharacter =
            $"INSERT INTO `character` ({BuildQueryField(CharacterFields)}) VALUES ({BuildQueryInsert(CharacterFields)});";

        private static readonly string SqlUpdateCharacter =
            $"UPDATE `character` SET {BuildQueryUpdate(CharacterFields)} WHERE `id` = @id;";

        private static readonly string SqlSelectCharacterById =
            $"SELECT `character`.`id`, {BuildQueryField(CharacterFields)} FROM `character` WHERE `id` = @id;";

        private static readonly string SqlSelectCharactersByAccountId =
            $"SELECT `character`.`id`, {BuildQueryField(CharacterFields)} FROM `character` WHERE `account_id` = @account_id;";

        private const string SqlDeleteCharacter =
            "DELETE FROM `character` WHERE `id`=@id;";

        public bool CreateCharacter(Character character)
        {
            character.Created = DateTime.Now;
            int rowsAffected = ExecuteNonQuery(SqlInsertCharacter, command =>
            {
                AddParameter(command, "@account_id", character.AccountId);
                AddParameter(command, "@name", character.Name);
                AddParameter(command, "@created", character.Created);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            character.Id = (uint)autoIncrement;
            return true;
        }


        public Character SelectCharacterById(uint characterId)
        {
            Character character = null;
            ExecuteReader(SqlSelectCharacterById, command => { AddParameter(command, "@id", characterId); }, reader =>
            {
                if (reader.Read())
                {
                    character = ReadCharacter(reader);
                }
            });
            return character;
        }

        public List<Character> SelectCharactersByAccountId(uint accountId)
        {
            List<Character> characters = new List<Character>();
            ExecuteReader(
                SqlSelectCharactersByAccountId,
                command => { AddParameter(command, "@account_id", accountId); },
                reader =>
                {
                    if (reader.Read())
                    {
                        Character character = ReadCharacter(reader);
                        characters.Add(character);
                    }
                });
            return characters;
        }

        public bool UpdateCharacter(Character character)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateCharacter, command =>
            {
                AddParameter(command, "@account_id", character.AccountId);
                AddParameter(command, "@name", character.Name);
                AddParameter(command, "@created", character.Created);
                AddParameter(command, "@id", character.Id);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteCharacter(uint characterId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteCharacter,
                command => { AddParameter(command, "@id", characterId); });
            return rowsAffected > NoRowsAffected;
        }

        private Character ReadCharacter(DbDataReader reader)
        {
            Character character = new Character();

            character.Id = GetUInt32(reader, "id");
            character.AccountId = GetUInt32(reader, "account_id");
            character.Name = GetString(reader, "name");
            character.Created = GetDateTime(reader, "created");
            return character;
        }
    }
}