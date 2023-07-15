using System.Collections.Generic;
using Arrowgene.Logging;

namespace Arrowgene.MonsterHunterOnline.Service.System.Chat;

public class ChatSystem
{
    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(ChatSystem));

    private readonly List<IChatHandler> _handler;

    public ChatSystem()
    {
        _handler = new List<IChatHandler>();
    }

    public void AddHandler(IChatHandler handler)
    {
        _handler.Add(handler);
    }

    public void Handle(Client client, ChatMessage message)
    {
        if (client == null)
        {
            Logger.Debug("Client is Null");
            return;
        }

        if (message == null)
        {
            Logger.Debug(client, "Chat Message is Null");
            return;
        }

        // TODO obsolete
        message.Client.State.OnChatMsg(message);
        //

        List<ChatResponse> responses = new List<ChatResponse>();
        foreach (IChatHandler handler in _handler)
        {
            handler.Handle(client, message, responses);
        }

        if (message.Deliver)
        {
            // deliver original chat message
            //   ChatResponse response = ChatResponse.FromMessage(client, message);
      //      Deliver(client, response);
        }

        foreach (ChatResponse response in responses)
        {
            // deliver additional messages generated form handler
            if (!response.Deliver)
            {
                continue;
            }

            Deliver(client, response);
        }
    }

    private void Deliver(Client client, ChatResponse response)
    {
        switch (response.ChannelType)
        {
            case ChannelType.Global:
                //  response.Recipients.AddRange(_server.Clients);
                break;
            default:
                response.Recipients.Add(client);
                break;
        }

        // _router.Send(response);
    }
}