using Autofac;
using AutoMechanic.Common.Services;
using AutoMechanic.Common.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AutoMechanic.Common;

public static class DependencyLoader
{
    public static IServiceCollection RegisterCommonDependencies(this IServiceCollection services, ContainerBuilder builder)
    {
        builder.RegisterType<EnvironmentService>().As<IEnvironmentService>().SingleInstance();
        //services.AddSingleton<IEnvironmentService, EnvironmentService>();

        return services;
    }
}
