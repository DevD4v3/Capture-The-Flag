namespace CTF.Application.Tests.Maps;

public class CurrentMapTests
{
    [Test]
    public void Constructor_WhenMapIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IMap map = default;
        List<SpawnLocation> spawnLocations = [SpawnLocation.Empty];
        FlagLocations flagLocations = FlagLocations.Empty;

        // Act
        Action act = () =>
        {
            var currentMap = new CurrentMap(map, spawnLocations, spawnLocations, flagLocations);
        };

        // Assert
        act.Should()
           .Throw<ArgumentNullException>()
           .WithParameterName(nameof(map));
    }

    [Test]
    public void Constructor_WhenAlphaTeamLocationsIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IMap map = new FakeMap();
        List<SpawnLocation> alphaTeamLocations = default;
        List<SpawnLocation> betaTeamLocations = [SpawnLocation.Empty];
        FlagLocations flagLocations = FlagLocations.Empty;

        // Act
        Action act = () =>
        {
            var currentMap = new CurrentMap(map, alphaTeamLocations, betaTeamLocations, flagLocations);
        };

        // Assert
        act.Should()
           .Throw<ArgumentNullException>()
           .WithParameterName(nameof(alphaTeamLocations));
    }

    [Test]
    public void Constructor_WhenBetaTeamLocationsIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IMap map = new FakeMap();
        List<SpawnLocation> betaTeamLocations = default;
        List<SpawnLocation> alphaTeamLocations = [SpawnLocation.Empty];
        FlagLocations flagLocations = FlagLocations.Empty;

        // Act
        Action act = () =>
        {
            var currentMap = new CurrentMap(map, alphaTeamLocations, betaTeamLocations, flagLocations);
        };

        // Assert
        act.Should()
           .Throw<ArgumentNullException>()
           .WithParameterName(nameof(betaTeamLocations));
    }

    [Test]
    public void Constructor_WhenFlagLocationsIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IMap map = new FakeMap();
        List<SpawnLocation> betaTeamLocations = [SpawnLocation.Empty];
        List<SpawnLocation> alphaTeamLocations = [SpawnLocation.Empty];
        FlagLocations flagLocations = default;

        // Act
        Action act = () =>
        {
            var currentMap = new CurrentMap(map, alphaTeamLocations, betaTeamLocations, flagLocations);
        };

        // Assert
        act.Should()
           .Throw<ArgumentNullException>()
           .WithParameterName(nameof(flagLocations));
    }

    [Test]
    public void Constructor_WhenAlphaTeamLocationsIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        IMap map = new FakeMap();
        List<SpawnLocation> alphaTeamLocations = [];
        List<SpawnLocation> betaTeamLocations = [SpawnLocation.Empty];
        FlagLocations flagLocations = FlagLocations.Empty;

        // Act
        Action act = () =>
        {
            var currentMap = new CurrentMap(map, alphaTeamLocations, betaTeamLocations, flagLocations);
        };

        // Assert
        act.Should()
           .Throw<ArgumentException>()
           .WithParameterName(nameof(alphaTeamLocations));
    }

    [Test]
    public void Constructor_WhenBetaTeamLocationsIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        IMap map = new FakeMap();
        List<SpawnLocation> alphaTeamLocations = [SpawnLocation.Empty];
        List<SpawnLocation> betaTeamLocations = [];
        FlagLocations flagLocations = FlagLocations.Empty;

        // Act
        Action act = () =>
        {
            var currentMap = new CurrentMap(map, alphaTeamLocations, betaTeamLocations, flagLocations);
        };

        // Assert
        act.Should()
           .Throw<ArgumentException>()
           .WithParameterName(nameof(betaTeamLocations));
    }

    [Test]
    public void GetMapNameAsText_WhenNameIsObtained_ShouldReturnValidStringFormat()
    {
        // Arrange
        IMap map = new FakeMap(id: 0, name: "RC_Battlefield");
        List<SpawnLocation> spawnLocations = [SpawnLocation.Empty];
        FlagLocations flagLocations = FlagLocations.Empty;
        var currentMap = new CurrentMap(map, spawnLocations, spawnLocations, flagLocations);
        var expectedString = "Map: ~w~RC_Battlefield";

        // Act
        string actual = currentMap.GetMapNameAsText();

        // Assert
        actual.Should().Be(expectedString);
    }

    [TestCase(TeamId.Alpha)]
    [TestCase(TeamId.Beta)]
    public void GetRandomSpawnLocation_WhenTeamIsAlphaOrBeta_ShouldReturnSpawnLocation(TeamId team)
    {
        // Arrange
        IMap map = new FakeMap();
        List<SpawnLocation> spawnLocations = [SpawnLocation.Empty];
        FlagLocations flagLocations = FlagLocations.Empty;
        var currentMap = new CurrentMap(map, spawnLocations, spawnLocations, flagLocations);
        SpawnLocation expectedSpawnLocation = spawnLocations[0];

        // Act
        SpawnLocation actual = currentMap.GetRandomSpawnLocation(team);

        // Assert
        actual.Should().Be(expectedSpawnLocation);
    }

    [Test]
    public void GetRandomSpawnLocation_WhenTeamIsNotAlphaOrBeta_ShouldThrowNotSupportedException()
    {
        // Arrange
        IMap map = new FakeMap();
        List<SpawnLocation> spawnLocations = [SpawnLocation.Empty];
        FlagLocations flagLocations = FlagLocations.Empty;
        TeamId team = TeamId.NoTeam;
        var currentMap = new CurrentMap(map, spawnLocations, spawnLocations, flagLocations);

        // Act
        Action act = () => currentMap.GetRandomSpawnLocation(team);

        // Assert
        act.Should().Throw<NotSupportedException>();
    }
}
