using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Core
{
    public class CsProtoStructurePacket : ICsProtoStructurePacket
    {
        public CsProtoStructurePacket(CS_CMD_ID cmd, IStructure structure)
        {
            Cmd = cmd;
            _structure = structure;
        }

        protected readonly IStructure _structure;

        public CS_CMD_ID Cmd { get; }
        public IStructure Structure => _structure;

        public void Write(IBuffer buffer)
        {
            _structure.Write(buffer);
        }

        public void Read(IBuffer buffer)
        {
            _structure.Read(buffer);
        }
    }

    public class CsProtoStructurePacket<TStructure> : CsProtoStructurePacket
        where TStructure : IStructure, new()
    {
        public new TStructure Structure => (TStructure)_structure;

        public CsProtoStructurePacket(CS_CMD_ID cmd) : base(cmd, new TStructure())
        {
        }
    }
}