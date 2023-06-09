using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdFileCheckHandler : ICsProtoHandler
{
    public CsProtoCmd Cmd => CsProtoCmd.CS_CMD_FILE_CHECK;

    public void Handle(Client client, CsProtoPacket packet)
    {
        IBuffer req = new StreamBuffer(packet.Body);
        req.SetPositionStart();
        // <struct name="C2SFileCheckReq" version="1" desc="客户端文件数据校验">
        //     <entry name="Code" type="string" size="CS_FILE_CHECK_ID_LEN" desc="校验码" sizeinfo="int"/>
        //     <entry name="ScanFinish" type="uint8" desc="校验计算是否结束"/>
        //     <entry name="ScanTime" type="int" desc="校验计算时间"/>
        //     <entry name="TimeoutReScan" type="uint8" desc="是否超时重算"/>
        //     </struct>
        int CodeLen = req.ReadInt32(Endianness.Big);
        string Code = req.ReadString(CodeLen - 1);
        byte ScanFinish = req.ReadByte();
        int ScanTime = req.ReadInt32(Endianness.Big);
        int TimeoutReScan = req.ReadByte();


        // <struct name="S2CFileCheckRsp" version="1" desc="客户端文件数据校验结果">
        //     <entry name="Code" type="uchar" desc="校验结果"/>
        //     </struct>
        IBuffer res = new StreamBuffer();
        res.WriteByte(0);

        CsProtoPacket resp = new CsProtoPacket();
        resp.Body = res.GetAllBytes();
        resp.Cmd = CsProtoCmd.SC_CMD_FILE_CHECK_RLT;
        client.SendCsProto(resp);
    }
}