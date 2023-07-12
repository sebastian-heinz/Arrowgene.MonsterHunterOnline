using System;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.TqqApi.Structure;

public class TqqExtAuthInfo : CsProto.Core.Structure
{
    public TqqExtAuthInfo()
    {
        EncMethod = TConnSecEnc.TCONN_SEC_NONE;
        ServiceId = 0;
        AuthType = TconnSecAuth.TCONN_SEC_AUTH_NONE;
        AuthData = null;
    }

    public TConnSecEnc EncMethod { get; set; }
    public int ServiceId { get; set; }
    public TconnSecAuth AuthType { get; set; }
    public ITpduExtAuthData AuthData { get; set; }
    
    public override void Write(IBuffer buffer)
    {
        WriteInt32(buffer, (int)EncMethod);
        WriteInt32(buffer, ServiceId);
        WriteInt32(buffer, (int)AuthType);
        WriteStructure(buffer, AuthData);
    }

    public override void Read(IBuffer buffer)
    {
        EncMethod = (TConnSecEnc)ReadInt32(buffer);
        ServiceId = ReadInt32(buffer);
        AuthType = (TconnSecAuth)ReadInt32(buffer);
        switch (AuthType)
        {
            case TconnSecAuth.TCONN_SEC_AUTH_QQV1:
            case TconnSecAuth.TCONN_SEC_AUTH_QQV2:
                AuthData = ReadStructure<TqqAuthInfo>(buffer);
                break;
            case TconnSecAuth.TCONN_SEC_AUTH_QQUNIFIED:
                AuthData = ReadStructure<TqqUnifiedAuthInfo>(buffer);
                break;
            default:
                AuthData = null;
                break;
        }
    }
}