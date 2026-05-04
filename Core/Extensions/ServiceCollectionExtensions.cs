using GearrOnes.HOA.Template.Core.Interfaces;
using GearrOnes.HOA.Template.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GearrOnes.HOA.Template.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IFeatureFlagService, FeatureFlagService>();
        return services;
    }
}