using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AutoMechanic.CarAPI.Models
{
    public class CarAPIModelsResponse
    {
        [JsonPropertyName("collection")]
        public CarAPICollection Collection { get; set; } = new CarAPICollection();

        [JsonPropertyName("data")]
        public List<CarAPIModel> Data { get; set; } = new List<CarAPIModel>();
    }
}
