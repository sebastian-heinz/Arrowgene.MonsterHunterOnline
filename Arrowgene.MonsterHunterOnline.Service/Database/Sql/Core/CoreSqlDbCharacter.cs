using System;
using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.System;

namespace Arrowgene.MonsterHunterOnline.Service.Database.Sql.Core
{
    public abstract partial class CoreSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private static readonly string[] CharacterFields = new string[]
        {
            "account_id", "role_index", "gender", "level", "name", "role_state", "role_state_end_left_time",
            "avatar_set_id", "face_id", "hair_id", "underclothes_id", "skin_color", "hair_color", "inner_color",
            "eye_ball", "eye_color", "face_tattoo_index", "face_tattoo_color", "facial_info", "star_level", "hr_level",
            "soul_stone_lv", "hide_helm", "hide_fashion", "hide_suite", "created"
        };

        private readonly string SqlInsertCharacter =
            $"INSERT INTO `character` ({BuildQueryField(CharacterFields)}) VALUES ({BuildQueryInsert(CharacterFields)});";

        private static readonly string SqlUpdateCharacter =
            $"UPDATE `character` SET {BuildQueryUpdate(CharacterFields)} WHERE `id` = @id;";

        private static readonly string SqlSelectCharacterById =
            $"SELECT `character`.`id`, {BuildQueryField(CharacterFields)} FROM `character` WHERE `id` = @id;";

        private static readonly string SqlSelectCharactersByAccountId =
            $"SELECT `character`.`id`, {BuildQueryField(CharacterFields)} FROM `character` WHERE `account_id` = @account_id;";

        private static readonly string SqlSelectCharactersByRoleIndex =
            $"SELECT `character`.`id`, {BuildQueryField(CharacterFields)} FROM `character` WHERE `account_id` = @account_id AND `role_index` = @role_index;";

        private const string SqlDeleteCharacter =
            "DELETE FROM `character` WHERE `id`=@id;";

        private const string SqlFreeCharacterIndex =
            "SELECT MIN(`role_index`)+1 AS `free_index` FROM `character` WHERE `role_index`+1 NOT IN (SELECT `role_index` FROM `character` WHERE `account_id`=@account_id);";


        public bool GetFreeCharacterIndex(uint accountId, out byte freeIndex)
        {
            freeIndex = 0;
            byte index = 0;
            bool result = false;
            ExecuteReader(
                SqlFreeCharacterIndex,
                command => { AddParameter(command, "@account_id", accountId); },
                reader =>
                {
                    if (reader.Read())
                    {
                        int ordinal = reader.GetOrdinal("free_index");
                        if (reader.IsDBNull(ordinal))
                        {
                            result = true;
                            index = 0;
                        }
                        else
                        {
                            index = GetByte(reader, "free_index");
                            result = true;
                        }
                    }
                });

            freeIndex = index;
            return result;
        }

        public bool CreateCharacter(Character character)
        {
            if (!GetFreeCharacterIndex(character.AccountId, out byte freeIndex))
            {
                return false;
            }

            character.Index = freeIndex;
            character.Created = DateTime.Now;
            int rowsAffected = ExecuteNonQuery(
                SqlInsertCharacter,
                command => { AddParameter(command, character); },
                out long autoIncrement
            );
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

        public Character SelectCharacterByRoleIndex(uint accountId, byte roleIndex)
        {
            Character character = null;
            ExecuteReader(SqlSelectCharactersByRoleIndex,
                command =>
                {
                    AddParameter(command, "@account_id", accountId);
                    AddParameter(command, "@role_index", roleIndex);
                }, reader =>
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
                    while (reader.Read())
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
                AddParameter(command, character);
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
            character.Index = GetByte(reader, "role_index");
            character.Gender = GetByte(reader, "gender");
            character.Level = GetUInt32(reader, "level");
            character.Name = GetString(reader, "name");
            character.RoleState = GetInt32(reader, "role_state");
            character.RoleStateEndLeftTime = GetUInt32(reader, "role_state_end_left_time");
            character.AvatarSetId = GetByte(reader, "avatar_set_id");
            character.FaceId = GetUInt16(reader, "face_id");
            character.HairId = GetUInt16(reader, "hair_id");
            character.UnderclothesId = GetUInt16(reader, "underclothes_id");
            character.SkinColor = GetInt32(reader, "skin_color");
            character.HairColor = GetInt32(reader, "hair_color");
            character.InnerColor = GetInt32(reader, "inner_color");
            character.EyeBall = GetInt32(reader, "eye_ball");
            character.EyeColor = GetInt32(reader, "eye_color");
            character.FaceTattooIndex = GetInt32(reader, "face_tattoo_index");
            character.FaceTattooColor = GetInt32(reader, "face_tattoo_color");

            int facialInfoEncodedSize = CsProtoConstant.CS_MAX_FACIALINFO_COUNT * 2;
            byte[] facialInfoEncoded = GetBytes(reader, "facial_info", facialInfoEncodedSize);
            Buffer.BlockCopy(facialInfoEncoded, 0, character.FacialInfo, 0, facialInfoEncodedSize);

            character.StarLevel = GetString(reader, "star_level");
            character.HrLevel = GetInt32(reader, "hr_level");
            character.SoulStoneLv = GetInt32(reader, "soul_stone_lv");
            character.HideHelm = GetBool(reader, "hide_helm");
            character.HideFashion = GetBool(reader, "hide_fashion");
            character.HideSuite = GetBool(reader, "hide_suite");
            character.Created = GetDateTime(reader, "created");
            return character;
        }

        private void AddParameter(TCom command, Character character)
        {
            AddParameter(command, "@account_id", character.AccountId);
            AddParameter(command, "@role_index", character.Index);
            AddParameter(command, "@gender", character.Gender);
            AddParameter(command, "@level", character.Level);
            AddParameter(command, "@name", character.Name);
            AddParameter(command, "@role_state", character.RoleState);
            AddParameter(command, "@role_state_end_left_time", character.RoleStateEndLeftTime);
            AddParameter(command, "@avatar_set_id", character.AvatarSetId);
            AddParameter(command, "@face_id", character.FaceId);
            AddParameter(command, "@hair_id", character.HairId);
            AddParameter(command, "@underclothes_id", character.UnderclothesId);
            AddParameter(command, "@skin_color", character.SkinColor);
            AddParameter(command, "@hair_color", character.HairColor);
            AddParameter(command, "@inner_color", character.InnerColor);
            AddParameter(command, "@eye_ball", character.EyeBall);
            AddParameter(command, "@eye_color", character.EyeColor);
            AddParameter(command, "@face_tattoo_index", character.FaceTattooIndex);
            AddParameter(command, "@face_tattoo_color", character.FaceTattooColor);

            byte[] facialInfoEncoded = new byte[CsProtoConstant.CS_MAX_FACIALINFO_COUNT * 2];
            Buffer.BlockCopy(character.FacialInfo, 0, facialInfoEncoded, 0, CsProtoConstant.CS_MAX_FACIALINFO_COUNT);
            AddParameter(command, "@facial_info", facialInfoEncoded);

            AddParameter(command, "@star_level", character.StarLevel);
            AddParameter(command, "@hr_level", character.HrLevel);
            AddParameter(command, "@soul_stone_lv", character.SoulStoneLv);
            AddParameter(command, "@hide_helm", character.HideHelm);
            AddParameter(command, "@hide_fashion", character.HideFashion);
            AddParameter(command, "@hide_suite", character.HideSuite);
            AddParameter(command, "@created", character.Created);
        }
    }
}