namespace CTF.Application.Teams.ClassSelection;

public class ClassSelectionSystem(
    IWorldService worldService,
    ClassSelectionTextDrawRenderer classSelectionTextDrawRenderer,
    TeamTextDrawRenderer teamTextDrawRenderer,
    ServerSettings serverSettings) : ISystem
{
    [Event]
    public void OnGameModeInit(IServerService serverService)
    {
        serverService.AddPlayerClass((int)Team.Alpha.SkinId, new Vector3(0f, 0f, 0f), 0);
        serverService.AddPlayerClass((int)Team.Beta.SkinId, new Vector3(0f, 0f, 0f), 0);
    }

    [Event]
    public void OnPlayerConnect(Player player)
    {
        player.Color = Team.None.ColorHex;
        player.AddComponent<ClassSelectionComponent>();
        player.RemoveAttachedObject(0);
        player.PlayAudioStream(serverSettings.IntroAudioUrl);
        classSelectionTextDrawRenderer.Show(player);
    }

    /// <summary>
    /// This callback is called when a player changes class at class selection (and when class selection first appears).
    /// </summary>
    [Event]
    public void OnPlayerRequestClass(Player player, Class @class)
    {
        if (player.HasForcedClassSelectionAfterDeath())
        {
            player.SetSpawnInfo(player.Team, player.Skin, player.Position, player.Angle);
            player.Spawn();
            return;
        }

        player.Color = Team.None.ColorHex;
        player.Position = new Vector3(-1389.137451f, 3314.043701f, 20.493314f);
        player.CameraPosition = new Vector3(-1399.776000f, 3310.254150f, 21.525623f);
        player.SetCameraLookAt(new Vector3(-1395.072143f, 3311.873291f, 22.027709f));
        player.Angle = 111.68f;
        player.Interior = 0;
        player.PlaySound(soundId: 1132);
        Team selectedTeam = @class.Id == (int)TeamId.Alpha ? Team.Alpha : Team.Beta;
        string gameText = selectedTeam.GetAvailabilityMessage();
        player.GameText(gameText, TimeSpan.FromMilliseconds(999999999), GameTextStyle.Style3);
        player.Team = (int)selectedTeam.Id;
    }

    /// <summary>
    /// This callback is called when a player attempts to spawn via class selection either 
    /// by pressing SHIFT or clicking the 'Spawn' button.
    /// </summary>
    [Event]
    public bool OnPlayerRequestSpawn(Player player)
    {
        Team selectedTeam = player.Team == (int)TeamId.Alpha ? Team.Alpha : Team.Beta;
        player.DisableClassSelection();
        player.HideGameText(style: 3);
        player.GetInfo().SetTeam(selectedTeam.Id);
        player.StopAudioStream();
        selectedTeam.Members.Add(player);
        classSelectionTextDrawRenderer.Hide(player);
        teamTextDrawRenderer.UpdateTeamMembers(selectedTeam);
        var message = Smart.Format(Messages.PlayerAddedToTeam, new
        {
            PlayerName = player.Name,
            TeamName = selectedTeam.Name
        });
        worldService.SendClientMessage(selectedTeam.ColorHex, message);
        return true;
    }

    [Event]
    public void OnPlayerDisconnect(Player player, DisconnectReason reason)
    {
        if (player.IsUnauthenticated())
            return;

        PlayerInfo playerInfo = player.GetInfo();
        if (playerInfo.Team == Team.None)
            return;

        playerInfo.Team.Members.Remove(player);
        teamTextDrawRenderer.UpdateTeamMembers(playerInfo.Team);
    }

    [PlayerCommand("class")]
    public void RedirectToClassSelection(Player player)
    {
        PlayerInfo playerInfo = player.GetInfo();
        if (playerInfo.HasCapturedFlag())
        {
            player.SendClientMessage(Color.Red, Messages.HasCapturedFlag);
            return;
        }

        if (player.Health < 85)
        {
            player.SendClientMessage(Color.Red, Messages.PlayerWithInsufficientHealth);
            return;
        }

        Team removedTeam = player.RemoveFromCurrentTeam();
        teamTextDrawRenderer.UpdateTeamMembers(removedTeam);
        player.RedirectToClassSelection();
    }
}
