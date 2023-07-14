using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.TqqApi.Structure;

public interface ITpduExtAuthData : IStructure
{
    public uint Uin { get; set; }
}