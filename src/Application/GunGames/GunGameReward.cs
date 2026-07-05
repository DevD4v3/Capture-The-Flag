namespace CTF.Application.GunGames;

public class GunGameReward(PlayerStatsRenderer playerStatsRenderer)
{
    private const int WinnerHealth = 100;
    private const int WinnerArmour = 100;
    private const int WinnerCoins  = 100;
    private const int TeamHealth   = 50;
    private const int TeamArmour   = 50;
    private const int TeamCoins    = 15;
    private const int TeamScore    = 3;
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
            Rewards = $"+{WinnerHealth} Health, +{WinnerArmour} Armour, " +
                      $"+{WinnerCoins} Coins, +{weaponReward.Weapon.Name}"
        });
        PlayerInfo winnerInfo = winner.GetRequiredInfo();
        winner.AddHealth(WinnerHealth);
        winner.AddArmour(WinnerArmour);
        winner.GiveWeapon(weaponReward.Weapon.Id, weaponReward.Ammo);
        winnerInfo.StatsPerRound.AddCoins(WinnerCoins);
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
            Team = winnerInfo.Team.Name,
            Rewards = $"+{TeamHealth} Health, +{TeamArmour} Armour, " +
                      $"+{TeamCoins} Coins, +{TeamScore} Score"
        });

        foreach (Player teammate in winnerInfo.Team.Members)
        {
            if (teammate == winner)
                continue;

            PlayerInfo teammateInfo = teammate.GetRequiredInfo();
            teammate.AddHealth(TeamHealth);
            teammate.AddArmour(TeamArmour);
            teammate.AddScore(TeamScore);
            teammateInfo.StatsPerRound.AddCoins(TeamCoins);
            playerStatsRenderer.UpdateTextDraw(teammate);

            teammate.SendClientMessage(Color.LightGreen, teamRewardGranted);
            teammate.SendClientMessage(Color.LightGreen, teamRewardSummary);
        }
    }
}
