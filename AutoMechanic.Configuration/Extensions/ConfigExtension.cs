using Microsoft.Extensions.Configuration;

namespace AutoMechanic.Configuration.Extensions
{
    public static class ConfigExtension
    {
        public static string GetValue(this IConfiguration configuration, string key)
        {
            var secret = configuration[key];
            return secret;
        }

        public static string? TryGetValue(this IConfiguration configuration, string key)
        {
            return string.IsNullOrEmpty(key) ? null : configuration[key];
        }

        public static bool SecretExists(this IConfiguration configuration, string key)
        {
            var value = configuration[key];
            return !string.IsNullOrEmpty(value);
        }
    }
}
