using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Core
{
    // TODO remove
    public class CsTPacket<T> : CsPacket where T : IStructure
    {
        public CsTPacket(CS_CMD_ID cmd, T structure)
        {
            Cmd = cmd;
            Structure = structure;
        }

        public T Structure { get; }

        public override CS_CMD_ID Cmd { get; }

        public override void Write(IBuffer buffer)
        {
            Structure.Write(buffer);
        }
    }
}