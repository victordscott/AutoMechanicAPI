
using Autofac;
using AutoMechanic.Configuration.Configuration;
using AutoMechanic.Configuration.Options;
using AutoMechanic.DataAccess.DirectAccess;
using AutoMechanic.DataAccess.EF.Context;
using AutoMechanic.DataAccess.Interfaces;
using AutoMechanic.DataAccess.Models;
using AutoMechanic.DataAccess.Repositories;
using AutoMechanic.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace AutoMechanic.DataAccess;

public static class DependencyLoader
{
    public static IServiceCollection RegisterIdentityDependencies(this IServiceCollection services)
    {
        services.AddDbContextFactory<AutoMechanicIdentityContext>();
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<AutoMechanicIdentityContext>()
            .AddDefaultTokenProviders();
        services.AddDbContextFactory<AutoMechanicDbContext>();

        return services;
    }

    public static IServiceCollection RegisterDataAccessDependencies(this IServiceCollection services, ContainerBuilder builder)
    {
        //builder.RegisterType<ProcCallerFactory>().As<IProcCallerFactory>().SingleInstance();
        //builder.Register(context =>
        //{
        //    var factory = context.Resolve<IProcCallerFactory>();
        //    return factory.Create();
        //}).SingleInstance();

        
        builder.RegisterType<ProcCaller>().As<IProcCaller>().SingleInstance();
        
        //builder.RegisterType<AutoMechanicDbContext>()
        //    .AsSelf()
        //    .InstancePerLifetimeScope();
        builder.RegisterType<UserRepository>().As<IUserRepository>().SingleInstance();
        builder.RegisterType<ConsultantRepository>().As<IConsultantRepository>().SingleInstance();
        builder.RegisterType<VehicleRepository>().As<IVehicleRepository>().SingleInstance();
        builder.RegisterType<FileUploadRepository>().As<IFileUploadRepository>().SingleInstance();

        return services;
    }
}
