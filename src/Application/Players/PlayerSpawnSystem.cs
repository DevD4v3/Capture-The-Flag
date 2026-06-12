namespace CTF.Application.Players;

public class PlayerSpawnSystem(
    MapInfoService mapInfoService,
    MapRotationService mapRotationService) : ISystem
{
    [Event]
    public void OnPlayerSpawn(Player player)
    {
        CurrentMap currentMap = mapInfoService.CurrentMap;
        PlayerInfo playerInfo = player.GetInfo();
        SpawnLocation spawnLocation = currentMap.GetRandomSpawnLocation(playerInfo.Team.Id);
        player.Position = spawnLocation.Position;
        player.Angle = spawnLocation.Angle;
        player.Interior = currentMap.Interior;
        player.Color = playerInfo.Team.ColorHex;
        player.Team = (int)playerInfo.Team.Id;
        player.Skin = (int)playerInfo.Team.SkinId;
        if (playerInfo.HasSkin())
        {
            player.Skin = playerInfo.SkinId;
        }
        if (mapRotationService.IsMapLoading)
        {
            player.ToggleSpectating(true);
        }
    }
}
