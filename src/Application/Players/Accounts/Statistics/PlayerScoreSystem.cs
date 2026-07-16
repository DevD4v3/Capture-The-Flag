namespace CTF.Application.Players.Accounts.Statistics;

public class PlayerScoreSystem(
    IEntityManager entityManager,
    IWorldService worldService) : ISystem
{
    [PlayerCommand("setscore")]
    [RequiresRole(RoleId.Admin)]
    public void SetScoreToPlayer(
        Player currentPlayer, 
        [CommandParameter(Name = "playerId")]Player targetPlayer,
        int score)
    {
        if (score < 0)
        {
            currentPlayer.SendClientMessage(Color.Red, Messages.ValueCannotBeNegative);
            return;
        }

        targetPlayer.SetScore(score);

        {
            var message = Smart.Format(Messages.SetScoreToPlayer, new
            {
                PlayerName = targetPlayer.Name,
                Score = score
            });
            currentPlayer.SendClientMessage(Color.Yellow, message);
        }

        {
            var message = Smart.Format(Messages.ReceiveScoreFromPlayer, new
            {
                PlayerName = currentPlayer.Name,
                Score = score
            });
            targetPlayer.SendClientMessage(Color.Yellow, message);
        }
    }

    [PlayerCommand("addscore")]
    [RequiresRole(RoleId.Admin)]
    public void AddScoreToPlayer(
        Player currentPlayer, 
        [CommandParameter(Name = "playerId")]Player targetPlayer,
        int score)
    {
        if (score < 0)
        {
            currentPlayer.SendClientMessage(Color.Red, Messages.ValueCannotBeNegative);
            return;
        }

        targetPlayer.AddScore(score);

        {
            var message = Smart.Format(Messages.AddScoreToPlayer, new
            {
                PlayerName = targetPlayer.Name,
                Score = score
            });
            currentPlayer.SendClientMessage(Color.Yellow, message);
        }

        {
            var message = Smart.Format(Messages.ReceiveScoreFromPlayer, new
            {
                PlayerName = currentPlayer.Name,
                Score = score
            });
            targetPlayer.SendClientMessage(Color.Yellow, message);
        }
    }

    [PlayerCommand("addallscore")]
    [RequiresRole(RoleId.Admin)]
    public void AddScoreToAllPlayers(Player currentPlayer, int score)
    {
        if (score < 0)
        {
            currentPlayer.SendClientMessage(Color.Red, Messages.ValueCannotBeNegative);
            return;
        }

        IEnumerable<Player> players = entityManager.GetComponents<Player>();
        foreach (Player targetPlayer in players)
        {
            targetPlayer.AddScore(score);
        }

        var message = Smart.Format(Messages.AddScoreToAllPlayers, new
        {
            PlayerName = currentPlayer.Name,
            Score = score
        });

        worldService.SendClientMessage(Color.Yellow, message);
    }
}
