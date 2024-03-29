﻿using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

public class TlvItemList : Structure, ITlvStructure
{
    private const int ItemsMaxSize = 0x9C4;

    public TlvItemList()
    {
        UnknownA = 0;
        UnknownB = 0;
        Items = new List<Item>();
    }

    public short UnknownA { get; set; }
    public short UnknownB { get; set; }
    public List<Item> Items { get; }

    public void WriteTlv(IBuffer buffer)
    {
        // case 1
        WriteTlvInt16(buffer, 1, UnknownA);

        // case 2
        int maxItems = Math.Min(Items.Count, ItemsMaxSize);
        if (maxItems > 0)
        {
            WriteTlvSubStructureList(buffer, 2, maxItems, Items);
        }

        // case 3
        WriteTlvInt16(buffer, 3, UnknownA);
    }

    public void ReadTlv(IBuffer buffer)
    {
        throw new NotImplementedException();
    }
}