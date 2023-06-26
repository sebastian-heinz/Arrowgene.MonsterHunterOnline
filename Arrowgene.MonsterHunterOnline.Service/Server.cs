using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;
using Arrowgene.MonsterHunterOnline.Service.ScaleformAmp;
using Arrowgene.MonsterHunterOnline.Service.TQQApi;
using Arrowgene.MonsterHunterOnline.Service.TQQApi.Handler;
using Arrowgene.Networking.Tcp.Server.AsyncEvent;
using Arrowgene.Networking.Udp;
using Arrowgene.WebServer;
using Arrowgene.WebServer.Route;
using Arrowgene.WebServer.Server.Kestrel;
using WebRequest = Arrowgene.WebServer.WebRequest;
using WebResponse = Arrowgene.WebServer.WebResponse;

namespace Arrowgene.MonsterHunterOnline.Service
{
    public class Server
    {
        private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(Server));

        private readonly TpduConsumer _tpduConsumer;
        private readonly CsProtoOverTpduConsumer _csProtoOverTpduConsumer;
        private readonly CsProtoConsumer _battleServerConsumer;
        private readonly AsyncEventServer _server;
        private readonly AsyncEventServer _battleServer;
        private readonly Setting _setting;
        private readonly AmpClient _ampClient;

        public Server(Setting setting)
        {
            _setting = new Setting(setting);

            _battleServerConsumer = new CsProtoConsumer(_setting);
            _csProtoOverTpduConsumer = new CsProtoOverTpduConsumer(_setting);
            
            List<ICsProtoHandler> handlers = new List<ICsProtoHandler>();
            handlers.Add(new C2SCmdActivityAddSecretQuestHandler());
            handlers.Add(new C2SCmdPetRngHandler());
            handlers.Add(new C2SCmdSActivityListReqHandler());
            handlers.Add(new C2SCmdShopRefreshShopsHandler());
            handlers.Add(new CsCmdBattleActorFifoSyncHandler());
            handlers.Add(new CsCmdCheckVersionHandler());
            handlers.Add(new CsCmdClientSendLogHandler());
            handlers.Add(new CsCmdDataLoadHandler());
            handlers.Add(new CsCmdDragonBoxDetailReqHandler());
            handlers.Add(new CsCmdFileCheckHandler());
            handlers.Add(new CsCmdFriendsOnlineReqHandler());
            handlers.Add(new CsCmdGiftBagGroupStateReqHandler());
            handlers.Add(new CsCmdInstanceVerifyReq());
            handlers.Add(new CsCmdItemReBuildLimitDataHandler());
            handlers.Add(new CsCmdMartGoodsListReqHandler());
            handlers.Add(new CsCmdMultiNetIpInfoHandler());
            handlers.Add(new CsCmdPlayerExtNotifyHandler());
            handlers.Add(new CsCmdSelectRoleHandler());
            handlers.Add(new CsCmdSystemEncryptData(_battleServerConsumer));
            handlers.Add(new CsCmdSystemPkgTimerRecordHandler());
            handlers.Add(new CsCmdSystemTransAntiDataHandler());
            handlers.Add(new CsCmdTeamInfoGetReqHandler());

            foreach (ICsProtoHandler handler in handlers)
            {
                _battleServerConsumer.AddHandler(handler);
                _csProtoOverTpduConsumer.AddHandler(handler);
            }
            

            _tpduConsumer = new TpduConsumer(_setting);
            _tpduConsumer.AddHandler(new TpduCmdAuthHandler());
            _tpduConsumer.AddHandler(new TpduCmdSynAckHandler());
            _tpduConsumer.AddHandler(new TpduCmdNoneHandler(_csProtoOverTpduConsumer));
            _tpduConsumer.AddHandler(new TpduCmdCloseHandler());

            _server = new AsyncEventServer(
                _setting.ListenIpAddress,
                _setting.ServerPort,
                _tpduConsumer,
                _setting.SocketSettings
            );

            _battleServer = new AsyncEventServer(
                _setting.ListenIpAddress,
                _setting.BattleServerPort,
                _battleServerConsumer,
                _setting.SocketSettings
            );

            // OTHER SERVICES - START
            // For now all other services are not handled, as there seems to be no need to.

            // port 7533/UDP and 7534/TCP are most likely related to Scaleform AMP protocol for client perf analysis.
            //_ampClient = new AmpClient();
            //_ampClient.Start();

            // client hits a few web routes but hard to know original content
            WebSetting webSetting = new WebSetting();
            WebService webService = new WebService(new KestrelWebServer(webSetting));
            webService.AddRoute(new WebRouter());
            //webService.Start();

            // this seems to be for anti cheat stuff
            // tqos.gamesafe.qq.com
            UdpSocket gamesafeSocket = new UdpSocket(DefaultMaxPayloadSizeBytes);
            gamesafeSocket.ReceivedPacket += GamesafeSocketOnReceivedPacket;
            IPEndPoint gamesafeSocketIpEndPoint = new IPEndPoint(IPAddress.Any, 8081);
            // gamesafeSocket.StartListen(gamesafeSocketIpEndPoint);

            // this seems to be a logging server, receiving system/game logs
            // ied-tqos.qq.com 
            UdpSocket iedSocket = new UdpSocket(DefaultMaxPayloadSizeBytes);
            iedSocket.ReceivedPacket += IedSocketOnReceivedPacket;
            IPEndPoint iedSocketSocketIpEndPoint = new IPEndPoint(IPAddress.Any, 8000);
            //iedSocket.StartListen(iedSocketSocketIpEndPoint);

            // OTHER SERVICES - END
        }


        private class WebRouter : WebRoute
        {
            public override string Route => "/mho/*";

            public override async Task<WebResponse> Post(WebRequest request)
            {
                string body = await request.ReadStringAsync();

                Logger.Debug(request.ToString());
                Logger.Debug(body);

                WebResponse response = new WebResponse();
                response.StatusCode = 200;
                await response.WriteAsync("");
                return response;
            }
        }

        public const int DefaultMaxPayloadSizeBytes = 1024;

        private void IedSocketOnReceivedPacket(object sender, ReceivedUdpPacketEventArgs e)
        {
            Logger.Debug("Te " + e.RemoteIpEndPoint + Environment.NewLine + Util.HexDump(e.Received));
        }

        private void GamesafeSocketOnReceivedPacket(object sender, ReceivedUdpPacketEventArgs e)
        {
            Logger.Debug("GS " + e.RemoteIpEndPoint + Environment.NewLine + Util.HexDump(e.Received));
        }

        public void Start()
        {
            _server.Start();
            _battleServer.Start();
        }

        public void Stop()
        {
            _server.Stop();
            _battleServer.Stop();
        }
    }
}