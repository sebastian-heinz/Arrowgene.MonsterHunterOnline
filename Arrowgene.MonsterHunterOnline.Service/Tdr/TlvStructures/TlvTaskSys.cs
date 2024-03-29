﻿using System;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

public class TlvTaskSys : Structure, ITlvStructure
{
    public TlvTaskSys()
    {
    }

    public void WriteTlv(IBuffer buffer)
    {
        WriteTlvInt32(buffer, 1, 1);
        WriteInt32(buffer, 0xABCDEF);
    }

    public void ReadTlv(IBuffer buffer)
    {
        throw new NotImplementedException();
    }
}