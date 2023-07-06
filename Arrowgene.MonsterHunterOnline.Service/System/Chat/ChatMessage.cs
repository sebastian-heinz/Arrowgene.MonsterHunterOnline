namespace Arrowgene.MonsterHunterOnline.Service.System.Chat;

public class ChatMessage
{
    public Client Client { get; }
    public string Message { get; }
    public ChannelType Channel { get; }

    public ChatMessage(Client client, ChannelType channel, string message)
    {
        Client = client;
        Channel = channel;
        Message = message;
    }
}