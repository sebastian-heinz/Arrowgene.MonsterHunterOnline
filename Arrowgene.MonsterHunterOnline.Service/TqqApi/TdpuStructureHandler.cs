using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.TqqApi;

public abstract class TdpuStructureHandler<TStructure> : ITpduHandler where TStructure : class, ICsStructure, new()
{
    public abstract TpduCmd Cmd { get; }

    public abstract void Handle(Client client, TStructure req);

    public void Handle(Client client, TpduPacket packet)
    {
        TStructure structure = new TStructure();
        structure.ReadCs(packet.NewHeaderExtBuffer());
        Handle(client, structure);
    }
}