using Arrowgene.Buffers;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdInstanceVerifyReq : ICsProtoHandler
{
    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(CsCmdInstanceVerifyReq));


    public CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_INSTANCE_VERIFY_REQ;

    public void Handle(Client client, CsProtoPacket packet)
    {
        IBuffer req = new StreamBuffer(packet.Body);
        req.SetPositionStart();

        uint Uin = req.ReadUInt32(Endianness.Big);
        int PVPMode = req.ReadInt32(Endianness.Big);
        int VerifyType = req.ReadInt32(Endianness.Big);
        int RoleID = req.ReadInt32(Endianness.Big);
        int ServiceID = req.ReadInt32(Endianness.Big);
        int KeySize = req.ReadInt32(Endianness.Big);
        string Key = req.ReadString(KeySize - 1);
        int ProtoVer = req.ReadInt32(Endianness.Big);
        int Reserve1 = req.ReadInt32(Endianness.Big);
        int Reserve2 = req.ReadInt32(Endianness.Big);


      //  client.State.OnBattleSvr();

    }
}