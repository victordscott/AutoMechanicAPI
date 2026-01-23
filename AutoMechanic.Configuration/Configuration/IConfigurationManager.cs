namespace AutoMechanic.Configuration.Configuration
{
    public interface IConfigurationManager
    {
        string GetSecret(string secretName);
    }
}
