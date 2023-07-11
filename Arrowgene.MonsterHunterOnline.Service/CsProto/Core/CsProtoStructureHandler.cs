using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

public abstract class CsProtoStructureHandler<TStructure> : ICsProtoHandler where TStructure : class, IStructure, new()
{
    public abstract CS_CMD_ID Cmd { get; }

    public abstract void Handle(Client client, TStructure req);

    public void Handle(Client client, CsProtoPacket packet)
    {
        TStructure structure = new TStructure();
        structure.Read(packet.NewBuffer());
        Handle(client, structure);
    }
}