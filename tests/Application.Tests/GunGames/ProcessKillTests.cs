namespace CTF.Application.Tests.GunGames;

public class ProcessKillTests
{
    private GunGame _gunGame;
    private MaxWeaponLevel _maxLevel;

    [SetUp]
    public void Init()
    {
        var session = new GunGameSession
        {
            WeaponProgressionType = WeaponProgressionType.Classic,
            KillsRequiredPerLevel = new KillsRequiredPerLevel(2)
        };

        var progressions = new Dictionary<WeaponProgressionType, WeaponProgressionBase>
        {
            [WeaponProgressionType.Classic] = new TestWeaponProgression()
        };

        var weaponProgression = new WeaponProgression(session, progressions);
        _gunGame = new GunGame(weaponProgression, session.KillsRequiredPerLevel);
        _maxLevel = weaponProgression.MaxLevel;
    }

    [Test]
    public void ProcessKill_WhenPlayerAtMaxLevelKillsWithKnife_ShouldReturnScoredFinalKill()
    {
        // Arrange
        var killer = new PlayerProgression();
        var victim = new PlayerProgression();
        Weapon finalLevelWeapon = Weapon.Knife;

        killer.LevelUp(_maxLevel);
        killer.LevelUp(_maxLevel);
        killer.LevelUp(_maxLevel);

        // Act
        GunGameResult result = _gunGame.ProcessKill(killer, victim, finalLevelWeapon);

        // Assert
        result.Should().Be(GunGameResult.ScoredFinalKill);
        killer.WeaponLevel.Value.Should().Be(_maxLevel.Value);
        killer.KillsTowardsNextLevel.Should().Be(0);
        victim.WeaponLevel.Should().Be(WeaponLevel.First);
    }

    [Test]
    public void ProcessKill_WhenVictimIsAtFirstLevelAndKilledWithKnife_ShouldReturnNone()
    {
        // Arrange
        var killer = new PlayerProgression();
        var victim = new PlayerProgression();
        Weapon firstLevelWeapon = Weapon.Knife;

        // Act
        GunGameResult result = _gunGame.ProcessKill(killer, victim, firstLevelWeapon);

        // Assert
        result.Should().Be(GunGameResult.None);
        victim.WeaponLevel.Should().Be(WeaponLevel.First);
        victim.KillsTowardsNextLevel.Should().Be(0);
    }

    [Test]
    public void ProcessKill_WhenVictimIsAboveFirstLevelAndKilledWithKnife_ShouldLevelDownVictim()
    {
        // Arrange
        var killer = new PlayerProgression();
        var victim = new PlayerProgression();
        Weapon finalLevelWeapon = Weapon.Knife;
        victim.LevelUp(_maxLevel);

        // Act
        GunGameResult result = _gunGame.ProcessKill(killer, victim, finalLevelWeapon);

        // Assert
        result.Should().Be(GunGameResult.LeveledDown);
        victim.WeaponLevel.Should().Be(WeaponLevel.First);
        victim.KillsTowardsNextLevel.Should().Be(0);
    }

    [Test]
    public void ProcessKill_WhenPlayerAtFirstLevelKillsWithWeaponFromAnotherLevel_ShouldReturnNone()
    {
        // Arrange
        var killer = new PlayerProgression();
        var victim = new PlayerProgression();
        Weapon weaponFromAnotherLevel = Weapon.M4;
        killer.AddKillsTowardsNextLevel();

        // Act
        GunGameResult result = _gunGame.ProcessKill(killer, victim, weaponFromAnotherLevel);

        // Assert
        result.Should().Be(GunGameResult.None);
        killer.WeaponLevel.Should().Be(WeaponLevel.First);
        killer.KillsTowardsNextLevel.Should().Be(1);
        victim.WeaponLevel.Should().Be(WeaponLevel.First);
    }

    [Test]
    public void ProcessKill_WhenRequiredKillsHaveNotBeenReached_ShouldReturnNone()
    {
        // Arrange
        var killer = new PlayerProgression();
        var victim = new PlayerProgression();
        Weapon firstLevelWeapon = Weapon.Colt45;

        // Act
        GunGameResult result = _gunGame.ProcessKill(killer, victim, firstLevelWeapon);

        // Assert
        result.Should().Be(GunGameResult.None);
        killer.WeaponLevel.Should().Be(WeaponLevel.First);
        killer.KillsTowardsNextLevel.Should().Be(1);
        victim.WeaponLevel.Should().Be(WeaponLevel.First);
    }

    [Test]
    public void ProcessKill_WhenRequiredKillsAreReached_ShouldReturnLeveledUp()
    {
        // Arrange
        var killer = new PlayerProgression();
        var victim = new PlayerProgression();
        killer.AddKillsTowardsNextLevel();
        Weapon firstLevelWeapon = Weapon.Colt45;
        int expectedWeaponLevel = 2;

        // Act
        GunGameResult result = _gunGame.ProcessKill(killer, victim, firstLevelWeapon);

        // Assert
        result.Should().Be(GunGameResult.LeveledUp);
        killer.WeaponLevel.Value.Should().Be(expectedWeaponLevel);
        killer.KillsTowardsNextLevel.Should().Be(0);
        victim.WeaponLevel.Should().Be(WeaponLevel.First);
    }

    [Test]
    public void ProcessKill_WhenPlayerReachesFinalLevel_ShouldReturnReachedFinalLevel()
    {
        // Arrange
        var killer = new PlayerProgression();
        var victim = new PlayerProgression();

        killer.LevelUp(_maxLevel);
        killer.LevelUp(_maxLevel);

        killer.AddKillsTowardsNextLevel();
        Weapon thirdLevelWeapon = Weapon.AK47;

        // Act
        GunGameResult result = _gunGame.ProcessKill(killer, victim, thirdLevelWeapon);

        // Assert
        result.Should().Be(GunGameResult.ReachedFinalLevel);
        killer.WeaponLevel.Value.Should().Be(_maxLevel.Value);
        killer.KillsTowardsNextLevel.Should().Be(0);
    }

    [Test]
    public void ProcessKill_WhenPlayerAtFinalLevelKillsWithNonKnifeFinalWeapon_ShouldReturnScoredFinalKill()
    {
        // Arrange
        var session = new GunGameSession
        {
            WeaponProgressionType = WeaponProgressionType.Classic,
            KillsRequiredPerLevel = new KillsRequiredPerLevel(2)
        };

        var progressions = new Dictionary<WeaponProgressionType, WeaponProgressionBase>
        {
            [WeaponProgressionType.Classic] = new NonKnifeFinalWeaponProgression()
        };

        var weaponProgression = new WeaponProgression(session, progressions);
        var gunGame = new GunGame(weaponProgression, session.KillsRequiredPerLevel);
        var killer = new PlayerProgression();
        var victim = new PlayerProgression();

        MaxWeaponLevel maxLevel = weaponProgression.MaxLevel;
        Weapon finalLevelWeapon = Weapon.Minigun;

        killer.LevelUp(maxLevel);
        killer.LevelUp(maxLevel);
        killer.LevelUp(maxLevel);

        // Act
        GunGameResult result = gunGame.ProcessKill(killer, victim, finalLevelWeapon);

        // Assert
        result.Should().Be(GunGameResult.ScoredFinalKill);
        killer.WeaponLevel.Value.Should().Be(maxLevel.Value);
        killer.KillsTowardsNextLevel.Should().Be(0);
        victim.WeaponLevel.Should().Be(WeaponLevel.First);
    }
}
