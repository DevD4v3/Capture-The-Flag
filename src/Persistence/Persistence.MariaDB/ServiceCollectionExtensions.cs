namespace Persistence.MariaDB;

public static class PersistenceMariaDBServicesExtensions
{
    public static IServiceCollection AddPersistenceMariaDBServices(
        this IServiceCollection services, 
        IConfiguration configuration,
        string sqlPath)
    {
        var mariadbSettings = configuration
            .GetRequiredSection("MariaDB")
            .Get<MariaDbSettings>();

        var connectionString = new MySqlConnectionStringBuilder()
        {
            Server   = mariadbSettings.Server,
            Port     = mariadbSettings.Port,
            Database = mariadbSettings.Database,
            UserID   = mariadbSettings.UserName,
            Password = mariadbSettings.Password
        }.ToString();

        mariadbSettings.ConnectionString = connectionString;
        services.AddSingleton(mariadbSettings)
                .AddSingleton<IPlayerRepository, PlayerRepository>()
                .AddSingleton<ITopPlayersRepository, TopPlayersRepository>();

        var path = Path.Combine(sqlPath, typeof(PersistenceMariaDBServicesExtensions).Namespace);
        ISqlCollection sqlCollection = new YeSqlLoader().LoadFromDirectories(path);
        services.AddSingleton(sqlCollection);
        return services;
    }
}
