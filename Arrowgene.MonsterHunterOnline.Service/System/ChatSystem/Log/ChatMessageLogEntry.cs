using System;
using Arrowgene.MonsterHunterOnline.Service.System.CharacterSystem;

namespace Arrowgene.MonsterHunterOnline.Service.System.ChatSystem.Log;

public class ChatMessageLogEntry
{
    public ChatMessageLogEntry(Character character, ChatMessage chatMessage)
    {
        DateTime = DateTime.Now;
        Name = character.Name;
        CharacterId = character.Id;
        ChatMessage = chatMessage;
    }

    public DateTime DateTime { get; set; }
    public string Name { get; set; }
    public uint CharacterId { get; set; }
    public ChatMessage ChatMessage { get; set; }
}