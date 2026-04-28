using System.Net;
using System.Runtime.Serialization;
using Arrowgene.Networking.SAEAServer;

namespace Arrowgene.MonsterHunterOnline.Service
{
    [DataContract]
    public class Setting
    {
        [DataMember(Order = 2)] public string Name { get; set; }

        [IgnoreDataMember] public IPAddress ListenIpAddress { get; set; }

        [DataMember(Name = "ListenIpAddress", Order = 5)]
        public string DataListenIpAddress
        {
            get => ListenIpAddress.ToString();
            set => ListenIpAddress = string.IsNullOrEmpty(value) ? null : IPAddress.Parse(value);
        }

        [DataMember(Order = 6)] public ushort ServerPort { get; set; }
        [DataMember(Order = 7)] public ushort BattleServerPort { get; set; }
        [DataMember(Order = 20)] public int LogLevel { get; set; }
        [DataMember(Order = 21)] public string LogFilePath { get; set; }

        [DataMember(Order = 100)] public TcpServerSettings TcpServerSettings { get; set; }
        
        [DataMember(Order = 120)] public int ConsumerQueueCapacityPerLane { get; set; }

        public Setting()
        {
            Name = "Server";
            ListenIpAddress = IPAddress.Any;
            ServerPort = 8142;
            BattleServerPort = 8143;
            LogLevel = 0;
            LogFilePath = "/Users/shiba/dev/mho_decomp/server.log";
            TcpServerSettings = new TcpServerSettings();
            TcpServerSettings.OrderingLaneCount = 1;
            TcpServerSettings.MaxQueuedSendBytes = 8388608;
            ConsumerQueueCapacityPerLane = 100000;
        }

        public Setting(Setting setting)
        {
            Name = setting.Name;
            ListenIpAddress = setting.ListenIpAddress;
            ServerPort = setting.ServerPort;
            BattleServerPort = setting.BattleServerPort;
            LogLevel = setting.LogLevel;
            LogFilePath = setting.LogFilePath;
            TcpServerSettings = new TcpServerSettings(setting.TcpServerSettings);
            ConsumerQueueCapacityPerLane = setting.ConsumerQueueCapacityPerLane;
        }
    }
}