using Autofac;
using AutoMechanic.CarAPI.Services;
using AutoMechanic.CarAPI.Services.Interfaces;
using AutoMechanic.Common.Services;
using AutoMechanic.Common.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.CarAPI
{
    public static class DependencyLoader
    {
        public static IServiceCollection RegisterCarAPIDependencies(this IServiceCollection services, ContainerBuilder builder)
        {
            builder.RegisterType<CarAPIService>().As<ICarAPIService>().SingleInstance();

            return services;
        }
    }
}
