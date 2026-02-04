using Autofac;
using AutoMechanic.DataAccess.DirectAccess;
using AutoMechanic.DataAccess.EF.Context;
using AutoMechanic.DataAccess.Interfaces;
using AutoMechanic.DataAccess.Repositories;
using AutoMechanic.Services.Services;
using AutoMechanic.Services.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.Services
{
    public static class DependencyLoader
    {
        public static IServiceCollection RegisterServicesDependencies(this IServiceCollection services, ContainerBuilder builder)
        {
            builder.RegisterType<UserService>().As<IUserService>().SingleInstance();
            builder.RegisterType<ConsultantService>().As<IConsultantService>().SingleInstance();
            builder.RegisterType<VehicleService>().As<IVehicleService>().SingleInstance();
            builder.RegisterType<FileUploadService>().As<IFileUploadService>().SingleInstance();

            return services;
        }
    }
}
