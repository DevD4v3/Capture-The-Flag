namespace CTF.Application.Players.Combos;

public static class ComboServicesExtensions
{
    public static IServiceCollection AddComboServices(this IServiceCollection services)
    {
        services
            .AddSingleton<ComboSettings>()
            .AddCombo<FlamethrowerVitality>()
            .AddCombo<GrenadesVitality>()
            .AddCombo<MolotovVitality>()
            .AddCombo<RocketLauncherVitality>()
            .AddCombo<SatchelChargesVitality>()
            .AddCombo<TearGasVitality>();

        return services;
    }

    private static IServiceCollection AddCombo<T>(this IServiceCollection services)
        where T : class, ICombo
    {
        services.AddSingleton<ICombo, T>();
        return services;
    }
}
