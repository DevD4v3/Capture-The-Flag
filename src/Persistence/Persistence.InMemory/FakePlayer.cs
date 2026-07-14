namespace Persistence.InMemory;

public class FakePlayer
{
    private const int NoSkin = -1;
    public FakePlayer() => Id = PlayerIdValueGenerator.Instance.Next();
    public FakePlayer(string name, string passwordHash)
    {
        Id = PlayerIdValueGenerator.Instance.Next();
        Name = name;
        PasswordHash = passwordHash;
    }

    public int Id { get; }
    public string Name { get; set; }
    public string PasswordHash { get; set; }
    public int TotalKills { get; set; }
    public int TotalDeaths { get; set; }
    public int MaxKillingSpree { get; set; }
    public int BroughtFlags { get; set; }
    public int CapturedFlags { get; set; }
    public int DroppedFlags { get; set; }
    public int ReturnedFlags { get; set; }
    public int HeadShots { get; set; }
    public int GunGameWins { get; set; }
    public int SkinId { get; set; } = NoSkin;
    public RoleId RoleId { get; set; } = RoleId.Basic;
    public RankId RankId { get; set; } = RankId.Noob;
    public DateTime CreatedAt { get; set; } = DateTime.Parse("2023-10-12 12:19:24");
    public DateTime LastConnection { get; set; } = DateTime.Parse("2023-10-13 12:19:24");
}
