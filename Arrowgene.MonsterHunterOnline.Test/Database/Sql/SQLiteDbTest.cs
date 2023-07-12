using System.Collections.Generic;
using System.IO;
using Arrowgene.MonsterHunterOnline.Service;
using Arrowgene.MonsterHunterOnline.Service.Database;
using Arrowgene.MonsterHunterOnline.Service.Database.Sql;
using Arrowgene.MonsterHunterOnline.Service.System;
using Xunit;

namespace Arrowgene.MonsterHunterOnline.Test.Database.Sql;

public class SQLiteDbTest
{
    [Fact]
    public void TestSQLiteDbAccount()
    {
        SQLiteDb db = CreateDb(0);

        Account acc = db.CreateAccount(1234, "pw");
        Assert.NotNull(acc);

        Account selectedAcc = db.SelectAccountByUin(1234);
        Assert.NotNull(selectedAcc);
    }

    [Fact]
    public void TestSQLiteDbCharacter()
    {
        SQLiteDb db = CreateDb(0);

        Account acc = db.CreateAccount(1234, "pw");
        Assert.NotNull(acc);

        Character cha = new Character();
        cha.AccountId = acc.Id;
        cha.Name = "Test";

        bool success = db.CreateCharacter(cha);
        Assert.True(success);

        List<Character> chas = db.SelectCharactersByAccountId(acc.Id);
        Assert.NotEmpty(chas);
        Assert.True(chas[0].Name == "Test");
    }

    private SQLiteDb CreateDb(int testId)
    {
        string sqliteFolder = Path.Combine(Util.ExecutingDirectory(), "Files/SQLite");
        string sqLitePath = Path.Combine(sqliteFolder, $"TEST_{testId}_db.v{SQLiteDb.Version}.sqlite");
        if (File.Exists(sqLitePath))
        {
            File.Delete(sqLitePath);
        }

        SQLiteDb db = new SQLiteDb(sqLitePath);
        if (db.CreateDatabase())
        {
            ScriptRunner scriptRunner = new ScriptRunner(db);
            scriptRunner.Run(Path.Combine(sqliteFolder, "schema_sqlite.sql"));
        }

        return db;
    }
}