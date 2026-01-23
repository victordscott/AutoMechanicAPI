using Microsoft.Extensions.DependencyInjection;
using AutoMechanic.Configuration.Configuration;
using Autofac;

namespace AutoMechanic.Configuration;

public static class DependencyLoader
{
    public static IServiceCollection RegisterConfigDependencies(this IServiceCollection services, ContainerBuilder builder)
    {
        builder.RegisterType<ConfigManager>().As<IConfigManager>().SingleInstance();
        //services.AddSingleton<IConfigManager, ConfigManager>();
        return services;
    }
}
