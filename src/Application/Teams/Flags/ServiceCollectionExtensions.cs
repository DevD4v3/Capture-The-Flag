namespace CTF.Application.Teams.Flags;

public static class FlagServicesExtensions
{
    public static IServiceCollection AddFlagServices(this IServiceCollection services)
    {
        services
            .AddSingleton<IFlagEvent, OnFlagAtBasePosition>()
            .AddSingleton<IFlagEvent, OnFlagCaptured>()
            .AddSingleton<IFlagEvent, OnFlagReturned>()
            .AddSingleton<IFlagEvent, OnFlagDropped>()
            .AddSingleton<IFlagEvent, OnFlagScore>()
            .AddSingleton<IFlagEvent, OnFlagTaken>()
            .AddSingleton<IDictionary<FlagStatus, IFlagEvent>>(sp =>
            {
                var flagEvents = sp.GetRequiredService<IEnumerable<IFlagEvent>>();
                return flagEvents.ToDictionary(f => f.FlagStatus);
            });

        services
            .AddSingleton<FlagAutoReturnTimer>()
            .AddSingleton<FlagStateResetter>();

        return services;
    }
}
