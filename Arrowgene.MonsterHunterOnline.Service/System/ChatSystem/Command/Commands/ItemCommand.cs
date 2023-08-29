using System.Collections.Generic;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;

namespace Arrowgene.MonsterHunterOnline.Service.System.ChatSystem.Command.Commands
{
    /// <summary>
    /// retrieve item with specified id
    /// </summary>
    public class ItemCommand : ChatCommand
    {
        private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(ItemCommand));

        private ItemManager _itemManager;

        public ItemCommand(ItemManager itemManager)
        {
            _itemManager = itemManager;
        }

        public override AccountType Account => AccountType.Admin;
        public override string Key => "itm";
        public override string HelpText => "usage: `/itm [itemId]` - retrieve item with specified id";

        public override void Execute(string[] command, Client client, ChatMessage message, List<ChatMessage> responses)
        {
            if (command.Length <= 0)
            {
                // check expected length before accessing
                responses.Add(ChatMessage.CommandError(client,
                    "invalid parameter count (usage: `/itm [itemId]` ex.:`/itm 1234`)"));
                return;
            }

            if (!uint.TryParse(command[0], out uint itemId))
            {
                responses.Add(ChatMessage.CommandError(client,
                    $"provided parameter '{itemId}' could not be parsed as uint (usage: `/itm [itemId]` ex.:`/itm 1234`)"));
                return;
            }

            if (!_itemManager.AddItem(client, itemId))
            {
                responses.Add(ChatMessage.CommandError(client, "failed to add item"));
                return;
            }

            responses.Add(ChatMessage.CommandMessage(client, "Executed"));
        }
    }
}