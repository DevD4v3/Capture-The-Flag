namespace Persistence.SQLite;

public static class PersistenceSQLiteServicesExtensions
{
    public static IServiceCollection AddPersistenceSQLiteServices(
        this IServiceCollection services, 
        IConfiguration configuration,
        string sqlBasePath)
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

        var sqlPath = Path.Combine(sqlBasePath, typeof(PersistenceSQLiteServicesExtensions).Namespace, "sql");
        ISqlCollection sqlCollection = new YeSqlLoader()
            .Exclude("schema.sql")
            .LoadFromDirectories(sqlPath);

        var schemaFile = Path.Combine(sqlPath, "schema.sql");
        SQLiteSchemaExecutor.Execute(connectionString, schemaFile);
        services.AddSingleton(sqlCollection);
        return services;
    }
}
