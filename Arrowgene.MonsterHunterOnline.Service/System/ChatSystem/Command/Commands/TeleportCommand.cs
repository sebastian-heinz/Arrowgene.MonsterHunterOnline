using System.Collections.Generic;
using Arrowgene.MonsterHunterOnline.Service.CsProto;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.System.ChatSystem.Command.Commands
{
    /// <summary>
    /// Teleport to position
    /// </summary>
    public class TeleportCommand : ChatCommand
    {
        public override AccountType Account => AccountType.User;
        public override string Key => "tp";
        public override string HelpText => "usage: `/tp x y z` - teleport to position";

        public override void Execute(string[] command, Client client, ChatMessage message, List<ChatMessage> responses)
        {
            if (!float.TryParse(command[0], out float posX))
            {
                responses.Add(ChatMessage.CommandError(client,
                    $"provided parameter '{command[0]}' could not be parsed as float (usage: `/tp x y z`)"));
                return;
            }

            if (!float.TryParse(command[1], out float posY))
            {
                responses.Add(ChatMessage.CommandError(client,
                    $"provided parameter '{command[1]}' could not be parsed as float (usage: `/tp x y z`)"));
                return;
            }

            if (!float.TryParse(command[2], out float posZ))
            {
                responses.Add(ChatMessage.CommandError(client,
                    $"provided parameter '{command[2]}' could not be parsed as float (usage: `/tp x y z`)"));
                return;
            }

            client.SendCsPacket(NewCsPacket.PlayerTeleport(new CSPlayerTeleport()
                {
                    SyncTime = 0,
                    NetObjId = client.Character.Id,
                    Region = client.State.levelId,
                    TargetPos = new CSQuatT()
                    {
                        t = new CSVec3()
                        {
                            x = posX,
                            y = posY,
                            z = posZ
                        }
                    },
                    ParentGUID = 1,
                    InitState = 1
                }
            ));
            string msg = $"Teleported to :\nX:{posX}\nY:{posY}\nZ:{posZ}";
            ChatMessage response = ChatMessage.CommandMessage(client, msg);

            responses.Add(response);
        }
    }
}