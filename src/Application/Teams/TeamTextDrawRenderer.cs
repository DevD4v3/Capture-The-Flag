namespace CTF.Application.Teams;

public class TeamTextDrawRenderer
{
    private readonly IWorldService _worldService;
    private TextDraw _redFlag;
    private TextDraw _blueFlag;
    private TextDraw _alphaScore;
    private TextDraw _betaScore;
    private TextDraw _redRic;
    private TextDraw _blueRic;
    private TextDraw _alphaTeamMembers;
    private TextDraw _betaTeamMembers;

    public TeamTextDrawRenderer(IWorldService worldService)
    {
        _worldService = worldService;
        Initialize();
    }

    public void Show(Player player)
    {
        _redFlag.Show(player);
        _blueFlag.Show(player);
        _alphaScore.Show(player);
        _betaScore.Show(player);
        _redRic.Show(player);
        _blueRic.Show(player);
        _alphaTeamMembers.Show(player);
        _betaTeamMembers.Show(player);
    }

    public void Hide(Player player)
    {
        _redFlag.Hide(player);
        _blueFlag.Hide(player);
        _alphaScore.Hide(player);
        _betaScore.Hide(player);
        _redRic.Hide(player);
        _blueRic.Hide(player);
        _alphaTeamMembers.Hide(player);
        _betaTeamMembers.Hide(player);
    }

    public void UpdateTeamScore(Team team)
    {
        if (team.Id == TeamId.Alpha)
        {
            _alphaScore.Text = team.GetScoreAsText();
        }
        else if (team.Id == TeamId.Beta)
        {
            _betaScore.Text = team.GetScoreAsText();
        }
    }

    public void UpdateTeamMembers(Team team)
    {
        if (team.Id == TeamId.Alpha)
        {
            _alphaTeamMembers.Text = team.GetMembersAsText();
        }
        else if (team.Id == TeamId.Beta)
        {
            _betaTeamMembers.Text = team.GetMembersAsText();
        }
    }

    private void Initialize()
    {
        _redFlag = _worldService.CreateTextDraw(new Vector2(-6.000000f, 302.000000f), string.Empty);
        _redFlag.Text = "Preview_Model";
        _redFlag.Font = TextDrawFont.PreviewModel;
        _redFlag.LetterSize = new Vector2(0.600000f, 2.000000f);
        _redFlag.TextSize = new Vector2(59.500000f, 50.000000f);
        _redFlag.Outline = 0;
        _redFlag.Shadow = 0;
        _redFlag.Alignment = TextDrawAlignment.Left;
        _redFlag.ForeColor = new Color(65535);
        _redFlag.BackColor = new Color(0);
        _redFlag.BoxColor = new Color(255);
        _redFlag.UseBox = false;
        _redFlag.Proportional = true;
        _redFlag.Selectable = false;
        _redFlag.PreviewModel = 19307;
        _redFlag.SetPreviewRotation(new Vector3(-10.000000f, 0.000000f, -20.000000f), zoom: 1.000000f);
        _redFlag.SetPreviewVehicleColor(1, 1);

        _blueFlag = _worldService.CreateTextDraw(new Vector2(-7.000000f, 264.000000f), string.Empty);
        _blueFlag.Text = "Preview_Model";
        _blueFlag.Font = TextDrawFont.PreviewModel;
        _blueFlag.LetterSize = new Vector2(0.600000f, 2.000000f);
        _blueFlag.TextSize = new Vector2(59.500000f, 50.500000f);
        _blueFlag.Outline = 0;
        _blueFlag.Shadow = 0;
        _blueFlag.Alignment = TextDrawAlignment.Left;
        _blueFlag.ForeColor = new Color(-16776961);
        _blueFlag.BackColor = new Color(0);
        _blueFlag.BoxColor = new Color(255);
        _blueFlag.UseBox = false;
        _blueFlag.Proportional = true;
        _blueFlag.Selectable = false;
        _blueFlag.PreviewModel = 19306;
        _blueFlag.SetPreviewRotation(new Vector3(-10.000000f, 0.000000f, -20.000000f), zoom: 1.000000f);
        _blueFlag.SetPreviewVehicleColor(1, 1);

        _alphaScore = _worldService.CreateTextDraw(new Vector2(46.000000f, 279.000000f), string.Empty);
        _alphaScore.Text = "Alpha: 0";
        _alphaScore.Font = TextDrawFont.Slim;
        _alphaScore.LetterSize = new Vector2(0.262497f, 1.299998f);
        _alphaScore.TextSize = new Vector2(400.000000f, 17.000000f);
        _alphaScore.Outline = 1;
        _alphaScore.Shadow = 0;
        _alphaScore.Alignment = TextDrawAlignment.Left;
        _alphaScore.ForeColor = new Color(-16776961);
        _alphaScore.BackColor = new Color(255);
        _alphaScore.BoxColor = new Color(0);
        _alphaScore.UseBox = true;
        _alphaScore.Proportional = true;
        _alphaScore.Selectable = false;

        _betaScore = _worldService.CreateTextDraw(new Vector2(46.000000f, 316.000000f), string.Empty);
        _betaScore.Text = "Beta: 0";
        _betaScore.Font = TextDrawFont.Slim;
        _betaScore.LetterSize = new Vector2(0.262497f, 1.299998f);
        _betaScore.TextSize = new Vector2(400.000000f, 17.000000f);
        _betaScore.Outline = 1;
        _betaScore.Shadow = 0;
        _betaScore.Alignment = TextDrawAlignment.Left;
        _betaScore.ForeColor = new Color(65535);
        _betaScore.BackColor = new Color(255);
        _betaScore.BoxColor = new Color(0);
        _betaScore.UseBox = true;
        _betaScore.Proportional = true;
        _betaScore.Selectable = false;

        _redRic = _worldService.CreateTextDraw(new Vector2(561.000000f, 332.000000f), string.Empty);
        // See https://dev.prineside.com/en/gtasa_samp_game_texture/view/LD_OTB2
        _redRic.Text = "LD_OTB2:ric2";
        _redRic.Font = TextDrawFont.DrawSprite;
        _redRic.LetterSize = new Vector2(0.600000f, 2.000000f);
        _redRic.TextSize = new Vector2(25.000000f, 33.500000f);
        _redRic.Outline = 1;
        _redRic.Shadow = 0;
        _redRic.Alignment = TextDrawAlignment.Left;
        _redRic.ForeColor = new Color(-16776961);
        _redRic.BackColor = new Color(255);
        _redRic.BoxColor = new Color(50);
        _redRic.UseBox = true;
        _redRic.Proportional = true;
        _redRic.Selectable = false;

        _blueRic = _worldService.CreateTextDraw(new Vector2(561.000000f, 362.000000f), string.Empty);
        // See https://dev.prineside.com/en/gtasa_samp_game_texture/view/LD_OTB2
        _blueRic.Text = "LD_OTB2:ric1";
        _blueRic.Font = TextDrawFont.DrawSprite;
        _blueRic.LetterSize = new Vector2(0.600000f, 2.000000f);
        _blueRic.TextSize = new Vector2(25.000000f, 33.500000f);
        _blueRic.Outline = 1;
        _blueRic.Shadow = 0;
        _blueRic.Alignment = TextDrawAlignment.Left;
        _blueRic.ForeColor = new Color(65535);
        _blueRic.BackColor = new Color(255);
        _blueRic.BoxColor = new Color(50);
        _blueRic.UseBox = true;
        _blueRic.Proportional = true;
        _blueRic.Selectable = false;

        _alphaTeamMembers = _worldService.CreateTextDraw(new Vector2(586.000000f, 344.000000f), string.Empty);
        _alphaTeamMembers.Text = "0";
        _alphaTeamMembers.Font = TextDrawFont.Slim;
        _alphaTeamMembers.LetterSize = new Vector2(0.320832f, 1.399999f);
        _alphaTeamMembers.TextSize = new Vector2(400.000000f, 17.000000f);
        _alphaTeamMembers.Outline = 1;
        _alphaTeamMembers.Shadow = 0;
        _alphaTeamMembers.Alignment = TextDrawAlignment.Left;
        _alphaTeamMembers.ForeColor = new Color(-16776961);
        _alphaTeamMembers.BackColor = new Color(255);
        _alphaTeamMembers.BoxColor = new Color(50);
        _alphaTeamMembers.UseBox = false;
        _alphaTeamMembers.Proportional = true;
        _alphaTeamMembers.Selectable = false;

        _betaTeamMembers = _worldService.CreateTextDraw(new Vector2(586.000000f, 373.000000f), string.Empty);
        _betaTeamMembers.Text = "0";
        _betaTeamMembers.Font = TextDrawFont.Slim;
        _betaTeamMembers.LetterSize = new Vector2(0.320832f, 1.399999f);
        _betaTeamMembers.TextSize = new Vector2(400.000000f, 17.000000f);
        _betaTeamMembers.Outline = 1;
        _betaTeamMembers.Shadow = 0;
        _betaTeamMembers.Alignment = TextDrawAlignment.Left;
        _betaTeamMembers.ForeColor = new Color(65535);
        _betaTeamMembers.BackColor = new Color(255);
        _betaTeamMembers.BoxColor = new Color(50);
        _betaTeamMembers.UseBox = false;
        _betaTeamMembers.Proportional = true;
        _betaTeamMembers.Selectable = false;
    }
}
