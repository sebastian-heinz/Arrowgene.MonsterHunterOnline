using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Packets;

/// <summary>
/// 副本初始化
/// </summary>
public class InstanceInitNtf : CsPacket
{
    public override CsProtoCmd Cmd => CsProtoCmd.CS_CMD_INSTANCE_INIT_NTF;

    public InstanceInitNtf()
    {
        CsInstanceInitInfo = new CsInstanceInitInfo();
    }

    public CsInstanceInitInfo CsInstanceInitInfo { get; set; }

    public override void Write(IBuffer buffer)
    {
        CsInstanceInitInfo.Write(buffer);
    }
}