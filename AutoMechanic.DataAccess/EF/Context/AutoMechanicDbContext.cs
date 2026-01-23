using AutoMechanic.Configuration.Configuration;
using AutoMechanic.Configuration.Options;
using AutoMechanic.DataAccess.EF.Models;
using AutoMechanic.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AutoMechanic.DataAccess.EF.Context
{
    public class AutoMechanicDbContext: AutoMechanicDbContextGenerated
    {
        public static readonly Microsoft.Extensions.Logging.LoggerFactory loggerFactory =
            new LoggerFactory(new[] {
                new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()
            });

        private IOptions<DatabaseOptions> databaseOptions;

        public AutoMechanicDbContext(IOptions<DatabaseOptions> databaseOptions) : base(new DbContextOptions<AutoMechanicDbContextGenerated>())
        {
            this.databaseOptions = databaseOptions;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(databaseOptions.Value.ConnectionString);

            // TODO: move this to a central location
            var isLocal = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Local";
            if (isLocal)
            {
                //optionsBuilder.UseLoggerFactory(loggerFactory);
                //optionsBuilder.EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // time zone experiment
            //modelBuilder.Entity<UserLogin>()
            //    .Property(e => e.LoginDate)
            //    .HasColumnType("timestamp with time zone");
        }
    }
}
