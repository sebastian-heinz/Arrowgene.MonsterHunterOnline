using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 新手引导步骤状态
/// </summary>
public class CSGuideStep : IStructure
{
    public CSGuideStep()
    {
        StepId = 0;
        StepState = 0;
    }
    
    public byte StepId { get; set; }
    
    public byte StepState { get; set; }


    public void Write(IBuffer buffer)
    {
        buffer.WriteByte(StepId);
        buffer.WriteByte(StepState);
    }
}