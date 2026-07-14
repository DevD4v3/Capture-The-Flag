namespace CTF.Application.Players.Accounts;

public interface IPlayerRepository
{
    PlayerInfo GetOrDefault(string name);
    bool Exists(string name);
    void Create(PlayerInfo player);
    void UpdateName(PlayerInfo player);
    void UpdatePassword(PlayerInfo player);
    void UpdateTotalKills(PlayerInfo player);
    void UpdateTotalDeaths(PlayerInfo player);
    void UpdateMaxKillingSpree(PlayerInfo player);
    void UpdateBroughtFlags(PlayerInfo player);
    void UpdateCapturedFlags(PlayerInfo player);
    void UpdateDroppedFlags(PlayerInfo player);
    void UpdateReturnedFlags(PlayerInfo player);
    void UpdateHeadShots(PlayerInfo player);
    void UpdateGunGameWins(PlayerInfo player);
    void UpdateRole(PlayerInfo player);
    void UpdateSkin(PlayerInfo player);
    void UpdateRank(PlayerInfo player);
    void UpdateLastConnection(PlayerInfo player);
}
