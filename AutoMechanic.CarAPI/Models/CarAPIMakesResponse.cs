using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AutoMechanic.CarAPI.Models
{
    public class CarAPIMakesResponse
    {
        [JsonPropertyName("collection")]
        public CarAPICollection Collection { get; set; } = new CarAPICollection();

        [JsonPropertyName("data")]
        public List<CarAPIMake> Data { get; set; } = new List<CarAPIMake>();
    }
}
