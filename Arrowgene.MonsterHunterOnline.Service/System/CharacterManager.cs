using System.Collections.Generic;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.Database;

namespace Arrowgene.MonsterHunterOnline.Service.System;

public class CharacterManager
{
    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(CharacterManager));

    private readonly IDatabase _database;

    public CharacterManager(IDatabase database)
    {
        _database = database;
    }

    public bool ModifyCharacter(Client client, ModifyFaceReq req)
    {
        Character character = _database.SelectCharacterByRoleIndex(client.Account.Id, (byte)req.RoleIndex);
        if (character == null)
        {
            return false;
        }

        character.AccountId = client.Account.Id;
        //req.Uin;
        //req.DbId;
        //req.ChangeGender;
        character.Gender = (byte)req.Gender;

        character.FaceId = req.FaceId;
        character.HairId = req.HairId;
        character.UnderclothesId = req.UnderclothesId;
        character.SkinColor = req.SkinColor;
        character.HairColor = req.HairColor;
        character.InnerColor = req.InnerColor;
        character.EyeBall = req.EyeBall;
        character.EyeColor = req.EyeColor;
        character.FaceTattooIndex = req.FaceTattooIndex;
        character.FaceTattooColor = req.FaceTattooColor;
        for (int i = 0; i < CsProtoConstant.CS_MAX_FACIALINFO_COUNT; i++)
        {
            character.FacialInfo[i] = req.FacialInfo[i];
        }

        return _database.UpdateCharacter(character);
    }

    public bool DeleteCharacter(Client client, byte roleIndex)
    {
        // TODO it looks like there is a grace period, but not sure about the mechanic, for now just delete it
        CsProtoStructurePacket<DeleteRoleRsp> rsp = CsProtoResponse.DeleteRoleRsp;
        rsp.Structure.RoleState = 0;
        // 2 = removing role failed -> login protection
        // 3 = the leader cant delete the guild
        // 4 = you are the leader, you can not transfer the role
        rsp.Structure.RoleStateEndLeftTime = 0;
        client.SendCsProtoStructurePacket(rsp);

        Character character = _database.SelectCharacterByRoleIndex(client.Account.Id, roleIndex);
        if (character == null)
        {
            return false;
        }

        if (!_database.DeleteCharacter(character.Id))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Creates a new character with default values
    /// </summary>
    public Character CreateCharacter(Client client, RoleCreateInfo req)
    {
        Logger.Trace(client, "CreateCharacter");
        Character character = new Character();
        character.AccountId = client.Account.Id;
        character.Index = 0;
        character.Gender = req.Gender;
        character.Level = 1;
        character.Name = req.Name;
        character.RoleState = 0;
        character.RoleStateEndLeftTime = 0;
        character.AvatarSetId = 0;
        character.FaceId = req.FaceId;
        character.HairId = req.HairId;
        character.UnderclothesId = req.UnderclothesId;
        character.SkinColor = req.SkinColor;
        character.HairColor = req.HairColor;
        character.InnerColor = req.InnerColor;
        character.EyeBall = req.EyeBall;
        character.EyeColor = req.EyeColor;
        character.FaceTattooIndex = req.FaceTattooIndex;
        character.FaceTattooColor = req.FaceTattooColor;
        for (int i = 0; i < CsProtoConstant.CS_MAX_FACIALINFO_COUNT; i++)
        {
            character.FacialInfo[i] = req.FacialInfo[i];
        }

        character.StarLevel = "";
        character.HrLevel = 0;
        character.SoulStoneLv = 0;
        character.HideHelm = 0;
        character.HideFashion = 0;
        character.HideSuite = 0;

        if (!_database.CreateCharacter(character))
        {
            return null;
        }

        return character;
    }

    /// <summary>
    /// Adds all roles for given client to `RoleList`
    /// </summary>
    public void PopulateRoleList(Client client, ListRoleRsp rsp)
    {
        Logger.Trace(client, "PopulateRoleList");
        List<Character> characters = _database.SelectCharactersByAccountId(client.Account.Id);
        foreach (Character character in characters)
        {
            RoleBaseInfo role = new RoleBaseInfo();
            role.RoleId = character.Id;
            role.RoleIndex = character.Index;
            role.Name = character.Name;
            role.Gender = character.Gender;
            role.Level = (int)character.Level;
            role.RoleState = character.RoleState;
            role.RoleStateEndLeftTime = character.RoleStateEndLeftTime;
            role.AvatarSetId = character.AvatarSetId;
            role.FaceId = character.FaceId;
            role.HairId = character.HairId;
            role.UnderclothesId = character.UnderclothesId;
            role.SkinColor = character.SkinColor;
            role.HairColor = character.HairColor;
            role.InnerColor = character.InnerColor;
            role.EyeBall = character.EyeBall;
            role.EyeColor = character.EyeColor;
            role.FaceTattooIndex = character.FaceTattooIndex;
            role.FaceTattooColor = character.FaceTattooColor;
            // TODO reference table
            // role.Equip = character.Equip; 
            role.HideHelm = character.HideHelm;
            role.HideFashion = character.HideFashion;
            role.HideSuite = character.HideSuite;
            for (int i = 0; i < CsProtoConstant.CS_MAX_FACIALINFO_COUNT; i++)
            {
                role.FacialInfo[i] = character.FacialInfo[i];
            }

            role.StarLevel = character.StarLevel;
            role.HrLevel = character.HrLevel;
            role.SoulStoneLv = character.SoulStoneLv;
            rsp.RoleList.Add(role);
        }
    }

    public Character GetCharacter(uint accountId, byte roleIndex)
    {
        return _database.SelectCharacterByRoleIndex(accountId, roleIndex);
    }

    public void PopulatePlayerInitInfo(Client client, Character character, PlayerInitInfo structure)
    {
        structure.AccountId = client.Account.Id;
        // TODO if this is entityId then need to be unique across NPC/Monster/Char
        structure.NetId = (int)character.Id;
        structure.DbId = character.Id;
        // for now set all to 1
        structure.SessionId = 1;
        structure.WorldId = 1;
        structure.ServerId = 1;
        structure.WorldSvrId = 1;
        //
        structure.ServerTime = 0;
        structure.IsReConnect = 0;
        structure.Name = character.Name;
        structure.Gender = character.Gender;
        structure.IsGm = 0;
        //structure.Pose;
        structure.ParentEntityGuid = 0;
        structure.AvatarSetId = 0;
        structure.Faction = 0;
        structure.RandSeed = 0;
        structure.Weapon = 0;
        structure.LastLoginTime = 0;
        structure.CreateTime = (uint)Util.GetUnixTime(character.Created);
        structure.StoreSize = 0;
        structure.NormalSize = 0;
        structure.MaterialStoreSize = 0;
        structure.FirstEnterLevel = 0;
        structure.FirstEnterMap = 0;
        structure.PvpPrepareStageState = 0;
        structure.CurPlayerUsedCatCarCount = 0;
        structure.CatCuisineId = 0;
        structure.CatCuisineLevel = 0;
        structure.CatCuisineBuffs = 0;
        structure.CatCuisineLastTm = 0;
        structure.CatCuisineFormulaCount = 0;
        structure.IsSpectating = 0;
        structure.DragonShopBox = 0;
        structure.CanGetRewarded = 0;
        // TODO figure out attr and other binary blobs
    }
}