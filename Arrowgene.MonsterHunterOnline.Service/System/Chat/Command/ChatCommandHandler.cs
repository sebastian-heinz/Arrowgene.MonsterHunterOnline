using System;
using System.Collections.Generic;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.System.Chat.Command.Commands;

namespace Arrowgene.MonsterHunterOnline.Service.System.Chat.Command
{
    public class ChatCommandHandler : IChatHandler
    {
        public const char ChatCommandStart = '/';
        public const char ChatCommandSeparator = ' ';

        private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(ChatCommandHandler));

        private readonly Dictionary<string, ChatCommand> _commands;

        public ChatCommandHandler(Server server)
        {
            _commands = new Dictionary<string, ChatCommand>();
            AddCommand(new VersionCommand());
            AddCommand(new TownInitCommand());
            AddCommand(new SpawnCommand());
            AddCommand(new PositionCommand());
        }

        public void AddCommand(ChatCommand command)
        {
            _commands.Add(command.KeyToLowerInvariant, command);
        }

        public void Handle(Client client, ChatMessage message, List<ChatMessage> responses)
        {
            if (client == null)
            {
                return;
            }

            if (message.Message == null || message.Message.Length <= 1)
            {
                return;
            }

            if (!message.Message.StartsWith(ChatCommandStart))
            {
                return;
            }

            // message starts with `/` treat it as a chat command
            // this should not reach any audience
            message.Deliver = false;

            string commandMessage = message.Message.Substring(1);
            string[] command = commandMessage.Split(ChatCommandSeparator);
            if (command.Length <= 0)
            {
                Logger.Error(client, $"Command '{message.Message}' is invalid");
                responses.Add(ChatMessage.CommandError(client, $"Command '{message.Message}' is invalid"));
                return;
            }

            string commandKey = command[0].ToLowerInvariant();
            if (!_commands.ContainsKey(commandKey))
            {
                Logger.Error(client, $"Command '{commandKey}' does not exist");
                responses.Add(ChatMessage.CommandError(client, $"Command does not exist: {commandKey}"));
                return;
            }

            ChatCommand chatCommand = _commands[commandKey];
            if (client.Account.AccountType < chatCommand.Account)
            {
                Logger.Error(client,
                    $"Not entitled to execute command '{chatCommand.Key}' (State < Required: {client.Account.AccountType} < {chatCommand.Account})");
                responses.Add(ChatMessage.CommandError(client, $"Not authorized to execute this command"));
                return;
            }

            int cmdLength = command.Length - 1;
            string[] subCommand;
            if (cmdLength > 0)
            {
                subCommand = new string[cmdLength];
                Array.Copy(command, 1, subCommand, 0, cmdLength);
            }
            else
            {
                subCommand = new string[0];
            }

            chatCommand.Execute(subCommand, client, message, responses);
        }
    }
}