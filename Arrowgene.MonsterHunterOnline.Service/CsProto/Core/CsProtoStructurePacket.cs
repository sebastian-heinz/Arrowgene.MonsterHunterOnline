using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Core
{
    public class CsCsProtoStructurePacket : CSICsCsProtoStructurePacket
    {
        public CsCsProtoStructurePacket(CS_CMD_ID cmd, ICsStructure csStructure)
        {
            Cmd = cmd;
            _structure = csStructure;
        }

        protected readonly ICsStructure _structure;

        public CS_CMD_ID Cmd { get; }
        public ICsStructure CsStructure => _structure;

        public void WriteCs(IBuffer buffer)
        {
            _structure.WriteCs(buffer);
        }

        public void ReadCs(IBuffer buffer)
        {
            _structure.ReadCs(buffer);
        }
    }

    public class CsCsProtoStructurePacket<TStructure> : CsCsProtoStructurePacket
        where TStructure : ICsStructure, new()
    {
        public new TStructure Structure => (TStructure)_structure;

        public CsCsProtoStructurePacket(CS_CMD_ID cmd) : base(cmd, new TStructure())
        {
        }
    }
}