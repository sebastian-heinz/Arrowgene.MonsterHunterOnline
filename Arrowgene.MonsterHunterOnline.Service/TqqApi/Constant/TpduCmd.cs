namespace Arrowgene.MonsterHunterOnline.Service.TqqApi;

public enum TpduCmd : byte
{
    TPDU_CMD_NONE = 0, // 通信包
    TPDU_CMD_CHGSKEY = 1, // 交换密钥(下行)
    TPDU_CMD_QUEINFO = 2, // 排队信息(下行)
    TPDU_CMD_AUTH = 3, // 签名请求信息(上行)
    TPDU_CMD_IDENT = 4, // 连接建立(下行)
    TPDU_CMD_PLAIN = 5, // 未加密通信包(下行)
    TPDU_CMD_RELAY = 6, // 重连请求信息(上行)
    TPDU_CMD_STOP = 7, // 服务器断开连接下发错误码
    TPDU_CMD_SYN = 8, // 连接握手信息(下行)
    TPDU_CMD_SYNACK = 9, // 三次握手请求(上行)
    TPDU_CMD_MBA_QUERYRSP = 10, // 查询密保返回结果(下行)
    TPDU_CMD_MBA_VERIFYREQ = 11, // 验证密保请求(上行)
    TPDU_CMD_MBA_VERIFYRSP = 12, // 下发密保问题检验结果(下行)
    TPDU_CMD_CLOSE = 13, // 客户端关闭连接通知消息(上行)
    TPDU_CMD_CLIENT_ADDR = 210, // TGC发送的客户端真实地址消息(上行)
}