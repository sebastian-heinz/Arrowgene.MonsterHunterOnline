using System.Collections.Generic;
using Arrowgene.MonsterHunterOnline.Service.CsProto;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.System.ChatSystem.Command.Commands
{
    /// <summary>
    /// test random stuff
    /// </summary>
    public class TestCommand : ChatCommand
    {
        public override AccountType Account => AccountType.Admin;
        public override string Key => "test";
        public override string HelpText => "usage: `/test` - test random stuff";

        public override void Execute(string[] command, Client client, ChatMessage message, List<ChatMessage> responses)
        {
            //client.SendCsPacket(NewCsPacket.SelectHuntingBagRsp(new CSSelectHuntingBagRsp()));
           
            client.SendCsPacket(NewCsPacket.HunterStarInitNtf(new CSHunterStarInitNtf()
            {
                Entry = 0
            }));

            
            
        }
    }
}