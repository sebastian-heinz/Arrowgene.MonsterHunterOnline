using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Core
{
    public class CsProtoStructurePacket<TStructure> : ICsProtoStructurePacket
        where TStructure : IStructure, new()
    {
        public CsProtoStructurePacket(CS_CMD_ID cmd)
        {
            Cmd = cmd;
            Structure = new TStructure();
        }

        public TStructure Structure { get; }

        public CS_CMD_ID Cmd { get; }

        public void Write(IBuffer buffer)
        {
            Structure.Write(buffer);
        }

        public void Read(IBuffer buffer)
        {
            Structure.Read(buffer);
        }
    }
}