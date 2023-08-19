using System.Collections.Generic;

namespace Arrowgene.MonsterHunterOnline.Service.System.Chat;

public class ChatMessage
{
    public static ChatMessage CommandError(Client client, string message)
    {
        return new ChatMessage(client, ChannelType.Sys_A, message);
    }

    public static ChatMessage CommandMessage(Client client, string message)
    {
        return new ChatMessage(client, ChannelType.Sys_A, message);
    }

    public Client Client { get; }
    public string Message { get; }
    public string TargetName { get; }
    public ChannelType Channel { get; }
    public bool Deliver { get; set; }

    public List<Client> Recipients { get; }

    public ChatMessage(Client client, ChannelType channel, string message)
    {
        Client = client;
        Channel = channel;
        Message = message;
        TargetName = null;
        Deliver = true;
        Recipients = new List<Client>();
    }

    public ChatMessage(Client client, ChannelType channel, string targetName, string message)
    {
        Client = client;
        Channel = channel;
        Message = message;
        TargetName = targetName;
        Deliver = true;
        Recipients = new List<Client>();
    }
}