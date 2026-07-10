namespace CTF.Application.Players.Accounts.Statistics;

public class PlayerRankUpdater(
    IPlayerRepository playerRepository,
    IGunGameMode gunGameMode)
{
    private const int EarnedHealth = 100;
    private const int EarnedArmour = 100;
    private const int EarnedCoins  = 100;

    public void Update(Player player)
    {
        PlayerInfo playerInfo = player.GetRequiredInfo();
        if (!playerInfo.CanMoveUpToNextRank())
            return;

        IRank nextRank = RankCollection.GetNextRank(playerInfo.RankId).Value;
        playerInfo.SetRank(nextRank.Id);
        playerRepository.UpdateRank(playerInfo);
        player.SendClientMessage(Color.Yellow, Smart.Format(Messages.NextRank, nextRank));

        if (nextRank.IsMax())
        {
            var message = Smart.Format(Messages.PromotedToRole, new { RoleName = RoleId.VIP });
            player.GameText(message, TimeSpan.FromSeconds(4), GameTextStyle.Style3);
            player.SendClientMessage(Color.Orange, message);
            playerInfo.SetRole(RoleId.VIP);
            playerRepository.UpdateRole(playerInfo);
        }

        if (gunGameMode.IsEnabled)
            return;

        player.Armour = EarnedArmour;
        player.Health = EarnedHealth;
        playerInfo.StatsPerRound.AddCoins(EarnedCoins);

        var rankUpAwardSummary = Smart.Format(Messages.RankUpAwardSummary, new
        {
            Health = EarnedHealth,
            Armour = EarnedArmour,
            Coins  = EarnedCoins
        });

        player.SendClientMessage(Color.Orange, Messages.RankUpAwardGranted);
        player.SendClientMessage(Color.Orange, rankUpAwardSummary);
    }
}
