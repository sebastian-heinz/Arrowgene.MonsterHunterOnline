using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 机密研究院数据同步 服务端->客户端
/// </summary>
public class S2CSecretResearchLabDataSynchronizationRsp : IStructure
{
    public S2CSecretResearchLabDataSynchronizationRsp()
    {
        ItemBoxList = new ItemBoxType[CsConstant.SECRET_RESEARCH_LAB_ITEM_BOX_MAX_LEN];
        for (int i = 0; i < CsConstant.SECRET_RESEARCH_LAB_ITEM_BOX_MAX_LEN; i++)
        {
            ItemBoxList[i] = new ItemBoxType();
        }
    }

    /// <summary>
    /// 抽奖盒子列表
    /// </summary>
    public ItemBoxType[] ItemBoxList { get; }


    public void Write(IBuffer buffer)
    {
        for (int i = 0; i < CsConstant.SECRET_RESEARCH_LAB_ITEM_BOX_MAX_LEN; i++)
        {
            ItemBoxList[i].Write(buffer);
        }
    }
}