using System;

namespace Arrowgene.MonsterHunterOnline.Service.System;

public class Character
{
    public uint Id { get; set; }
    public uint AccountId { get; set; }

    public string Name { get; set; }
    public DateTime Created { get; set; }
}