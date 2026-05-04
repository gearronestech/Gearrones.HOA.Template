using GearrOnes.HOA.Template.Config.Interfaces;
using GearrOnes.HOA.Template.Config.Models;
using GearrOnes.HOA.Template.Config.Services;
using GearrOnes.HOA.Template.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GearrOnes.HOA.Template.Config.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPlanConfiguration(this IServiceCollection services, IConfiguration config)
    {
        var planPath = config["PlanConfig:Path"];
        if (string.IsNullOrWhiteSpace(planPath))
        {
            throw new InvalidOperationException("Missing configuration value for 'PlanConfig:Path'.");
        }

        var loader = new JsonPlanConfigurationLoader();
        var planConfiguration = loader.Load(planPath);

        services.AddSingleton(planConfiguration);
        services.AddSingleton<IPlanContext, PlanContext>();
        services.AddSingleton<FeatureFlags>(sp =>
            sp.GetRequiredService<PlanConfiguration>().FeatureFlags
            ?? throw new InvalidOperationException("Plan configuration must include FeatureFlags."));

        return services;
    }

    public static IServiceCollection AddHoaClientContext(this IServiceCollection services, IConfiguration config)
    {
        var section = config.GetSection("HoaClient");
        var options = section.Get<HoaClientOptions>() ?? new HoaClientOptions();

        ValidateRequired(options.HoaName, "HoaClient:HoaName");
        ValidateRequired(options.PortalTitle, "HoaClient:PortalTitle");
        ValidateRequired(options.ContactEmail, "HoaClient:ContactEmail");

        services.Configure<HoaClientOptions>(section);
        services.AddSingleton<IHoaClientContext, HoaClientContext>();

        return services;
    }

    private static void ValidateRequired(string? value, string key)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"Missing required configuration value for '{key}'.");
        }
    }
}