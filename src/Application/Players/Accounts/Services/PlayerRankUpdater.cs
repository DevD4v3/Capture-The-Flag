namespace CTF.Application.Players.Accounts.Services;

public class PlayerRankUpdater(
    IPlayerRepository playerRepository,
    IGunGameMode gunGameMode)
{
    public void Update(Player player, PlayerInfo playerInfo)
    {
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

        player.Armour = 100;
        player.Health = 100;
        playerInfo.StatsPerRound.AddCoins(100);
        player.SendClientMessage(Color.Orange, Messages.RankUpAward);
    }
}
