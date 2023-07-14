using System;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;

namespace Arrowgene.MonsterHunterOnline.Service.System;

public class Character
{
    public Character()
    {
        FacialInfo = new short[CsProtoConstant.CS_MAX_FACIALINFO_COUNT];
        StarLevel = "";
        Name = "";

        // TODO hack for visuals
        IsSync = false;
    }

    public bool IsSync { get; set; }
    
    public uint Id { get; set; }
    public uint AccountId { get; set; }
    public byte Index { get; set; }
    public byte Gender { get; set; }
    public uint Level { get; set; }
    public string Name { get; set; }

    public int RoleState { get; set; }

    public uint RoleStateEndLeftTime { get; set; }

    public byte AvatarSetId { get; set; }

    public ushort FaceId { get; set; }

    public ushort HairId;

    public ushort UnderclothesId;

    public int SkinColor;

    public int HairColor;

    public int InnerColor;

    public int EyeBall;

    public int EyeColor;

    public int FaceTattooIndex;

    public int FaceTattooColor;

    public short[] FacialInfo { get; }

    public string StarLevel { get; set; }

    public int HrLevel { get; set; }

    public int SoulStoneLv { get; set; }

    public byte HideHelm { get; set; }

    public byte HideFashion { get; set; }

    public byte HideSuite { get; set; }
    public DateTime Created { get; set; }
}