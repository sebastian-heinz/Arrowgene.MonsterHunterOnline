using Arrowgene.Buffers;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdSystemEncryptData : ICsProtoHandler
{
    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(CsCmdSystemEncryptData));


    public CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_SYSTEM_ENCRYPT_DATA;

    private static string Key = "MultiByteToWideC";

    private CsProtoConsumer _csProtoConsumer;

    public CsCmdSystemEncryptData(CsProtoConsumer csProtoConsumer)
    {
        _csProtoConsumer = csProtoConsumer;
    }

    public void Handle(Client client, CsProtoPacket packet)
    {
        IBuffer req = new StreamBuffer(packet.Body);
        req.SetPositionStart();

        uint encSize = req.ReadUInt32(Endianness.Big);
        byte[] encData = req.ReadBytes((int)encSize);
        byte[] decData = new byte[encData.Length];
        
        for (int i = 0; i < encData.Length; i++)
        {
            decData[i] = (byte)(Key[i % Key.Length] ^ encData[i]);
        }
        
        _csProtoConsumer.HandleCustom(client.Socket, decData);
    }
}