﻿using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.TqqApi.Structure;

public interface CSICsTpduExtAuthData : ICsStructure
{
    public uint Uin { get; set; }
}