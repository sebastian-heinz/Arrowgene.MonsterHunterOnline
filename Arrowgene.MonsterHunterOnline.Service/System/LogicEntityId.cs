namespace Arrowgene.MonsterHunterOnline.Service.System;

public struct LogicEntityId
{
    private uint _raw;

    const int Sz0 = 4, Loc0 = 0, Mask0 = ((1 << Sz0) - 1) << Loc0;
    const int Sz1 = 28, Loc1 = Loc0 + Sz0, Mask1 = ((1 << Sz1) - 1) << Loc1;

    public uint Id
    {
        get => _raw;
        set => _raw = value;
    }

    public LogicEntityType Type
    {
        get { return (LogicEntityType)((uint)(_raw & Mask0) >> Loc0); }
        set { _raw = (uint)(_raw & ~Mask0 | ((uint)value << Loc0) & Mask0); }
    }

    public uint UniqueId
    {
        get { return (uint)(_raw & Mask1) >> Loc1; }
        set { _raw = (uint)(_raw & ~Mask1 | (value << Loc1) & Mask1); }
    }
}