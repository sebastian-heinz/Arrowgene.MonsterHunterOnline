using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class CsInstanceInitInfo : IStructure
{
    public CsInstanceInitInfo()
    {
        BattleGroundId = 0;
        LevelId = 0;
        CreateMaxPlayerCount = 0;
        GameMode = 0;
        TimeType = 0;
        WeatherType = 0;
        Time = 0;
        LevelRandSeed = 0;
        WarningFlag = 0;
        CreatePlayerMaxLv = 0;
    }

    public int BattleGroundId { get; set; }
    public int LevelId { get; set; }

    /// <summary>
    /// 创建副本的玩家数量
    /// </summary>
    public int CreateMaxPlayerCount { get; set; }

    public int GameMode { get; set; }

    /// <summary>
    /// 关卡时间
    /// </summary>
    public int TimeType { get; set; }

    /// <summary>
    /// 关卡天气
    /// </summary>
    public int WeatherType { get; set; }

    public float Time { get; set; }

    /// <summary>
    /// 关卡内数据随机种子，特定系统专用
    /// </summary>
    public int LevelRandSeed { get; set; }

    /// <summary>
    /// 是否是warning
    /// </summary>
    public byte WarningFlag { get; set; }

    /// <summary>
    /// 副本的玩家最大等级
    /// </summary>
    public int CreatePlayerMaxLv { get; set; }


    public void Write(IBuffer buffer)
    {
        buffer.WriteInt32(BattleGroundId, Endianness.Big);
        buffer.WriteInt32(LevelId, Endianness.Big);
        buffer.WriteInt32(CreateMaxPlayerCount, Endianness.Big);
        buffer.WriteInt32(GameMode, Endianness.Big);
        buffer.WriteInt32(TimeType, Endianness.Big);
        buffer.WriteInt32(WeatherType, Endianness.Big);
        buffer.WriteFloat(Time, Endianness.Big);
        buffer.WriteInt32(LevelRandSeed, Endianness.Big);
        buffer.WriteByte(WarningFlag);
        buffer.WriteInt32(CreatePlayerMaxLv, Endianness.Big);
    }
}