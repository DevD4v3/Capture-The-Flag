namespace Persistence.MariaDB;

internal class PlayerRepository(
    IPasswordHasher passwordHasher,
    ISqlCollection sqlCollection,
    MariaDbSettings settings) : IPlayerRepository
{
    public void Create(PlayerInfo player)
    {
        var passwordHash = passwordHasher.HashPassword(player.Password);
        using var connection = new MySqlConnection(settings.ConnectionString);
        connection.Open();

        MySqlCommand command = connection.CreateCommand();
        command.CommandText = sqlCollection["CreatePlayer"];
        command.Parameters.AddWithValue("@name",              player.Name);
        command.Parameters.AddWithValue("@password",          passwordHash);
        command.Parameters.AddWithValue("@total_kills",       player.TotalKills);
        command.Parameters.AddWithValue("@total_deaths",      player.TotalDeaths);
        command.Parameters.AddWithValue("@max_killing_spree", player.MaxKillingSpree);
        command.Parameters.AddWithValue("@brought_flags",     player.BroughtFlags);
        command.Parameters.AddWithValue("@captured_flags",    player.CapturedFlags);
        command.Parameters.AddWithValue("@dropped_flags",     player.DroppedFlags);
        command.Parameters.AddWithValue("@returned_flags",    player.ReturnedFlags);
        command.Parameters.AddWithValue("@head_shots",        player.HeadShots);
        command.Parameters.AddWithValue("@gungame_wins",      player.GunGameWins);
        command.Parameters.AddWithValue("@role_id",           player.RoleId.ToString());
        command.Parameters.AddWithValue("@skin_id",           player.SkinId);
        command.Parameters.AddWithValue("@rank_id",           player.RankId);
        command.Parameters.AddWithValue("@created_at",        player.CreatedAt);
        command.Parameters.AddWithValue("@last_connection",   player.LastConnection);
        int id = (int)(ulong)command.ExecuteScalar();

        // The Account ID is immutable and lacks a public setter; Reflection is used to modify it.
        player.SetValue(value: id, propertyName: nameof(PlayerInfo.AccountId));
    }

    public bool Exists(string name)
    {
        using var connection = new MySqlConnection(settings.ConnectionString);
        connection.Open();
        MySqlCommand command = connection.CreateCommand();
        command.CommandText = sqlCollection["PlayerExists"];
        command.Parameters.AddWithValue("@name", name);
        using MySqlDataReader reader = command.ExecuteReader();
        return reader.HasRows;
    }

    public PlayerInfo GetOrDefault(string name)
    {
        using var connection = new MySqlConnection(settings.ConnectionString);
        connection.Open();

        MySqlCommand command = connection.CreateCommand();
        command.CommandText = sqlCollection["GetPlayerByName"];
        command.Parameters.AddWithValue("@name", name);

        using MySqlDataReader reader = command.ExecuteReader();
        bool notExists = !reader.Read();
        if (notExists)
            return default;

        var playerInfo = new PlayerInfo();
        // The public setter is used only for plaintext passwords.
        // For that reason, we use Reflection to set the already encrypted password.
        playerInfo.SetValue(value: reader.GetString("password"), propertyName: nameof(PlayerInfo.Password));

        playerInfo.SetName(reader.GetString("name"));
        playerInfo.SetTotalKills(reader.GetInt32("total_kills"));
        playerInfo.SetTotalDeaths(reader.GetInt32("total_deaths"));
        playerInfo.SetMaxKillingSpree(reader.GetInt32("max_killing_spree"));
        var roleId = Enum.Parse<RoleId>(reader.GetString("role_id"));
        playerInfo.SetRole(roleId);
        playerInfo.SetRank((RankId)reader.GetInt32("rank_id"));
        playerInfo.SetSkin(reader.GetInt32("skin_id"));
 
        // Reflection is used here because these properties are immutable.
        // What we did here is what ORMs like EF Core do, so it's nothing new.
        playerInfo.SetValue(value: reader.GetInt32("id"),                 propertyName: nameof(PlayerInfo.AccountId));
        playerInfo.SetValue(value: reader.GetInt32("brought_flags"),      propertyName: nameof(PlayerInfo.BroughtFlags));
        playerInfo.SetValue(value: reader.GetInt32("captured_flags"),     propertyName: nameof(PlayerInfo.CapturedFlags));
        playerInfo.SetValue(value: reader.GetInt32("dropped_flags"),      propertyName: nameof(PlayerInfo.DroppedFlags));
        playerInfo.SetValue(value: reader.GetInt32("returned_flags"),     propertyName: nameof(PlayerInfo.ReturnedFlags));
        playerInfo.SetValue(value: reader.GetInt32("head_shots"),         propertyName: nameof(PlayerInfo.HeadShots));
        playerInfo.SetValue(value: reader.GetInt32("gungame_wins"),       propertyName: nameof(PlayerInfo.GunGameWins));
        playerInfo.SetValue(value: reader.GetDateTime("created_at"),      propertyName: nameof(PlayerInfo.CreatedAt));
        playerInfo.SetValue(value: reader.GetDateTime("last_connection"), propertyName: nameof(PlayerInfo.LastConnection));
        return playerInfo;
    }

    public void UpdateBroughtFlags(PlayerInfo player)
        => Update(player.AccountId, "brought_flags", player.BroughtFlags);

    public void UpdateCapturedFlags(PlayerInfo player)
        => Update(player.AccountId, "captured_flags", player.CapturedFlags);

    public void UpdateDroppedFlags(PlayerInfo player)
        => Update(player.AccountId, "dropped_flags", player.DroppedFlags);

    public void UpdateReturnedFlags(PlayerInfo player)
        => Update(player.AccountId, "returned_flags", player.ReturnedFlags);

    public void UpdateHeadShots(PlayerInfo player)
        => Update(player.AccountId, "head_shots", player.HeadShots);

    public void UpdateGunGameWins(PlayerInfo player)
        => Update(player.AccountId, "gungame_wins", player.GunGameWins);

    public void UpdateLastConnection(PlayerInfo player)
        => Update(player.AccountId, "last_connection", player.LastConnection);

    public void UpdateMaxKillingSpree(PlayerInfo player)
        => Update(player.AccountId, "max_killing_spree", player.MaxKillingSpree);

    public void UpdateName(PlayerInfo player)
        => Update(player.AccountId, "name", player.Name);

    public void UpdatePassword(PlayerInfo player)
    {
        var passwordHash = passwordHasher.HashPassword(player.Password);
        Update(player.AccountId, "password", passwordHash);
    }

    public void UpdateRank(PlayerInfo player)
        => Update(player.AccountId, "rank_id", player.RankId);

    public void UpdateRole(PlayerInfo player)
        => Update(player.AccountId, "role_id", player.RoleId.ToString());

    public void UpdateSkin(PlayerInfo player)
        => Update(player.AccountId, "skin_id", player.SkinId);

    public void UpdateTotalDeaths(PlayerInfo player)
        => Update(player.AccountId, "total_deaths", player.TotalDeaths);

    public void UpdateTotalKills(PlayerInfo player)
        => Update(player.AccountId, "total_kills", player.TotalKills);

    private void Update(int id, string columnName, object value)
    {
        using var connection = new MySqlConnection(settings.ConnectionString);
        connection.Open();

        MySqlCommand command = connection.CreateCommand();
        command.CommandText = $"UPDATE players SET {columnName} = @column_value WHERE id = @id";
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@column_value", value);
        command.ExecuteNonQuery();
    }
}
