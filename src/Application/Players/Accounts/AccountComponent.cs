namespace CTF.Application.Players.Accounts;

public class AccountComponent : Component
{
    public PlayerInfo PlayerInfo { get; }
    public bool IsAuthenticated { get; private set; }
    public bool IsUnauthenticated => !IsAuthenticated;
    public void Authenticate() => IsAuthenticated = true;

    public AccountComponent(PlayerInfo playerInfo, bool isAuthenticated)
    {
        ArgumentNullException.ThrowIfNull(playerInfo);
        PlayerInfo = playerInfo;
        IsAuthenticated = isAuthenticated;
    }

    public AccountComponent(PlayerInfo playerInfo) 
        : this(playerInfo, isAuthenticated: false)
    {
    }
}
