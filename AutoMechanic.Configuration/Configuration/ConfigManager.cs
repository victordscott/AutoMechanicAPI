using Microsoft.Extensions.Configuration;
using AutoMechanic.Configuration.Extensions;

namespace AutoMechanic.Configuration.Configuration
{
    public class ConfigManager : IConfigManager
    {
        public readonly IConfiguration Configuration;
        public ConfigManager(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string Environment => Configuration.GetValue("Environment");
        public bool IsLocalEnv() => Environment.ToLower() is "local";
        public bool IsTestEnv() => Environment.ToLower() is "test";
        public bool IsDevEnv() => Environment.ToLower() is "dev";
        public bool IsUatEnv() => Environment.ToLower() is "uat";
        public bool IsTestOrLocalEnv() => IsTestEnv() || IsLocalEnv();
 
    }
}
