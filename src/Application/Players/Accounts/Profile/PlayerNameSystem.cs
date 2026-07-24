namespace CTF.Application.Players.Accounts.Profile;

public class PlayerNameSystem(
    IPlayerRepository playerRepository,
    IWorldService worldService) : ISystem
{
    [PlayerCommand("changename")]
    public void ChangeName(Player player, string newName)
    {
        if (playerRepository.Exists(newName))
        {
            player.SendClientMessage(Color.Red, Messages.PlayerNameAlreadyExists);
            return;
        }

        PlayerInfo playerInfo = player.GetRequiredInfo();
        string oldName = playerInfo.Name;
        Result result = playerInfo.SetName(newName);
        if (result.IsFailed)
        {
            player.SendClientMessage(Color.Red, result.Message);
            return;
        }

        var message = Smart.Format(Messages.NameSuccessfullyChanged, new { OldName = oldName, NewName = newName });
        worldService.SendClientMessage(Color.Yellow, message);
        player.SetName(newName);
        playerRepository.UpdateName(playerInfo);
    }
}
