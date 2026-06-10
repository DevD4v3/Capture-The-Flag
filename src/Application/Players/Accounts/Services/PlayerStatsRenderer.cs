namespace CTF.Application.Players.Accounts.Services;

public class PlayerStatsRenderer(IWorldService worldService)
{
    public void CreateTextDraw(Player player)
    {
        ArgumentNullException.ThrowIfNull(player);
        bool isTextDrawCreated = player.GetComponent<PlayerStatsTextDraw>() is not null;
        if (isTextDrawCreated)
            return;

        PlayerTextDraw playerTextDraw = worldService.CreatePlayerTextDraw(
            player, 
            position: new Vector2(319.000000f, 433.000000f), 
            string.Empty
        );
        playerTextDraw.Font = TextDrawFont.Slim;
        playerTextDraw.LetterSize = new Vector2(0.279166f, 1.350000f);
        playerTextDraw.TextSize = new Vector2(12.000000f, 640.000000f);
        playerTextDraw.Outline = 1;
        playerTextDraw.Shadow = 0;
        playerTextDraw.Alignment = TextDrawAlignment.Center;
        playerTextDraw.ForeColor = new Color(-1);
        playerTextDraw.BackColor = new Color(255);
        playerTextDraw.BoxColor = new Color(101);
        playerTextDraw.UseBox = true;
        playerTextDraw.Proportional = true;
        playerTextDraw.Selectable = false;
        player.AddComponent<PlayerStatsTextDraw>(playerTextDraw);
    }

    public void UpdateTextDraw(Player player)
    {
        ArgumentNullException.ThrowIfNull(player);
        PlayerStatsTextDraw playerStatsTextDraw = GetTextDrawOrThrow(player);
        PlayerInfo playerInfo = player.GetInfo();
        playerStatsTextDraw.Value.Text = playerInfo.GetStatsAsText();
        playerStatsTextDraw.Value.Show();
    }

    public void ShowTextDraw(Player player)
    {
        ArgumentNullException.ThrowIfNull(player);
        PlayerStatsTextDraw playerStatsTextDraw = GetTextDrawOrThrow(player);
        playerStatsTextDraw.Value.Show();
    }

    public void HideTextDraw(Player player)
    {
        ArgumentNullException.ThrowIfNull(player);
        PlayerStatsTextDraw playerStatsTextDraw = GetTextDrawOrThrow(player);
        playerStatsTextDraw.Value.Hide();
    }

    private PlayerStatsTextDraw GetTextDrawOrThrow(Player player)
    {
        return player.GetComponent<PlayerStatsTextDraw>()
             ?? throw new InvalidOperationException($"The '{nameof(PlayerStatsTextDraw)}' component is not attached to the player");
    }

    private class PlayerStatsTextDraw : Component
    {
        public PlayerTextDraw Value { get; }
        public PlayerStatsTextDraw(PlayerTextDraw value)
        {
            ArgumentNullException.ThrowIfNull(value);
            Value = value;
        }
    }
}
