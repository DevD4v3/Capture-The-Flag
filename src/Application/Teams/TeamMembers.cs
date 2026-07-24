namespace CTF.Application.Teams;

public class TeamMembers : IEnumerable<Player>
{
    private readonly Dictionary<int, Player> _players = [];
    public bool IsEmpty() => _players.Count == 0;
    public int Count => _players.Count;
    public void Clear() => _players.Clear();

    /// <summary>
    /// Removes the player from the team.
    /// </summary>
    /// <remarks>
    /// This method throws an <see cref="ArgumentException"/> if the player is not found.
    /// </remarks>
    /// <param name="player">The player to remove.</param>
    public void Remove(Player player)
    {
        bool playerIsNotFound = !_players.Remove(player.Id);
        if (playerIsNotFound)
        {
            var message = Smart.Format(Messages.PlayerNotFound, new { player.Name });
            throw new ArgumentException(message, nameof(player));
        }
    }

    /// <summary>
    /// Adds the player as a member of a team.
    /// </summary>
    /// <remarks>
    /// This method throws an <see cref="ArgumentException"/> if the member already exists.
    /// </remarks>
    /// <param name="player">The player to add.</param>
    public void Add(Player player)
    {
        bool exists = !_players.TryAdd(player.Id, player);
        if (exists)
        {
            var message = Smart.Format(Messages.MemberAlreadyExists, new { player.Name });
            throw new ArgumentException(message, nameof(player));
        }
    }

    public IEnumerator<Player> GetEnumerator() => _players.Values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}
