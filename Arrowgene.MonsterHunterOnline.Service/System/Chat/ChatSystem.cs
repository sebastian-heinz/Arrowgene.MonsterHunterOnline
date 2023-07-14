namespace Arrowgene.MonsterHunterOnline.Service.System.Chat;

public class ChatSystem
{
    public void Handle(ChatMessage chatMessage)
    {
        chatMessage.Client.State.OnChatMsg(chatMessage);
    }
}