using Autofac;
using AutoMechanic.API.Hangfire;
using AutoMechanic.CarAPI.Services;
using AutoMechanic.CarAPI.Services.Interfaces;

namespace AutoMechanic.API
{
    public static class DependencyLoader
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services, ContainerBuilder builder)
        {
            builder.RegisterType<HangfireTestJob>().As<IHangfireTestJob>().SingleInstance();

            return services;
        }
    }
}
