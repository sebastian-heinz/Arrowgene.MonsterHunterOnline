namespace Arrowgene.MonsterHunterOnline.Service.System;

public struct LogicEntityId
{
    private uint _raw;

    private const int UniqueIdBits = 28;
    private const int TypeBits = 4;
    private const int UniqueIdShift = 0;
    private const int TypeShift = UniqueIdBits;
    private const uint UniqueIdMask = ((1u << UniqueIdBits) - 1u) << UniqueIdShift;
    private const uint TypeMask = ((1u << TypeBits) - 1u) << TypeShift;

    public uint Id
    {
        get => _raw;
        set => _raw = value;
    }

    public LogicEntityType Type
    {
        get => (LogicEntityType)((_raw & TypeMask) >> TypeShift);
        set => _raw = (_raw & ~TypeMask) | (((uint)value << TypeShift) & TypeMask);
    }

    public uint UniqueId
    {
        get => (_raw & UniqueIdMask) >> UniqueIdShift;
        set => _raw = (_raw & ~UniqueIdMask) | ((value << UniqueIdShift) & UniqueIdMask);
    }
}
