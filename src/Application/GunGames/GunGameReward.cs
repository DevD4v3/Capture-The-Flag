namespace CTF.Application.GunGames;

public class GunGameReward(PlayerStatsRenderer playerStatsRenderer)
{
    private const int WinnerEarnedHealth = 100;
    private const int WinnerEarnedArmour = 100;
    private const int WinnerEarnedCoins  = 100;
    private const int TeamEarnedHealth   = 50;
    private const int TeamEarnedArmour   = 50;
    private const int TeamEarnedCoins    = 15;
    private const int TeamEarnedScore    = 3;
    private readonly record struct WeaponReward(IWeapon Weapon, int Ammo);
    private readonly WeaponReward[] _weaponRewards = 
    [
        new(WeaponDefinitions.Grenade,       Ammo: 5),
        new(WeaponDefinitions.Molotov,       Ammo: 5),
        new(WeaponDefinitions.SatchelCharge, Ammo: 5),
        new(WeaponDefinitions.Flamethrower,  Ammo: 1000)
    ];

    public void Give(Player winner)
    {
        var weaponReward = _weaponRewards[Random.Shared.Next(_weaponRewards.Length)];
        var winnerRewardSummary = Smart.Format(GunGameMessages.WinnerRewardSummary, new
        {
            Health = WinnerEarnedHealth,
            Armour = WinnerEarnedArmour,
            Coins  = WinnerEarnedCoins,
            Weapon = weaponReward.Weapon.Name
        });
        PlayerInfo winnerInfo = winner.GetRequiredInfo();
        winner.AddHealth(WinnerEarnedHealth);
        winner.AddArmour(WinnerEarnedArmour);
        winner.GiveWeapon(weaponReward.Weapon.Id, weaponReward.Ammo);

        // Although the detonator is automatically available when Satchel Charges
        // are given, its HUD icon is not displayed unless it is explicitly granted.
        if (weaponReward.Weapon.Id == Weapon.SatchelCharge)
            winner.GiveWeapon(Weapon.Detonator, 1);

        winnerInfo.StatsPerRound.AddCoins(WinnerEarnedCoins);
        playerStatsRenderer.UpdateTextDraw(winner);

        winner.SendClientMessage(Color.Yellow, GunGameMessages.WinnerRewardGranted);
        winner.SendClientMessage(Color.Yellow, winnerRewardSummary);

        var teamRewardGranted = Smart.Format(GunGameMessages.TeamRewardGranted, new
        {
            Team = winnerInfo.Team.Name,
            Killer = winner.Name
        });

        var teamRewardSummary = Smart.Format(GunGameMessages.TeamRewardSummary, new
        {
            Team   = winnerInfo.Team.Name,
            Health = TeamEarnedHealth,
            Armour = TeamEarnedArmour,
            Coins  = TeamEarnedCoins,
            Score  = TeamEarnedScore
        });

        foreach (Player teammate in winnerInfo.Team.Members)
        {
            if (teammate == winner)
                continue;

            PlayerInfo teammateInfo = teammate.GetRequiredInfo();
            teammate.AddHealth(TeamEarnedHealth);
            teammate.AddArmour(TeamEarnedArmour);
            teammate.AddScore(TeamEarnedScore);
            teammateInfo.StatsPerRound.AddCoins(TeamEarnedCoins);
            playerStatsRenderer.UpdateTextDraw(teammate);

            teammate.SendClientMessage(Color.LightGreen, teamRewardGranted);
            teammate.SendClientMessage(Color.LightGreen, teamRewardSummary);
        }
    }
}
