namespace CTF.Application.Players.Chats;

public static class ChatServicesExtensions
{
    public static IServiceCollection AddChatServices(this IServiceCollection services)
    {
        services
            .AddSingleton<IChatMessage, PrivateAdminChat>()
            .AddSingleton<IChatMessage, PrivateModeratorChat>()
            .AddSingleton<IChatMessage, PrivateTeamChat>()
            .AddSingleton<IChatMessage, PrivateVipChat>()
            .AddSingleton<IDictionary<char, IChatMessage>>(sp =>
            {
                var chats = sp.GetRequiredService<IEnumerable<IChatMessage>>();
                return chats.ToDictionary(c => c.Id);
            });

        return services;
    }
}
