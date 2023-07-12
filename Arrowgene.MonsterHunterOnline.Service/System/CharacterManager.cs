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

    public bool CreateCharacter(Client client, RoleCreateInfo req)
    {
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

        return _database.CreateCharacter(character);
    }

    /// <summary>
    /// Sends RoleBaseInfo to client.
    /// Since this structure has two possible ids
    /// it is required to provide the desired response as second argument.
    /// Either CsProtoResponse.ListRoleRsp or CsProtoResponse.CreateRoleRsp
    /// </summary>
    public void SendRoleList(Client client, CsProtoStructurePacket<ListRoleRsp> rsp)
    {
        Logger.Trace(client, "SendRoleList");
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
            role.FacialInfo = character.FacialInfo;
            role.StarLevel = character.StarLevel;
            role.HrLevel = character.HrLevel;
            role.SoulStoneLv = character.SoulStoneLv;
            rsp.Structure.RoleList.Add(role);
        }

        client.SendCsProtoStructurePacket(rsp);
    }
}