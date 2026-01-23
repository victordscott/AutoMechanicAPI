using AutoMechanic.Common.Exceptions;
using AutoMechanic.Common.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace AutoMechanic.Common.Services;

public class EnvironmentService(ILogger<EnvironmentService> logger) : IEnvironmentService
{
    public string? GetEnvironmentVariable(string variable)
    {
        return Environment.GetEnvironmentVariable(variable);
    }

    public bool IsLocalEnvironment()
    {
        return GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Local";
    }

    public string GetEnvironmentName()
    {
        var name = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (string.IsNullOrEmpty(name))
        {
            var errorMessage = $"Could not get environment name. Got: {name}";
            logger.LogError(errorMessage);
            throw new InternalLogicException(errorMessage);
        }

        return name switch
        {
            "Development" => "dev",
            "UAT" => "uat",
            "PROD" => "prod",
            _ => name.ToLower()
        };
    }
}
