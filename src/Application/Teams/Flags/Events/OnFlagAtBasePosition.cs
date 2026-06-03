namespace CTF.Application.Teams.Flags.Events;

/// <summary>
/// This event occurs when a player attempts to pick up their own team's flag, which is currently at the base position.
/// </summary>
public class OnFlagAtBasePosition : IFlagEvent
{
    public FlagStatus FlagStatus => FlagStatus.BasePosition;

    public void Handle(Team team, Player player)
    {
        var text = Smart.Format(Messages.OnFlagAtBasePosition, new { team.GameText });
        player.GameText(text, TimeSpan.FromSeconds(5), GameTextStyle.Style3);
    }
}
