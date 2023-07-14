using System.IO;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;
using Arrowgene.MonsterHunterOnline.Service.Database;
using Arrowgene.MonsterHunterOnline.Service.Database.Sql;
using Arrowgene.MonsterHunterOnline.Service.System;
using Arrowgene.MonsterHunterOnline.Service.System.Chat;
using Arrowgene.MonsterHunterOnline.Service.TqqApi;
using Arrowgene.MonsterHunterOnline.Service.TqqApi.Handler;
using Arrowgene.MonsterHunterOnline.Service.Web;
using Arrowgene.Networking.Tcp.Server.AsyncEvent;

namespace Arrowgene.MonsterHunterOnline.Service
{
    public class Server
    {
        private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(Server));

        private readonly TpduConsumer _tpduConsumer;
        private readonly CsProtoPacketHandler _csProtoPacketHandler;
        private readonly CsProtoConsumer _battleServerConsumer;
        private readonly AsyncEventServer _server;
        private readonly AsyncEventServer _battleServer;
        private readonly MhoWebServer _webServer;

        public Server(Setting setting)
        {
            Setting = new Setting(setting);

            Database = CreateDatabase();

            _tpduConsumer = new TpduConsumer(Setting);
            _csProtoPacketHandler = new CsProtoPacketHandler(Setting);
            _battleServerConsumer = new CsProtoConsumer(Setting, _csProtoPacketHandler);
            _webServer = new MhoWebServer();
            _server = new AsyncEventServer(
                Setting.ListenIpAddress,
                Setting.ServerPort,
                _tpduConsumer,
                Setting.SocketSettings
            );
            _battleServer = new AsyncEventServer(
                Setting.ListenIpAddress,
                Setting.BattleServerPort,
                _battleServerConsumer,
                Setting.SocketSettings
            );
            Chat = new ChatSystem();
            CharacterManager = new CharacterManager(Database);

            LoadPacketHandler();

            // TODO remove hack
            PlayerState.Server = this;
        }

        public Setting Setting { get; }
        public ChatSystem Chat { get; }
        public CharacterManager CharacterManager { get; }
        public IDatabase Database { get; }

        private IDatabase CreateDatabase()
        {
            string sqliteFolder = Path.Combine(Util.ExecutingDirectory(), "Files/SQLite");

            string sqLitePath = Path.Combine(sqliteFolder, $"db.v{SQLiteDb.Version}.sqlite");
            SQLiteDb db = new SQLiteDb(sqLitePath);
            if (db.CreateDatabase())
            {
                ScriptRunner scriptRunner = new ScriptRunner(db);
                scriptRunner.Run(Path.Combine(sqliteFolder, "schema_sqlite.sql"));
            }

            return db;
        }

        private void LoadPacketHandler()
        {
            // old handler
            _csProtoPacketHandler.AddHandler(new C2SCmdActivityAddSecretQuestHandler());
            _csProtoPacketHandler.AddHandler(new C2SCmdPetRngHandler());
            _csProtoPacketHandler.AddHandler(new C2SCmdSActivityListReqHandler());
            _csProtoPacketHandler.AddHandler(new C2SCmdShopRefreshShopsHandler());
            _csProtoPacketHandler.AddHandler(new CS2CmdDemonTrailGetLevelsPassTimeReq());
            _csProtoPacketHandler.AddHandler(new CS2CmdDemonTrailGetLevelsReqHandler());
            _csProtoPacketHandler.AddHandler(new CsCmdChangeTownInstanceReqHandler());
            _csProtoPacketHandler.AddHandler(new CsCmdChatBroadcastReqHandler(Chat));
            _csProtoPacketHandler.AddHandler(new CsCmdChatEncryptData(_csProtoPacketHandler, Chat));
            _csProtoPacketHandler.AddHandler(new CsCmdCheckVersionHandler());
            _csProtoPacketHandler.AddHandler(new CsCmdClientSendLogHandler());
            _csProtoPacketHandler.AddHandler(new CsCmdDragonBoxDetailReqHandler());
            _csProtoPacketHandler.AddHandler(new CsCmdFileCheckHandler());
            _csProtoPacketHandler.AddHandler(new CsCmdFriendsOnlineReqHandler());
            _csProtoPacketHandler.AddHandler(new CsCmdGiftBagGroupStateReqHandler());
            _csProtoPacketHandler.AddHandler(new CsCmdInstanceVerifyReq());
            _csProtoPacketHandler.AddHandler(new CsCmdItemReBuildLimitDataHandler());
            _csProtoPacketHandler.AddHandler(new CsCmdMailUnreadGetReqHandler());
            _csProtoPacketHandler.AddHandler(new CsCmdMartGoodsListReqHandler());
            _csProtoPacketHandler.AddHandler(new CsCmdPlayerExtNotifyHandler());
            _csProtoPacketHandler.AddHandler(new CsCmdSystemEncryptData(_csProtoPacketHandler));
            _csProtoPacketHandler.AddHandler(new CsCmdSystemPkgTimerRecordHandler());
            _csProtoPacketHandler.AddHandler(new CsCmdSystemTransAntiDataHandler());
            _csProtoPacketHandler.AddHandler(new CsCmdTeamInfoGetReqHandler());
            _csProtoPacketHandler.AddHandler(new CsCmdVipServiceExpireReqHandler());

            // new handler
            _csProtoPacketHandler.AddHandler(new BattleActorBeginMoveHandler());
            _csProtoPacketHandler.AddHandler(new BattleActorFifoSyncHandler());
            _csProtoPacketHandler.AddHandler(new BattleActorIdleMoveHandler());
            _csProtoPacketHandler.AddHandler(new BattleActorMoveStateHandler());
            _csProtoPacketHandler.AddHandler(new CreateRoleReqHandler(CharacterManager));
            _csProtoPacketHandler.AddHandler(new DataLoadHandler(CharacterManager));
            _csProtoPacketHandler.AddHandler(new DeleteRoleReqHandler(CharacterManager));
            _csProtoPacketHandler.AddHandler(new EnterLevelNtfHandler(CharacterManager));
            _csProtoPacketHandler.AddHandler(new ModifyFaceReqHandler(CharacterManager));
            _csProtoPacketHandler.AddHandler(new MultiNetIpInfoHandler(CharacterManager));
            _csProtoPacketHandler.AddHandler(new ReselectRoleReqHandler(CharacterManager));
            _csProtoPacketHandler.AddHandler(new SelectRoleHandler(CharacterManager));
            _csProtoPacketHandler.AddHandler(new ServerActorFifoSyncAck());
            _csProtoPacketHandler.AddHandler(new WorldAccountReqHandler());


            _tpduConsumer.AddHandler(new TdpuCmdRelay(Database));
            _tpduConsumer.AddHandler(new TpduCmdAuthHandler(Database));
            _tpduConsumer.AddHandler(new TpduCmdSynAckHandler());
            _tpduConsumer.AddHandler(new TpduCmdNoneHandler(_csProtoPacketHandler));
            _tpduConsumer.AddHandler(new TpduCmdCloseHandler());
        }

        public void Start()
        {
            _webServer.Start();
            _server.Start();
            _battleServer.Start();
        }

        public void Stop()
        {
            _webServer.Stop();
            _server.Stop();
            _battleServer.Stop();
        }
    }
}