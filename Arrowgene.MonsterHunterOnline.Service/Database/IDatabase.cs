using System.Collections.Generic;
using Arrowgene.MonsterHunterOnline.Service.System;

namespace Arrowgene.MonsterHunterOnline.Service.Database
{
    public interface IDatabase
    {
        void Execute(string sql);

        bool CreateDatabase();

        // Account
        Account CreateAccount(uint uin, string passwordHash);
        Account SelectAccountById(uint accountId);
        Account SelectAccountByUin(uint uin);
        bool UpdateAccount(Account account);
        bool DeleteAccount(uint accountId);

        // Character
        bool CreateCharacter(Character character);
        Character SelectCharacterById(uint characterId);
        bool UpdateCharacter(Character character);
        List<Character> SelectCharactersByAccountId(uint accountId);
        bool DeleteCharacter(uint characterId);
    }
}