namespace Arrowgene.MonsterHunterOnline.Service.TqqApi;

public enum TConnSecEnc : int
{
    TCONN_SEC_NONE = 0,
    TCONN_SEC_TEA = 1,
    TCONN_SEC_QQ = 2,
    TCONN_SEC_AES = 3,
    TCONN_SEC_AES2 = 4, // 与TCONN_SEC_AES算法不同的是修改了数据填充算法
}