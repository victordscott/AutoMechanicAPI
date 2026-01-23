using Autofac;
using AutoMechanic.Auth.Services;
using AutoMechanic.Auth.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.Auth
{
    public static class DependencyLoader
    {
        public static IServiceCollection RegisterAuthDependencies(this IServiceCollection services, ContainerBuilder builder)
        {
            builder.RegisterType<TokenService>().As<ITokenService>().SingleInstance();
            builder.RegisterType<AuthService>().As<IAuthService>().SingleInstance();

            return services;
        }
    }
}
