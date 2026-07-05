namespace CTF.Application.GunGames.Results;

/// <summary>
/// Handles the <see cref="GunGameResult.ScoredFinalKill"/> result.
/// </summary>
public class PlayerScoredFinalKill(
    IWorldService worldService) : IGunGameResultHandler
{
    public GunGameResult Result => GunGameResult.ScoredFinalKill;

    public void Handle(KillContext context)
    {
        var message = Smart.Format(GunGameMessages.PlayerScoredFinalKill, new
        {
            Killer = context.Killer.Name
        });
        worldService.SendClientMessage(Color.Gold, message);
    }
}
