using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// Instance initialize info
    /// </summary>
    public class InstanceInitInfo : Structure, ICsStructure
    {
        public InstanceInitInfo()
        {
            BattleGroundId = 0;
            LevelId = 0;
            CreateMaxPlayerCount = 0;
            GameMode = GameMode.Standard;
            TimeType = TimeType.Noon;
            WeatherType = WeatherType.Sunny;
            Time = 0.0f;
            LevelRandSeed = 0;
            WarningFlag = 0;
            CreatePlayerMaxLv = 0;
        }

        /// <summary>
        /// BattleGround ID
        /// </summary>
        public int BattleGroundId { get; set; }

        /// <summary>
        /// Level id
        /// </summary>
        public int LevelId { get; set; }

        /// <summary>
        /// 创建副本的玩家数量
        /// </summary>
        public int CreateMaxPlayerCount { get; set; }

        public GameMode GameMode { get; set; }

        /// <summary>
        /// 关卡时间
        /// </summary>
        public TimeType TimeType { get; set; }

        /// <summary>
        /// 关卡天气
        /// </summary>
        public WeatherType WeatherType { get; set; }

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

        public  void WriteCs(IBuffer buffer)
        {
            WriteInt32(buffer, BattleGroundId);
            WriteInt32(buffer, LevelId);
            WriteInt32(buffer, CreateMaxPlayerCount);
            WriteInt32(buffer, (int)GameMode);
            WriteInt32(buffer, (int)TimeType);
            WriteInt32(buffer, (int)WeatherType);
            WriteFloat(buffer, Time);
            WriteInt32(buffer, LevelRandSeed);
            WriteByte(buffer, WarningFlag);
            WriteInt32(buffer, CreatePlayerMaxLv);
        }

        public void ReadCs(IBuffer buffer)
        {
            BattleGroundId = ReadInt32(buffer);
            LevelId = ReadInt32(buffer);
            CreateMaxPlayerCount = ReadInt32(buffer);
            GameMode = (GameMode)ReadInt32(buffer);
            TimeType = (TimeType)ReadInt32(buffer);
            WeatherType = (WeatherType)ReadInt32(buffer);
            Time = ReadFloat(buffer);
            LevelRandSeed = ReadInt32(buffer);
            WarningFlag = ReadByte(buffer);
            CreatePlayerMaxLv = ReadInt32(buffer);
        }
    }
}