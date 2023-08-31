using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

/// <summary>
/// crygame.dll+0x217820
/// </summary>
public class TlvLevelInfo : Structure, ITlvStructure
{

    public TlvLevelInfo()
    {

    }


    public void WriteTlv(IBuffer buffer)
    {
        int startPos = buffer.Position;
        WriteInt32(buffer, 0);

        WriteTlvInt32(buffer, 2, 0xABCDE);
      
        //starNum
        //branchNum
        //branchList
        //starList
        //quality
        //finishTime
        //finishTime?
        

        int endPos = buffer.Position;
        int size = endPos - startPos + 1;
        buffer.Position = startPos;
        WriteInt32(buffer, size);
        buffer.Position = endPos;
    }

    public void ReadTlv(IBuffer buffer)
    {
        throw new NotImplementedException();
    }
}