namespace Persistence.SQLite;

public static class PersistenceSQLiteServicesExtensions
{
    public static IServiceCollection AddPersistenceSQLiteServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var sqliteSettings = configuration
            .GetRequiredSection("SQLite")
            .Get<SQLiteSettings>();

        var connectionString = new SqliteConnectionStringBuilder()
        {
            DataSource = sqliteSettings.DataSource
        }.ToString();

        sqliteSettings.ConnectionString = connectionString;
        services.AddSingleton(sqliteSettings)
                .AddSingleton<IPlayerRepository, PlayerRepository>()
                .AddSingleton<ITopPlayersRepository, TopPlayersRepository>();

        var path = Path.Combine(GameModePaths.Sql, typeof(PersistenceSQLiteServicesExtensions).Namespace);
        ISqlCollection sqlCollection = new YeSqlLoader().LoadFromDirectories(path);
        services.AddSingleton(sqlCollection);
        return services;
    }
}
