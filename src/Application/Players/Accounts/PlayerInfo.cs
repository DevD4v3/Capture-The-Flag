namespace CTF.Application.Players.Accounts;

public partial class PlayerInfo
{
    private const string PlayerNamePattern = @"^[0-9a-zA-Z\[\]\(\)\$\@._=]+$";
    private const int NoSkin = -1;
    private const int NoAccount = -1;
    [GeneratedRegex(PlayerNamePattern)]
    private static partial Regex PlayerNameRegex();

    public PlayerInfo() { }

    /// <summary>
    /// It is generated automatically by the database provider.
    /// </summary>
    /// <remarks>
    /// It is a permanent identifier that is generated when the player's account is created in the database.
    /// </remarks>
    public int AccountId { get; private set; } = NoAccount;
    public string Name { get; private set; } = "DefaultUser";
    public string Password { get; private set; } = "DefaultPassword";
    public PlayerStatsPerRound StatsPerRound { get; } = new();
    public int TotalKills { get; private set; }
    public int TotalDeaths { get; private set; }

    /// <summary>
    /// Indicates the maximum killing spree.
    /// </summary>
    public int MaxKillingSpree { get; private set; }

    /// <summary>
    /// Indicates the number of times a player has captured the opposing team's flag and brought it back to their own base.
    /// </summary>
    public int BroughtFlags { get; private set; }

    /// <summary>
    /// Indicates the number of times a player has captured the opposing team's flag from their base.
    /// </summary>
    public int CapturedFlags { get; private set; }

    /// <summary>
    /// Indicates the number of times a player has dropped the opposing team's flag.
    /// </summary>
    public int DroppedFlags { get; private set; }

    /// <summary>
    /// Indicates the number of times a player has returned the flag to their team's base.
    /// </summary>
    public int ReturnedFlags { get; private set; }

    /// <summary>
    /// Indicates the number of shots that the player has made at the heads of other players.
    /// </summary>
    public int HeadShots { get; private set; }
    public RoleId RoleId { get; private set; } = RoleId.Basic;
    public int SkinId { get; private set; } = NoSkin;
    public RankId RankId { get; private set; } = RankId.Noob;
    public Team Team { get; private set; } = Team.None;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime LastConnection { get; private set; } = DateTime.UtcNow;

    public bool HasSurpassedMaxKillingSpree() => StatsPerRound.KillingSpree > MaxKillingSpree;
    public bool HasSkin() => SkinId != NoSkin;
    public bool HasRole(RoleId id) => RoleId == id;
    public bool HasLowerRoleThan(RoleId id) => RoleId < id;
    public bool HasRank(RankId id) => RankId == id;
    public bool IsVIP() => HasRole(RoleId.VIP);
    public bool IsModerator() => HasRole(RoleId.Moderator);
    public bool IsAdmin() => HasRole(RoleId.Admin);
    public bool IsNotVIP() => !IsVIP();
    public bool IsNotModerator() => !IsModerator();
    public bool IsNotAdmin() => !IsAdmin();
    public void SetLastConnection() => LastConnection = DateTime.UtcNow;
    public void SetMaxKillingSpree(int value) => MaxKillingSpree = value;
    public void RemoveSkin() => SkinId = NoSkin;
    public void AddTotalKills() => TotalKills++;
    public void AddTotalDeaths() => TotalDeaths++;
    public void AddBroughtFlags() => BroughtFlags++;
    public void AddCapturedFlags() => CapturedFlags++;
    public void AddDroppedFlags() => DroppedFlags++;
    public void AddReturnedFlags() => ReturnedFlags++;
    public void AddHeadShots() => HeadShots++;

    public Result SetName(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure(Messages.NameCannotBeEmpty);

        if (value.Length < 3 || value.Length > 20)
            return Result.Failure(Messages.PlayerNameLength);

        if (!PlayerNameRegex().IsMatch(value))
            return Result.Failure(Messages.InvalidNickName);

        Name = value;
        return Result.Success();
    }

    public Result SetPassword(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure(Messages.PasswordCannotBeEmpty);

        if (value.Length < 5 || value.Length > 20)
            return Result.Failure(Messages.PasswordLength);

        Password = value;
        return Result.Success();
    }

    public Result SetTotalKills(int value)
    {
        if (value < 0)
            return Result.Failure(Messages.ValueCannotBeNegative);

        TotalKills = value;
        return Result.Success();
    }

    public Result SetTotalDeaths(int value)
    {
        if (value < 0)
            return Result.Failure(Messages.ValueCannotBeNegative);

        TotalDeaths = value;
        return Result.Success();
    }

    public Result SetRole(RoleId id)
    {
        if (id < 0 || (int)id >= RoleCollection.Count)
            return Result.Failure(Messages.InvalidRole);

        RoleId = id;
        return Result.Success();
    }

    public Result SetRank(RankId id)
    {
        if (id < 0 || (int)id >= RankCollection.Count)
            return Result.Failure(Messages.InvalidRank);

        RankId = id;
        return Result.Success();
    }

    public Result SetSkin(int id)
    {
        if (id < 0 || id > 311)
            return Result.Failure(Messages.InvalidSkin);

        SkinId = id;
        return Result.Success();
    }

    public Result SetTeam(TeamId id)
    {
        Result<Team> result = id switch
        {
            TeamId.Alpha  => Result<Team>.Success(Team.Alpha),
            TeamId.Beta   => Result<Team>.Success(Team.Beta),
            TeamId.NoTeam => Result<Team>.Success(Team.None),
            _ => Result<Team>.Failure()
        };

        if (result.IsSuccess)
        {
            Team = result.Value;
            return Result.Success();
        }

        return Result.Failure(Messages.InvalidTeam);
    }

    /// <summary>
    /// Checks if the player has captured the opposing team's flag.
    /// </summary>
    public bool HasCapturedFlag()
    {
        if (Team == Team.None) 
            return false;

        Flag rivalTeamFlag = Team.RivalTeam.Flag;
        if(rivalTeamFlag.IsCaptured())
        {
            Player carrier = rivalTeamFlag.Carrier;
            return carrier.Name.Equals(Name, StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }

    public bool CanMoveUpToNextRank()
    {
        IRank currentRank = RankCollection.GetById(RankId).Value;
        if (currentRank.IsMax())
            return false;

        IRank nextRank = RankCollection.GetNextRank(RankId).Value;
        return TotalKills >= nextRank.RequiredKills;
    }

    public string GetStatsAsText()
    {
        Result<IRank> rankResult = RankCollection.GetById(RankId);
        var stats = new
        {
            StatsPerRound.Kills,
            StatsPerRound.Deaths,
            StatsPerRound.KillingSpree,
            StatsPerRound.Coins,
            MaxRank = RankCollection.Count,
            Level = (int)RankId + 1,
            RankName = rankResult.Value.Name
        };
        const string message = 
            "~w~KILLS: ~y~{Kills} ~w~DEATHS: ~y~{Deaths} ~w~SPREE: ~y~{KillingSpree} " +
            "~w~COINS: ~y~{Coins}/100 ~w~LEVEL: ~y~{Level}/{MaxRank} ~w~RANK: ~y~{RankName}";
        return Smart.Format(message, stats);
    }
}
