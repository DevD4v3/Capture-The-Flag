namespace CTF.Application.Teams.Flags;

public static class FlagServicesExtensions
{
    public static IServiceCollection AddFlagServices(this IServiceCollection services)
    {
        services
            .AddFlagEvent<OnFlagAtBasePosition>()
            .AddFlagEvent<OnFlagCaptured>()
            .AddFlagEvent<OnFlagReturned>()
            .AddFlagEvent<OnFlagDropped>()
            .AddFlagEvent<OnFlagScore>()
            .AddFlagEvent<OnFlagTaken>()
            .AddSingleton(sp =>
            {
                var flagEvents = sp.GetRequiredService<IEnumerable<IFlagEvent>>();
                return flagEvents.ToFrozenDictionary(f => f.FlagStatus);
            });

        services
            .AddSingleton<FlagAutoReturnTimer>()
            .AddSingleton<FlagStateResetter>();

        return services;
    }

    private static IServiceCollection AddFlagEvent<T>(this IServiceCollection services)
        where T : class, IFlagEvent
    {
        services.AddSingleton<IFlagEvent, T>();
        return services;
    }
}
