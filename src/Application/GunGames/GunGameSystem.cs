namespace CTF.Application.GunGames;

public class GunGameSystem(
    IEntityManager entityManager,
    IWorldService worldService,
    IDialogService dialogService,
    IDictionary<GunGameResult, IGunGameResultHandler> handlers,
    WeaponProgression weaponProgression,
    GunGameSession gunGameSession,
    GunGameReward gunGameReward) : ISystem, IGunGameMode
{
    public bool IsEnabled { get; private set; }

    [Event]
    public void OnPlayerConnect(Player player)
    {
        player.AddComponent<PlayerProgression>();
    }

    [Event]
    public void OnPlayerSpawn(Player player)
    {
        if (!IsEnabled)
            return;

        var playerProgression = player.GetComponent<PlayerProgression>();
        player.GiveWeapon(Weapon.Knife, 1);

        IWeapon weapon = weaponProgression.GetWeapon(playerProgression.WeaponLevel);
        player.GiveWeapon(weapon.Id, IWeapon.UnlimitedAmmo);
    }

    [Event]
    public void OnPlayerDeath(Player victim, Player killer, Weapon reason)
    {
        if (!IsEnabled)
            return;

        if (killer is null)
            return;

        var killerProgression = killer.GetComponent<PlayerProgression>();
        var victimProgression = victim.GetComponent<PlayerProgression>();
        var gunGame = new GunGame(weaponProgression, gunGameSession.KillsRequiredPerLevel);

        GunGameResult result = gunGame.ProcessKill(killerProgression, victimProgression, reason);
        if (result == GunGameResult.None)
            return;

        IGunGameResultHandler handler = handlers[result];
        handler.Handle(new KillContext(victim, killer, reason));

        if (result == GunGameResult.ScoredFinalKill)
        {
            FinishGunGame();
            gunGameReward.Give(winner: killer);
        }
    }

    [Event]
    public void OnLoadingMap()
    {
        if (!IsEnabled)
            return; 

        var players = entityManager.GetComponents<PlayerProgression>();
        foreach (PlayerProgression player in players)
            player.Reset();
    }

    [PlayerCommand("gungameon")]
    public async Task GunGameOn(Player player, int killsRequiredPerLevel)
    {
        if (player.HasLowerRoleThan(RoleId.Moderator))
            return;

        if (IsEnabled)
        {
            player.SendClientMessage(Color.Red, GunGameMessages.GunGameModeAlreadyActive);
            return;
        }

        if (killsRequiredPerLevel < 1)
        {
            player.SendClientMessage(Color.Red, GunGameMessages.InvalidKillsRequiredPerLevel);
            return;
        }

        var dialog = new ListDialog("Select a Weapon Progression", "Select", "Close");
        foreach (WeaponProgressionType type in Enum.GetValues<WeaponProgressionType>())
            dialog.Add(type.GetDisplayName());

        ListDialogResponse response = await dialogService.ShowAsync(player, dialog);
        if (response.IsRightButtonOrDisconnected())
            return;

        gunGameSession.WeaponProgressionType = (WeaponProgressionType)response.ItemIndex;
        gunGameSession.KillsRequiredPerLevel = new KillsRequiredPerLevel(killsRequiredPerLevel);

        StartGunGame();
    }

    [PlayerCommand("gungameoff")]
    public void GunGameOff(Player player)
    {
        if (player.HasLowerRoleThan(RoleId.Moderator))
            return;

        if (!IsEnabled)
        {
            player.SendClientMessage(Color.Red, GunGameMessages.GunGameModeNotActive);
            return;
        }

        FinishGunGame();
        worldService.SendClientMessage(Color.Orange, GunGameMessages.GunGameModeDisabled);
    }

    private void StartGunGame()
    {
        IsEnabled = true;
        var players = entityManager.GetComponents<Player>();
        IWeapon initialWeapon = weaponProgression.GetWeapon(WeaponLevel.First);

        foreach (Player player in players)
        {
            player.GetComponent<PlayerProgression>().Reset();
            player.ResetWeapons();
            player.GiveWeapon(Weapon.Knife, 1);
            player.GiveWeapon(initialWeapon.Id, IWeapon.UnlimitedAmmo);
        }

        worldService.SendClientMessage(Color.Orange, GunGameMessages.GunGameModeStarted);
        worldService.SendClientMessage(Color.Orange, GunGameMessages.GunGameModeObjective);
        worldService.GameText(
            GunGameMessages.GunGameModeStartedGameText,
            TimeSpan.FromSeconds(5),
            GameTextStyle.Style3
        );
    }

    private void FinishGunGame()
    {
        IsEnabled = false;
        var players = entityManager.GetComponents<Player>();

        foreach (var player in players)
            RestorePlayerWeapons(player);

        worldService.GameText(
            GunGameMessages.GunGameModeFinishedGameText,
            TimeSpan.FromSeconds(5),
            GameTextStyle.Style3
        );
    }

    private static void RestorePlayerWeapons(Player player)
    {
        player.GetComponent<PlayerProgression>().Reset();
        player.ResetWeapons();
        var weaponSelection = player.GetComponent<WeaponSelectionComponent>();
        WeaponPack selectedWeapons = weaponSelection.SelectedWeapons;

        foreach (IWeapon weapon in selectedWeapons)
            player.GiveWeapon(weapon.Id, IWeapon.UnlimitedAmmo);
    }
}
