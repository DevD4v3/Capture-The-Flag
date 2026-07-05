namespace CTF.Application.Players;

public class PlayerDeathSystem(IWorldService worldService) : ISystem
{
    [Event]
    public void OnPlayerConnect(Player player)
    {
        worldService.SendDeathMessage(killer: null, player, Weapon.Connect);
    }

    [Event]
    public void OnPlayerDisconnect(Player player, DisconnectReason reason) 
    {
        worldService.SendDeathMessage(killer: null, player, Weapon.Disconnect);
    }

    [Event]
    public void OnPlayerDeath(Player victim, Player killer, Weapon reason)
    {
        worldService.SendDeathMessage(killer, victim, reason);
    }
}
