namespace CTF.Application.Players.Chats;

public static class ChatServicesExtensions
{
    public static IServiceCollection AddChatServices(this IServiceCollection services)
    {
        services
            .AddChatMessage<PrivateAdminChat>()
            .AddChatMessage<PrivateModeratorChat>()
            .AddChatMessage<PrivateTeamChat>()
            .AddChatMessage<PrivateVipChat>()
            .AddSingleton(sp =>
            {
                var chats = sp.GetRequiredService<IEnumerable<IChatMessage>>();
                return chats.ToFrozenDictionary(c => c.Id);
            });

        return services;
    }

    private static IServiceCollection AddChatMessage<T>(this IServiceCollection services)
        where T : class, IChatMessage
    {
        services.AddSingleton<IChatMessage, T>();
        return services;
    }
}
