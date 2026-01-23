namespace AutoMechanic.Common.Services.Interfaces;

public interface IEnvironmentService
{
    string? GetEnvironmentVariable(string variable);
    bool IsLocalEnvironment();
    public string GetEnvironmentName();
}
