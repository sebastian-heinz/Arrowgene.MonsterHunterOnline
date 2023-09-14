using System;
using System.Data.Common;
using Arrowgene.MonsterHunterOnline.Service.System;

namespace Arrowgene.MonsterHunterOnline.Service.Database.Sql.Core
{
    public abstract partial class CoreSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private static readonly string[] AccountFields = new string[]
        {
            "uin", "password_hash", "created"
        };

        private static readonly string SqlInsertAccount =
            $"INSERT INTO `account` ({BuildQueryField(AccountFields)}) VALUES ({BuildQueryInsert(AccountFields)});";

        private static readonly string SqlSelectAccountById =
            $"SELECT `id`, {BuildQueryField(AccountFields)} FROM `account` WHERE `id`=@id;";

        private static readonly string SqlSelectAccountByUin =
            $"SELECT `id`, {BuildQueryField(AccountFields)} FROM `account` WHERE `uin`=@uin;";

        private static readonly string SqlUpdateAccount =
            $"UPDATE `account` SET {BuildQueryUpdate(AccountFields)} WHERE `id`=@id;";

        private const string SqlDeleteAccount = "DELETE FROM `account` WHERE `id`=@id;";

        public Account CreateAccount(uint uin, string passwordHash)
        {
            Account account = new Account();
            account.Uin = uin;
            account.PasswordHash = passwordHash;
            account.Created = DateTime.Now;
            // TODO
            account.AccountType = AccountType.User;
            int rowsAffected = ExecuteNonQuery(SqlInsertAccount, command =>
            {
                AddParameter(command, "@uin", account.Uin);
                AddParameter(command, "@password_hash", account.PasswordHash);
                AddParameter(command, "@created", account.Created);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return null;
            }

            account.Id = (uint)autoIncrement;

            return account;
        }

        public Account SelectAccountById(uint accountId)
        {
            Account account = null;
            ExecuteReader(SqlSelectAccountById, command => { AddParameter(command, "@id", accountId); }, reader =>
            {
                if (reader.Read())
                {
                    account = ReadAccount(reader);
                }
            });
            return account;
        }

        public Account SelectAccountByUin(uint uin)
        {
            Account account = null;
            ExecuteReader(SqlSelectAccountByUin, command => { AddParameter(command, "@uin", uin); }, reader =>
            {
                if (reader.Read())
                {
                    account = ReadAccount(reader);
                }
            });
            return account;
        }

        public bool UpdateAccount(Account account)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateAccount, command =>
            {
                AddParameter(command, "@uin", account.Uin);
                AddParameter(command, "@hash", account.PasswordHash);
                AddParameter(command, "@created", account.Created);
                AddParameter(command, "@id", account.Id);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteAccount(uint accountId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteAccount,
                command => { AddParameter(command, "@id", accountId); });
            return rowsAffected > NoRowsAffected;
        }

        private Account ReadAccount(DbDataReader reader)
        {
            Account account = new Account();
            account.Id = GetUInt32(reader, "id");
            account.Uin = GetUInt32(reader, "uin");
            account.PasswordHash = GetString(reader, "password_hash");
            account.Created = GetDateTime(reader, "created");
            // TODO
            account.AccountType = AccountType.User;
            return account;
        }
    }
}