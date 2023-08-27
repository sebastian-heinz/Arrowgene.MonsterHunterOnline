using System.Collections.Generic;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System.CharacterSystem;

namespace Arrowgene.MonsterHunterOnline.Service.System.ChatSystem;

public class ChatManager
{
    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(ChatManager));

    private readonly List<IChatHandler> _handler;
    private readonly ClientManager _clientManager;

    public ChatManager(ClientManager clientManager)
    {
        _handler = new List<IChatHandler>();
        _clientManager = clientManager;
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

        List<ChatMessage> responses = new List<ChatMessage>();

        foreach (IChatHandler handler in _handler)
        {
            handler.Handle(client, message, responses);
        }

        if (message.Deliver)
        {
            Deliver(client, message);
        }

        foreach (ChatMessage response in responses)
        {
            if (!response.Deliver)
            {
                continue;
            }

            Deliver(client, response);
        }
    }

    private void Deliver(Client client, ChatMessage message)
    {
        switch (message.Channel)
        {
            case ChannelType.Local:
            case ChannelType.Sys_A:
            case ChannelType.Sys_B:
                message.Recipients.Add(client);

                CsProtoStructurePacket<ChatNtf> chatNtf = MakeChatNtf(client, message);
                foreach (Client recipient in message.Recipients)
                {
                    recipient.SendCsProtoStructurePacket(chatNtf);
                }

                break;
            case ChannelType.PM:
                Client targetClient = _clientManager.GetClientByCharacterName(message.TargetName);
                if (targetClient == null)
                {
                    // TODO chat error
                    return;
                }

                Character targetCharacter = targetClient.Character;
                if (targetCharacter == null)
                {
                    // TODO chat error
                    return;
                }

                CsProtoStructurePacket<ChatNtf> pmSource = MakeChatNtf(client, message);
                pmSource.Structure.SourceId = (int)targetCharacter.Id;
                pmSource.Structure.SourceName = targetCharacter.Name;
                pmSource.Structure.SendByMe = true;
                client.SendCsProtoStructurePacket(pmSource);

                CsProtoStructurePacket<ChatNtf> pmTarget = MakeChatNtf(client, message);
                targetClient.SendCsProtoStructurePacket(pmTarget);
                break;
            case ChannelType.Area:
                break;
            case ChannelType.Clan:
                break;
            default:
                Logger.Error(client, $"Unhandled ChannelType: {message.Channel}");
                break;
        }
    }

    private CsProtoStructurePacket<ChatNtf> MakeChatNtf(Client client, ChatMessage message)
    {
        CsProtoStructurePacket<ChatNtf> chatNtf = CsProtoResponse.ChatNtf;
        chatNtf.Structure.SourceId = (int)client.Character.Id;
        chatNtf.Structure.SrcUin = client.Character.Id;
        chatNtf.Structure.SrcDbId = client.Character.Id;
        chatNtf.Structure.SrcLevelGrpId = 0;
        chatNtf.Structure.SourceName = client.Character.Name;
        chatNtf.Structure.SrcVipLevel = 0;
        chatNtf.Structure.SrcVipCanUse = 0;
        chatNtf.Structure.ChannelType = message.Channel;
        chatNtf.Structure.LineId = 0;
        chatNtf.Structure.WorldSvrId = 1;
        chatNtf.Structure.ShowTime = 0;
        chatNtf.Structure.Content = message.Message;
        chatNtf.Structure.SendByMe = false;
        chatNtf.Structure.ContainBanWords = false;
        chatNtf.Structure.SrcLevel = (int)client.Character.Level;
        chatNtf.Structure.SrcGuildName = "GuildName";
        chatNtf.Structure.SrcHunterStar = "SrcHunterStar";
        chatNtf.Structure.SrcHrLevel = client.Character.HrLevel;
        return chatNtf;
    }
}