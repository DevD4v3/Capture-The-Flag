namespace CTF.Application.Teams.ClassSelection;

public class ClassSelectionTextDrawRenderer
{
    private readonly IWorldService _worldService;
    private TextDraw _gameModeTitle;
    private TextDraw _gameModeDescription;
    private TextDraw _commandList;
    private TextDraw _blueCommandListBox;

    public ClassSelectionTextDrawRenderer(IWorldService worldService)
    {
        _worldService = worldService;
        Initialize();
    }

    public void Show(Player player)
    {
        _gameModeDescription.Show(player);
        _commandList.Show(player);
        _blueCommandListBox.Show(player);
        _gameModeTitle.Show(player);
    }

    public void Hide(Player player)
    {
        _gameModeDescription.Hide(player);
        _commandList.Hide(player);
        _blueCommandListBox.Hide(player);
    }

    private void Initialize()
    {
        _gameModeTitle = _worldService.CreateTextDraw(new Vector2(483.000000f, 4.000000f), string.Empty);
        _gameModeTitle.Text = "Capture The Flag";
        _gameModeTitle.Font = TextDrawFont.Diploma;
        _gameModeTitle.LetterSize = new Vector2(0.680000f, 1.799998f);
        _gameModeTitle.BackColor = 255;
        _gameModeTitle.ForeColor = Color.Yellow;
        _gameModeTitle.Outline = 1;
        _gameModeTitle.Proportional = true;

        _gameModeDescription = _worldService.CreateTextDraw(new Vector2(18.000000f, 225.000000f), string.Empty);
        _gameModeDescription.Text = Messages.GameModeDescription;
        _gameModeDescription.Font = TextDrawFont.Normal;
        _gameModeDescription.LetterSize = new Vector2(0.320833f, 1.300000f);
        _gameModeDescription.TextSize = new Vector2(227.000000f, 42.000000f);
        _gameModeDescription.Outline = 1;
        _gameModeDescription.Shadow = 0;
        _gameModeDescription.Alignment = TextDrawAlignment.Left;
        _gameModeDescription.ForeColor = new Color(-1);
        _gameModeDescription.BackColor = new Color(255);
        _gameModeDescription.BoxColor = new Color(65368);
        _gameModeDescription.UseBox = true;
        _gameModeDescription.Proportional = true;
        _gameModeDescription.Selectable = false;

        _commandList = _worldService.CreateTextDraw(new Vector2(616.000000f, 431.000000f), string.Empty);
        _commandList.Text = "~r~/cmds ~y~/help ~p~~h~/weapons ~g~/pack ~r~/combos ~y~/stats ~p~~h~/tstats ~g~/team ~w~/admins";
        _commandList.Font = TextDrawFont.Pricedown;
        _commandList.LetterSize = new Vector2(0.479166f, 1.500000f);
        _commandList.TextSize = new Vector2(400.000000f, 17.000000f);
        _commandList.Outline = 1;
        _commandList.Shadow = 0;
        _commandList.Alignment = TextDrawAlignment.Right;
        _commandList.ForeColor = new Color(-1);
        _commandList.BackColor = new Color(255);
        _commandList.BoxColor = new Color(50);
        _commandList.UseBox = true;
        _commandList.Proportional = true;
        _commandList.Selectable = false;

        _blueCommandListBox = _worldService.CreateTextDraw(new Vector2(319.000000f, 430.000000f), "_");
        _blueCommandListBox.Font = TextDrawFont.Normal;
        _blueCommandListBox.LetterSize = new Vector2(0.612500f, 1.649996f);
        _blueCommandListBox.TextSize = new Vector2(303.000000f, 633.000000f);
        _blueCommandListBox.Outline = 1;
        _blueCommandListBox.Shadow = 0;
        _blueCommandListBox.Alignment = TextDrawAlignment.Center;
        _blueCommandListBox.ForeColor = new Color(65535);
        _blueCommandListBox.BackColor = new Color(255);
        _blueCommandListBox.BoxColor = new Color(65368);
        _blueCommandListBox.UseBox = true;
        _blueCommandListBox.Proportional = true;
        _blueCommandListBox.Selectable = false;
    }
}
