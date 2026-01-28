using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AutoMechanic.CarAPI.Models
{
    public class CarAPISubModelsResponse
    {
        [JsonPropertyName("collection")]
        public CarAPICollection Collection { get; set; } = new CarAPICollection();

        [JsonPropertyName("data")]
        public List<CarAPISubModel> Data { get; set; } = new List<CarAPISubModel>();
    }
}
