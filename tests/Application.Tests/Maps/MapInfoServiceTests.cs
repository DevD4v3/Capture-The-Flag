namespace CTF.Application.Tests.Maps;

public class MapInfoServiceTests
{
    private static readonly MapCollection s_maps = new(TestPaths.Maps);

    [Test]
    public void Constructor_WhenLoadMethodIsNotInvoked_CurrentMapShouldNotBeNull()
    {
        // Arrange

        // Act
        var service = new MapInfoService(s_maps, TestPaths.Maps);
        CurrentMap currentMap = service.CurrentMap;

        // Assert
        currentMap.Should().NotBeNull();
    }

    [Test]
    public void Load_WhenArgumentIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var service = new MapInfoService(s_maps, TestPaths.Maps);
        IMap map = default;

        // Act
        Action act = () => service.Load(map);

        // Assert
        act.Should()
           .Throw<ArgumentNullException>()
           .WithParameterName(nameof(map));
    }

    [TestCaseSource(typeof(MapInfoServiceTestCases))]
    public void Load_WhenMapIsLoadedFromFileSystem_ShouldCreateInstanceOfTypeCurrentMap(CurrentMap expectedCurrentMap)
    {
        // Arrange
        var service = new MapInfoService(s_maps, TestPaths.Maps);
        IMap map = s_maps.GetById(expectedCurrentMap.Id).Value;

        // Act
        service.Load(map);
        CurrentMap actual = service.CurrentMap;

        // Assert
        actual.Should().BeEquivalentTo(expectedCurrentMap);
    }

    private class MapInfoServiceTestCases : IEnumerable<CurrentMap>
    {
        public IEnumerator<CurrentMap> GetEnumerator()
        {
            yield return new CurrentMap(
                map: GetMap("Aim_Headshot"),
                alphaTeamLocations: 
                [
                    new SpawnLocation(-129.5612f,81.0056f,3.1172f,156.7189f),
                    new SpawnLocation(-127.6526f,87.7695f,3.1172f,156.7189f),
                    new SpawnLocation(-134.9525f,90.2646f,3.1172f,167.0590f),
                    new SpawnLocation(-138.4023f,83.5352f,3.1172f,163.6123f),
                    new SpawnLocation(-145.3216f,85.1476f,3.1172f,163.6123f),
                    new SpawnLocation(-152.4534f,80.4830f,3.1094f,163.6123f),
                    new SpawnLocation(-161.3326f,83.2152f,3.1094f,167.3724f),
                    new SpawnLocation(-159.6665f,98.1220f,3.1121f,165.8291f),
                    new SpawnLocation(-173.8980f,102.8791f,3.1668f,162.3824f),
                    new SpawnLocation(-186.4793f,93.0401f,3.1172f,162.3824f)
                ],
                betaTeamLocations:
                [
                    new SpawnLocation(-277.0338f,-85.0175f,2.8617f,345.0341f),
                    new SpawnLocation(-277.7510f,-90.4126f,2.7030f,345.0341f),
                    new SpawnLocation(-270.0297f,-92.0674f,3.0969f,345.0341f),
                    new SpawnLocation(-263.8904f,-93.2464f,3.1172f,345.0341f),
                    new SpawnLocation(-262.2849f,-87.2403f,3.1172f,345.0341f),
                    new SpawnLocation(-255.3565f,-84.1217f,3.1172f,345.0341f),
                    new SpawnLocation(-245.9794f,-86.1564f,3.1172f,345.0341f),
                    new SpawnLocation(-247.4499f,-99.9677f,3.1172f,345.0341f),
                    new SpawnLocation(-235.9863f,-102.5671f,3.1094f,345.3474f),
                    new SpawnLocation(-220.4884f,-110.9102f,3.1172f,352.5542f)
                ],
                flagLocations: new FlagLocations 
                { 
                    Red  = new Vector3(-151.8033f,52.8696f,4.2508f),
                    Blue = new Vector3(-247.5084f,-56.7737f,4.2713f)
                });

            yield return new CurrentMap(
                map: GetMap("RC_Battlefield"),
                alphaTeamLocations: 
                [
                    new SpawnLocation(-1136.4539f,1098.1666f,1345.8258f,269.4457f),
                    new SpawnLocation(-1135.1912f,1093.8064f,1345.8119f,267.8791f),
                    new SpawnLocation(-1135.6179f,1088.5378f,1345.8096f,267.8791f),
                    new SpawnLocation(-1135.4375f,1082.0457f,1345.8021f,267.8791f),
                    new SpawnLocation(-1135.2130f,1075.4181f,1345.7941f,267.8791f),
                    new SpawnLocation(-1135.0983f,1066.7756f,1345.7872f,267.8791f),
                    new SpawnLocation(-1129.9963f,1067.1566f,1345.7528f,274.1458f),
                    new SpawnLocation(-1129.5995f,1074.0201f,1345.7581f,274.1458f),
                    new SpawnLocation(-1129.0844f,1081.9758f,1345.7615f,268.5057f),
                    new SpawnLocation(-1131.0197f,1092.0566f,1345.7826f,264.1190f)
                ],
                betaTeamLocations:
                [
                    new SpawnLocation(-969.8113f,1021.9630f,1345.0767f,89.9275f),
                    new SpawnLocation(-970.1436f,1028.6224f,1345.0679f,88.9874f),
                    new SpawnLocation(-970.4197f,1036.5406f,1345.0582f,88.9874f),
                    new SpawnLocation(-969.6072f,1046.1462f,1345.0544f,88.9874f),
                    new SpawnLocation(-970.4572f,1055.3048f,1345.0397f,88.9874f),
                    new SpawnLocation(-976.7345f,1054.0677f,1344.9979f,90.8674f),
                    new SpawnLocation(-976.9385f,1044.5823f,1345.0059f,88.6741f),
                    new SpawnLocation(-977.0988f,1035.4301f,1345.0137f,88.6741f),
                    new SpawnLocation(-976.4217f,1024.6754f,1345.0288f,92.1208f),
                    new SpawnLocation(-976.2883f,1020.3914f,1345.0339f,93.3741f)
                ],
                flagLocations: new FlagLocations
                {
                    Red  = new Vector3(-1131.7461f,1029.1383f,1345.7311f),
                    Blue = new Vector3(-974.7156f,1089.7988f,1344.9755f)
                },
                interior: 10);

            yield return new CurrentMap(
                map: GetMap("TheBunker"),
                alphaTeamLocations:
                [
                    new SpawnLocation(592.8154f,-2433.9148f,10.8944f,79.0350f),
                    new SpawnLocation(593.3266f,-2430.6772f,10.8968f,75.2750f),
                    new SpawnLocation(593.7708f,-2427.1843f,10.9065f,81.8550f),
                    new SpawnLocation(594.2177f,-2425.0789f,10.9081f,78.0950f),
                    new SpawnLocation(591.5002f,-2424.4595f,10.9011f,77.1550f),
                    new SpawnLocation(590.2183f,-2427.9417f,10.8983f,77.1550f),
                    new SpawnLocation(588.6526f,-2431.8225f,10.8952f,77.1550f),
                    new SpawnLocation(584.6981f,-2430.9185f,10.9023f,348.4808f),
                    new SpawnLocation(575.6008f,-2417.5117f,10.9036f,252.9133f),
                    new SpawnLocation(584.4872f,-2417.1072f,10.9053f,166.1425f)
                ],
                betaTeamLocations:
                [
                    new SpawnLocation(891.1278f,-2397.3025f,19.8204f,87.1585f),
                    new SpawnLocation(889.6392f,-2407.8728f,20.2593f,80.5784f),
                    new SpawnLocation(887.4858f,-2418.1658f,21.0038f,80.5784f),
                    new SpawnLocation(885.3065f,-2425.9482f,21.5207f,49.8714f),
                    new SpawnLocation(875.5358f,-2419.4922f,21.7868f,67.4183f),
                    new SpawnLocation(876.6066f,-2412.1060f,22.0539f,80.2651f),
                    new SpawnLocation(877.5095f,-2404.7627f,22.3557f,80.2651f),
                    new SpawnLocation(878.8505f,-2396.0938f,22.6503f,92.4852f),
                    new SpawnLocation(870.1289f,-2393.8157f,23.9384f,92.4852f),
                    new SpawnLocation(864.5809f,-2400.9045f,25.0956f,94.6786f)
                ],
                flagLocations: new FlagLocations
                {
                    Red  = new Vector3(606.5786f,-2404.3359f,9.8766f),
                    Blue = new Vector3(755.5378f,-2410.6455f,30.1338f)
                },
                worldTime: 23);

            yield return new CurrentMap(
                map: GetMap("CrackFactory"),
                alphaTeamLocations:
                [
                    new SpawnLocation(2548.7009f,-1283.2224f,1060.9844f,230.3022f),
                    new SpawnLocation(2565.8301f,-1281.7773f,1065.3672f,238.1356f),
                    new SpawnLocation(2575.7759f,-1283.3206f,1065.3672f,177.9750f),
                    new SpawnLocation(2580.8525f,-1284.6443f,1065.3579f,88.0476f),
                    new SpawnLocation(2568.5518f,-1283.6564f,1060.9844f,181.0851f),
                    new SpawnLocation(2565.0220f,-1290.9614f,1060.9844f,224.6389f),
                    new SpawnLocation(2565.4963f,-1301.7297f,1060.9844f,275.0860f),
                    new SpawnLocation(2558.1384f,-1304.1283f,1060.9844f,272.2661f),
                    new SpawnLocation(2558.3372f,-1298.2233f,1060.9844f,272.2661f),
                    new SpawnLocation(2558.2466f,-1293.2781f,1062.0391f,260.6727f)
                ],
                betaTeamLocations:
                [
                    new SpawnLocation(2532.1660f,-1283.6971f,1031.4219f,270.0725f),
                    new SpawnLocation(2532.5823f,-1292.2178f,1031.4219f,275.7126f),
                    new SpawnLocation(2532.9485f,-1302.3477f,1031.4219f,269.4458f),
                    new SpawnLocation(2541.2852f,-1303.9135f,1031.4219f,269.4458f),
                    new SpawnLocation(2542.1389f,-1293.7726f,1031.4219f,262.8658f),
                    new SpawnLocation(2542.5986f,-1282.2380f,1031.4219f,264.1191f),
                    new SpawnLocation(2550.1204f,-1282.6653f,1031.4219f,263.8058f),
                    new SpawnLocation(2551.0386f,-1293.5071f,1031.4219f,277.2793f),
                    new SpawnLocation(2553.0278f,-1305.5601f,1031.4219f,277.2793f),
                    new SpawnLocation(2560.5957f,-1291.2533f,1031.4219f,265.3724f)
                ],
                flagLocations: new FlagLocations
                {
                    Red  = new Vector3(2579.7329f,-1285.4033f,1054.6406f),
                    Blue = new Vector3(2568.5762f,-1296.0222f,1031.4219f)
                },
                interior: 2,
                weather: 9,
                worldTime: 22);
        }

        private static IMap GetMap(string name) => s_maps.GetByName(name).Value;
        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();
    }
}
