using System.Collections.Generic;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.System.ChatSystem.Command.Commands
{
    /// <summary>
    /// Spawns enteties
    /// </summary>
    public class SpawnCommand : ChatCommand
    {
        private static readonly ServiceLogger Logger =
            LogProvider.Logger<ServiceLogger>(typeof(SpawnCommand));

        public override AccountType Account => AccountType.Admin;
        public override string Key => "spawn";
        public override string HelpText => "usage: `/spawn` - spawns entity";

        public override void Execute(string[] command, Client client, ChatMessage message, List<ChatMessage> responses)
        {
            
            
            
            CsCsProtoStructurePacket<EntityAppearNtfIdList> entityAppearNtfIdList = CsProtoResponse.EntityAppearNtfIdList;
            LogicEntityId leId = new LogicEntityId();
            leId.Type = LogicEntityType.MH_LETYPE_UNK_3;
            leId.Id = 39002;
            entityAppearNtfIdList.Structure.InitType = 0;
            entityAppearNtfIdList.Structure.LogicEntityId.Add(leId.Id);
            entityAppearNtfIdList.Structure.LogicEntityType.Add((uint)leId.Type);
            client.SendCsProtoStructurePacket(entityAppearNtfIdList);
            
            
            
            
        }
    }
}