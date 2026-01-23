using Microsoft.Extensions.Configuration;

namespace AutoMechanic.Common.Configuration
{
    public class AmazonApiGatewayConfiguration
    {
        private readonly IConfiguration configuration;
        public AmazonApiGatewayConfiguration(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string Region => configuration["AWS:Region"];
        public string BaseAddress => configuration["AWS:ApiGateway:BaseAddress"];
    }
}
