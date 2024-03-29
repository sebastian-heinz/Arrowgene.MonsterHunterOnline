using System.Collections.Generic;

namespace Arrowgene.MonsterHunterOnline.Service.System.ChatSystem.Command
{
    public abstract class ChatCommand
    {
        public abstract void Execute(string[] command, Client client, ChatMessage message,
            List<ChatMessage> responses);

        public abstract AccountType Account { get; }
        public abstract string Key { get; }
        public string KeyToLowerInvariant => Key.ToLowerInvariant();

        public virtual string HelpText
        {
            get { return null; }
        }
    }
}