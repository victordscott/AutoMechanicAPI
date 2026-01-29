using AutoMechanic.Configuration.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutoMechanic.Configuration;

public static class InternalConfiguration
{
    public static void LoadOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<DatabaseOptions>()
            .Bind(configuration.GetSection(DatabaseOptions.Database))
            .ValidateDataAnnotations();

        services.AddOptions<JWTOptions>()
            .Bind(configuration.GetSection(JWTOptions.JWT))
            .ValidateDataAnnotations();

        services.AddOptions<MiscOptions>()
            .Bind(configuration.GetSection(MiscOptions.Misc))
            .ValidateDataAnnotations();

        services.AddOptions<CarAPIOptions>()
            .Bind(configuration.GetSection(CarAPIOptions.CarAPI))
            .ValidateDataAnnotations();
    }
}
