using System.Collections.Generic;
using System.IO;
using Arrowgene.MonsterHunterOnline.Service;
using Arrowgene.MonsterHunterOnline.Service.Database;
using Arrowgene.MonsterHunterOnline.Service.Database.Sql;
using Arrowgene.MonsterHunterOnline.Service.System;
using Xunit;
using Xunit.Abstractions;

namespace Arrowgene.MonsterHunterOnline.Test.Database.Sql;

public class SQLiteDbTest : TestBase
{
    public SQLiteDbTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void TestSQLiteDbAccount_CreateAccount()
    {
        SQLiteDb db = CreateDb(1);

        Account acc = db.CreateAccount(1234, "pw");
        Assert.NotNull(acc);

        Account selectedAcc = db.SelectAccountByUin(1234);
        Assert.NotNull(selectedAcc);
    }

    [Fact]
    public void TestSQLiteDbCharacter_GetFreeCharacterIndex()
    {
        SQLiteDb db = CreateDb(2);

        Account acc = db.CreateAccount(1234, "pw");
        Assert.NotNull(acc);

        Character cha1 = new Character();
        cha1.AccountId = acc.Id;
        cha1.Name = "Test1";
        Assert.True(db.CreateCharacter(cha1));
        Assert.Equal(0, cha1.Index);
        Assert.True(db.GetFreeCharacterIndex(acc.Id, out byte index1));
        Assert.Equal(1, index1);

        Character cha2 = new Character();
        cha2.AccountId = acc.Id;
        cha2.Name = "Test2";
        Assert.True(db.CreateCharacter(cha2));
        Assert.Equal(1, cha2.Index);
        Assert.True(db.GetFreeCharacterIndex(acc.Id, out byte index2));
        Assert.Equal(2, index2);

        Character cha3 = new Character();
        cha3.AccountId = acc.Id;
        cha3.Name = "Test3";
        Assert.True(db.CreateCharacter(cha3));
        Assert.Equal(2, cha3.Index);
        Assert.True(db.GetFreeCharacterIndex(acc.Id, out byte index3));
        Assert.Equal(3, index3);

        Assert.True(db.DeleteCharacter(cha2.Id));

        Character cha4 = new Character();
        cha4.AccountId = acc.Id;
        cha4.Name = "Test4";
        Assert.True(db.CreateCharacter(cha4));
        Assert.Equal(1, cha4.Index);
        Assert.True(db.GetFreeCharacterIndex(acc.Id, out byte index4));
        Assert.Equal(3, index4);
    }

    [Fact]
    public void TestSQLiteDbCharacter_SelectCharactersByAccountId()
    {
        SQLiteDb db = CreateDb(3);

        Account acc = db.CreateAccount(1234, "pw");
        Assert.NotNull(acc);

        Character cha = new Character();
        cha.AccountId = acc.Id;
        cha.Name = "Test";
        Assert.True(db.CreateCharacter(cha));

        List<Character> chas = db.SelectCharactersByAccountId(acc.Id);
        Assert.NotEmpty(chas);
        Assert.True(chas[0].Name == "Test");
    }

    [Fact]
    public void TestSQLiteDbCharacter_SelectCharacterByRoleIndex()
    {
        SQLiteDb db = CreateDb(4);

        Account acc = db.CreateAccount(1234, "pw");
        Assert.NotNull(acc);

        Character cha = new Character();
        cha.AccountId = acc.Id;
        cha.Name = "Test";
        Assert.True(db.CreateCharacter(cha));

        Character selectedCharacter = db.SelectCharacterByRoleIndex(acc.Id, cha.Index);
        Assert.NotNull(selectedCharacter);
        Assert.True(cha.Name == selectedCharacter.Name);
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