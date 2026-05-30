namespace CTF.Application.Maps.Services;

public class MapTextDrawRenderer
{
    private readonly IWorldService _worldService;
    private TextDraw _mapName;
    private TextDraw _timer;
    private TextDraw _timeLeft;
    private TextDraw _loadTime;

    public MapTextDrawRenderer(IWorldService worldService)
    {
        _worldService = worldService;
        Initialize();
    }

    public void Show(Player player)
    {
        _mapName.Show(player);
        _timer.Show(player);
        _timeLeft.Show(player);
        _loadTime.Show();
    }

    public void Hide(Player player) 
    {
        _mapName.Hide(player);
        _timer.Hide(player);
        _timeLeft.Hide(player);
        _loadTime.Hide(player);
    }

    public void UpdateMapName(CurrentMap currentMap)
    {
        _mapName.Text = currentMap.GetMapNameAsText();
    }

    public void UpdateTimeLeft(TimeLeft timeLeft)
    {
        _timeLeft.Text = timeLeft.TextDraw;
    }

    public void UpdateLoadTime(LoadTime loadTime)
    {
        _loadTime.Text = loadTime.GameText;
    }

    private void Initialize()
    {
        _mapName = _worldService.CreateTextDraw(new Vector2(140.000000f, 399.000000f), string.Empty);
        _mapName.Text = "Map: ~w~RC Battlefield";
        _mapName.Font = TextDrawFont.Normal;
        _mapName.LetterSize = new Vector2(0.329166f, 1.900000f);
        _mapName.TextSize = new Vector2(400.000000f, 17.000000f);
        _mapName.Outline = 1;
        _mapName.Shadow = 0;
        _mapName.Alignment = TextDrawAlignment.Left;
        _mapName.ForeColor = new Color(-294256385);
        _mapName.BackColor = new Color(255);
        _mapName.BoxColor = new Color(50);
        _mapName.UseBox = false;
        _mapName.Proportional = true;
        _mapName.Selectable = false;

        _timer = _worldService.CreateTextDraw(new Vector2(566.000000f, 395.000000f), string.Empty);
        _timer.Text = "ld_grav:timer";
        _timer.Font = TextDrawFont.DrawSprite;
        _timer.LetterSize = new Vector2(0.600000f, 2.000000f);
        _timer.TextSize = new Vector2(17.000000f, 17.000000f);
        _timer.Outline = 1;
        _timer.Shadow = 0;
        _timer.Alignment = TextDrawAlignment.Left;
        _timer.ForeColor = new Color(-1);
        _timer.BackColor = new Color(255);
        _timer.BoxColor = new Color(50);
        _timer.UseBox = true;
        _timer.Proportional = true;
        _timer.Selectable = false;

        _timeLeft = _worldService.CreateTextDraw(new Vector2(586.000000f, 397.000000f), string.Empty);
        _timeLeft.Text = "00:00";
        _timeLeft.Font = TextDrawFont.Slim;
        _timeLeft.LetterSize = new Vector2(0.370833f, 1.500000f);
        _timeLeft.TextSize = new Vector2(400.000000f, 17.000000f);
        _timeLeft.Outline = 1;
        _timeLeft.Shadow = 0;
        _timeLeft.Alignment = TextDrawAlignment.Left;
        _timeLeft.ForeColor = new Color(-1);
        _timeLeft.BackColor = new Color(255);
        _timeLeft.BoxColor = new Color(0);
        _timeLeft.UseBox = true;
        _timeLeft.Proportional = true;
        _timeLeft.Selectable = false;

        _loadTime = _worldService.CreateTextDraw(new Vector2(211.000000f, 130.000000f), string.Empty);
        _loadTime.Text = string.Empty;
        _loadTime.Font = TextDrawFont.Slim;
        _loadTime.LetterSize = new Vector2(0.566666f, 2.599997f);
        _loadTime.TextSize = new Vector2(503.500000f, 16.000000f);
        _loadTime.Outline = 2;
        _loadTime.Shadow = 1;
        _loadTime.Alignment = TextDrawAlignment.Left;
        _loadTime.ForeColor = new Color(-764862722);
        _loadTime.BackColor = new Color(255);
        _loadTime.BoxColor = new Color(50);
        _loadTime.Proportional = true;
        _loadTime.Selectable = false;
    }
}
