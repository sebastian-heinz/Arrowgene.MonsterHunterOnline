using System;

namespace Arrowgene.MonsterHunterOnline.Service.System;

public class Account
{
    public uint Id { get; set; }
    public uint Uin { get; set; }
    public string PasswordHash { get; set; }
    public AccountType AccountType { get; set; }
    public DateTime Created { get; set; }
}