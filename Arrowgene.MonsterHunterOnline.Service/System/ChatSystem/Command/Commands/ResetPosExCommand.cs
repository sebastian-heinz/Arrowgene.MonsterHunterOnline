using System.Collections.Generic;
using Arrowgene.Logging;

namespace Arrowgene.MonsterHunterOnline.Service.System.ChatSystem.Command.Commands
{
    /// <summary>
    /// Client send this command when character seems to be out of bounds, include the character position
    /// </summary>
    public class ResetPosExCommand : ChatCommand
    {
        private static readonly ServiceLogger Logger =
            LogProvider.Logger<ServiceLogger>(typeof(ResetPosExCommand));

        public override AccountType Account => AccountType.User;
        public override string Key => "resetposex";
        public override string HelpText => "usage: `/resetposex x y z`";

        public override void Execute(string[] command, Client client, ChatMessage message, List<ChatMessage> responses)
        {
            int startOffset;

            if (command.Length == 3)
            {
                startOffset = 0;
            }
            else if (command.Length == 4)
            {
                startOffset = 1;
            }
            else
            {
                responses.Add(ChatMessage.CommandError(client,
                    "invalid parameter count (usage: `/resetposex x y z`)"));
                return;
            }

            if (!float.TryParse(command[startOffset].TrimEnd(','), out float posX))
            {
                responses.Add(ChatMessage.CommandError(client,
                    $"provided parameter '{command[startOffset]}' could not be parsed as float (usage: `/resetposex x y z`)"));
                return;
            }

            if (!float.TryParse(command[startOffset + 1].TrimEnd(','), out float posY))
            {
                responses.Add(ChatMessage.CommandError(client,
                    $"provided parameter '{command[startOffset + 1]}' could not be parsed as float (usage: `/resetposex x y z`)"));
                return;
            }

            if (!float.TryParse(command[startOffset + 2], out float posZ))
            {
                responses.Add(ChatMessage.CommandError(client,
                    $"provided parameter '{command[startOffset + 2]}' could not be parsed as float (usage: `/resetposex x y z`)"));
                return;
            }

            Logger.Info(client, $"Out of bounds {posX} {posY} {posZ}");

            // TODO this is a client executable command, so it should find the closest in bounds position and help
            // reset the character.
        }
    }
}