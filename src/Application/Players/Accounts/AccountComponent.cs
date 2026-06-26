namespace CTF.Application.Players.Accounts;

public class AccountComponent : Component
{
    public PlayerInfo PlayerInfo { get; }
    public bool IsAuthenticated { get; private set; }
    public bool IsUnauthenticated => !IsAuthenticated;
    public void Authenticate() => IsAuthenticated = true;

    public AccountComponent(PlayerInfo playerInfo, bool isAuthenticated = false)
    {
        ArgumentNullException.ThrowIfNull(playerInfo);
        PlayerInfo = playerInfo;
        IsAuthenticated = isAuthenticated;
    }
}
