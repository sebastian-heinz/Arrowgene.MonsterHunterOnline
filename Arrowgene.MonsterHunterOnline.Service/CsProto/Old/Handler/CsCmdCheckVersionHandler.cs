using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdCheckVersionHandler : ICsProtoHandler
{
    public CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_CHECK_VERSION_REQ;

    public void Handle(Client client, CsProtoPacket packet)
    {
        IBuffer req = new StreamBuffer(packet.Body);
        req.SetPositionStart();

        int MajorVerNo = req.ReadInt32(Endianness.Big);
        int MinorVerNo = req.ReadInt32(Endianness.Big);
        int RevisVerNo = req.ReadInt32(Endianness.Big);
        int BuildVerNo = req.ReadInt32(Endianness.Big);
        int IgnoreTag = req.ReadInt32(Endianness.Big);
        int ProtoVer = req.ReadInt32(Endianness.Big);

        uint FeatureHash = req.ReadUInt32(Endianness.Big);
        ulong FeatureEnabled = req.ReadUInt64(Endianness.Big);

        int Reserve1 = req.ReadInt32(Endianness.Big);
        int Reserve2 = req.ReadInt32(Endianness.Big);
        int TGPTicketLen = req.ReadInt32(Endianness.Big);
        for (int i = 0; i < TGPTicketLen; i++)
        {
            byte TGPTicket = req.ReadByte();
            //
        }


        IBuffer res = new StreamBuffer();
        res.WriteInt32(0, Endianness.Big); //ErrNo
        res.WriteInt32(MajorVerNo, Endianness.Big); //MajorVerNo
        res.WriteInt32(MinorVerNo, Endianness.Big); //MinorVerNo
        res.WriteInt32(RevisVerNo, Endianness.Big); //RevisVerNo
        res.WriteInt32(BuildVerNo, Endianness.Big); //BuildVerNo
        res.WriteUInt32(FeatureHash, Endianness.Big); //FeatureHash
        res.WriteUInt64(FeatureEnabled, Endianness.Big); //FeatureEnabled

        CsProtoPacket resp = new CsProtoPacket();
        resp.Body = res.GetAllBytes();
        resp.Cmd = CS_CMD_ID.CS_CMD_CHECK_VERSION_RSP;
        client.SendCsProtoPacket(resp);
    }
}