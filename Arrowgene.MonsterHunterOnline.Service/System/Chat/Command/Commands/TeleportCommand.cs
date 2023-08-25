using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.CsProto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.MonsterHunterOnline.Service.System.Chat.Command.Commands
{
    /// <summary>
    /// Teleport to position
    /// </summary>
    public class TeleportCommand : ChatCommand
    {
        public override AccountType Account => AccountType.User;
        public override string Key => "tp";
        public override string HelpText => "usage: `/tp` - teleport to position";

        public override void Execute(string[] command, Client client, ChatMessage message, List<ChatMessage> responses)
        {
            client.SendCsPacket(NewCsPacket.PlayerTeleport(new CSPlayerTeleport()
            {
                SyncTime = 0,
                NetObjId = client.Character.Id,
                Region = client.State.levelId,
                TargetPos = new CSQuatT()
                {
                    q = new CSQuat()
                    {
                        v = new CSVec3()
                        {
                            x = 10,
                            y = 10,
                            z = 10
                        },
                        w = 10
                    },
                    t = new CSVec3()
                    {
                        x = 1621.11597f,
                        y = 1550.0204f,
                        z = 123.0f
                    }
                },
                ParentGUID = 1,
                InitState = 1
            }

            ));
            string msg = $"Teleported to :\nX:{client.State.Position.x}\nY:{client.State.Position.y}\nZ:{client.State.Position.z}";
            ChatMessage response = ChatMessage.CommandMessage(client, msg);

            responses.Add(response);
        }
    }
}
