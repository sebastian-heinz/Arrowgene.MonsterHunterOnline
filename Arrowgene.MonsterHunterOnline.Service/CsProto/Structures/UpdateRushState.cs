using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{

    public class UpdateRushState : Structure
    {
        public UpdateRushState()
        {
            Rush = 0;
            Type = 0;
        }

        public int Rush { get; set; }

        public int Type { get; set; }


        public override void Write(IBuffer buffer)
        {
            WriteInt32(buffer, Rush);
            WriteInt32(buffer, Type);
           
        }

        public override void Read(IBuffer buffer)
        {
            Rush = ReadInt32(buffer);
            Type = ReadInt32(buffer);
        }
    }
}