namespace CTF.Application.Tests.Maps;

public class MapCollectionTests
{
    static readonly int[] InvalidMapCases = [-1, 1000];
    private MapCollection _maps;

    [SetUp]
    public void SetUp()
    {
        _maps = new MapCollection(TestPaths.Maps);
    }

    [TestCase("de")]
    [TestCase("DE")]
    [TestCase("dE")]
    [TestCase("De")]
    public void GetAll_WhenAllMapsAreObtainedWithFindBy_ShouldReturnsEnumerable(string findBy)
    {
        // Arrange
        string[] expectedMaps =
        [
            "de_aztec",
            "de_dust2",
            "de_dust2_small",
            "de_dust2x1",
            "de_dust2x2",
            "de_dust2x3",
            "de_dust2x4",
            "de_dust2x5",
            "de_dust5",
            "DesertGlory"
        ];

        // Act
        IEnumerable<IMap> maps = _maps.GetAll(findBy);
        string[] actual = maps.Select(map => map.Name).ToArray();

        // Assert
        actual.Should().BeEquivalentTo(expectedMaps);
    }

    [TestCaseSource(nameof(InvalidMapCases))]
    public void GetById_WhenMapIdIsInvalid_ShouldReturnsFailureResult(int mapId)
    {
        // Arrange
        string expectedMessage = Messages.InvalidMap;

        // Act
        Result<IMap> result = _maps.GetById(mapId);

        // Asserts
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(expectedMessage);
    }

    [Test]
    public void GetById_WhenMapIdEqualsCount_ShouldReturnsFailureResult()
    {
        // Arrange
        string expectedMessage = Messages.InvalidMap;

        // Act
        Result<IMap> result = _maps.GetById(_maps.Count);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(expectedMessage);
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    public void GetById_WhenMapIdIsValid_ShouldReturnsSuccessResult(int mapId)
    {
        // Act
        Result<IMap> result = _maps.GetById(mapId);

        // Asserts
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(mapId);
        result.Value.Name.Should().NotBeNullOrEmpty();
    }

    [Test]
    public void GetByName_WhenMapNameIsNotFound_ShouldReturnsFailureResult()
    {
        // Arrange
        string mapName = "NotFound";
        string expectedMessage = Messages.MapNotFound;

        // Act
        Result<IMap> result = _maps.GetByName(mapName);

        // Asserts
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(expectedMessage);
    }

    [TestCase("de_aztec")]
    [TestCase("DE_AZTEC")]
    [TestCase("De_Aztec")]
    public void GetByName_WhenMapNameIsFound_ShouldReturnsSuccessResult(string mapName)
    {
        // Arrange
        string expectedMapName = "de_aztec";

        // Act
        Result<IMap> result = _maps.GetByName(mapName);

        // Asserts
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be(expectedMapName);
    }

    [TestCase(0, 1)]
    [TestCase(1, 2)]
    [TestCase(2, 3)]
    [TestCase(3, 4)]
    [TestCase(4, 5)]
    [TestCase(5, 6)]
    [TestCase(6, 7)]
    [TestCase(7, 8)]
    [TestCase(31, 32)]
    public void GetNext_WhenMapExists_ShouldReturnsNextMap(int currentId, int expectedId)
    {
        // Arrange
        IMap current = _maps.GetById(currentId).Value;

        // Act
        IMap next = _maps.GetNext(current);

        // Assert
        next.Id.Should().Be(expectedId);
    }

    [Test]
    public void GetNext_WhenCurrentMapIsLast_ShouldWrapToFirstMap()
    {
        // Arrange
        IMap lastMap = _maps.GetById(_maps.Count - 1).Value;
        IMap firstMap = _maps.GetById(0).Value;

        // Act
        IMap next = _maps.GetNext(lastMap);

        // Assert
        next.Should().Be(firstMap);
    }
}