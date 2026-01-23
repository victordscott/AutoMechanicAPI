namespace AutoMechanic.Configuration.Configuration
{
    public interface IConfigManager
    {
        string Environment { get; }
        bool IsDevEnv();
        bool IsLocalEnv();
        bool IsTestEnv();
        bool IsTestOrLocalEnv();
        bool IsUatEnv();
    }
}