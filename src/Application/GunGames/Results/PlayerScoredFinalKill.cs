namespace CTF.Application.GunGames.Results;

/// <summary>
/// Handles the <see cref="GunGameResult.ScoredFinalKill"/> result.
/// </summary>
public class PlayerScoredFinalKill(
    IWorldService worldService,
    IPlayerRepository playerRepository) : IGunGameResultHandler
{
    public GunGameResult Result => GunGameResult.ScoredFinalKill;

    public void Handle(KillContext context)
    {
        PlayerInfo killerInfo = context.Killer.GetRequiredInfo();
        killerInfo.AddGunGameWins();
        playerRepository.UpdateGunGameWins(killerInfo);

        var message = Smart.Format(GunGameMessages.PlayerScoredFinalKill, new
        {
            Killer = context.Killer.Name
        });

        worldService.SendClientMessage(Color.Gold, message);
    }
}
