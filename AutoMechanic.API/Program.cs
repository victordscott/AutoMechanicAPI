using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using AutoMechanic.Auth;
using AutoMechanic.CarAPI;
using AutoMechanic.Common;
using AutoMechanic.Configuration;
using AutoMechanic.DataAccess;
using AutoMechanic.DataAccess.EF.Context;
using AutoMechanic.DataAccess.Models;
using AutoMechanic.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
//using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
if (isDev)
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.RegisterIdentityDependencies();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.LoadOptions(builder.Configuration);

var connectionString = builder.Configuration.GetValue<string>("Database:ConnectionString");

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
    configuration
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.PostgreSQL(connectionString, "logs", needAutoCreateTable: true);
});

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestBodyLogLimit = 16384;    // 16 KB, default is 32
    logging.ResponseBodyLogLimit = 16384;
    logging.MediaTypeOptions.AddText("application/json");
    logging.CombineLogs = true;
});

// without this, this error can occur: 
// "System.Text.Json.JsonException: A possible object cycle was detected. This can either be due to a cycle or if the object depth is larger
// than the maximum allowed depth of 32. Consider using ReferenceHandler.Preserve on JsonSerializerOptions to support cycles"
// The other solution is to select specific fields so the cycle / loop doesn't occur
// https://stackoverflow.com/questions/70603118/how-to-resolve-system-text-json-jsonexception-a-possible-object-cycle-was-detec
// https://stackoverflow.com/questions/70603118/how-to-resolve-system-text-json-jsonexception-a-possible-object-cycle-was-detec#comment124964436_70603806
// https://learn.microsoft.com/en-us/ef/core/querying/related-data/serialization
builder.Services.AddMvc()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
    );

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    builder.Services
        .RegisterConfigDependencies(containerBuilder)
        .RegisterCommonDependencies(containerBuilder)
        .RegisterDataAccessDependencies(containerBuilder)
        .RegisterServicesDependencies(containerBuilder)
        .RegisterAuthDependencies(containerBuilder)
        .RegisterCarAPIDependencies(containerBuilder);
});

Action<IMapperConfigurationExpression> configAction = null;
builder.Services.AddAutoMapper(configAction, AppDomain.CurrentDomain.GetAssemblies());

var validIssuer = builder.Configuration.GetValue<string>("JWT:ValidIssuer");
var validAudience = builder.Configuration.GetValue<string>("JWT:ValidAudience");
var secretKey = builder.Configuration.GetValue<string>("JWT:Secret");

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;    // didn't fix sub claim issue
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = validIssuer,
            ValidAudience = validAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddHttpClient();
builder.Services
    .AddControllers(options => {
        // https://stackoverflow.com/a/72981145/2030207
        // prevents web api validation of model objects
        // One or more validation errors occurred... The ... field is required.
        options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true; }
    ).AddJsonOptions(options =>
    {
        // prevents lowercase first letter
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Ensure database is migrated
//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<AutoMechanicDbContext>();
//    dbContext.Database.Migrate();
//}

// Seed roles
//using (var scope = app.Services.CreateScope())
//{
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//    var roles = new[] { "Administrator", "Consultant", "Customer" };

//    foreach (var role in roles)
//    {
//        if (!await roleManager.RoleExistsAsync(role))
//        {
//            await roleManager.CreateAsync(new IdentityRole(role));
//        }
//    }
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(cors => cors
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials()
);

app.UseHttpsRedirection();

// sub claim issue fix
// https://stackoverflow.com/a/61900842/2030207
//JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});

app.Run();
