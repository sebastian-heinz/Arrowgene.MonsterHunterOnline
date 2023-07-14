using System.Net;
using System.Runtime.Serialization;
using Arrowgene.Networking.Tcp.Server.AsyncEvent;

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

        [DataMember(Order = 100)] public AsyncEventSettings SocketSettings { get; set; }

        public Setting()
        {
            Name = "Server";
            ListenIpAddress = IPAddress.Any;
            ServerPort = 8142;
            BattleServerPort = 8143;
            LogLevel = 0;
            SocketSettings = new AsyncEventSettings();
            SocketSettings.MaxUnitOfOrder = 1;
        }

        public Setting(Setting setting)
        {
            Name = setting.Name;
            ListenIpAddress = setting.ListenIpAddress;
            ServerPort = setting.ServerPort;
            BattleServerPort = setting.BattleServerPort;
            LogLevel = setting.LogLevel;
            SocketSettings = new AsyncEventSettings(setting.SocketSettings);
        }
    }
}