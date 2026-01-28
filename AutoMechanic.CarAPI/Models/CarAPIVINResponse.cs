using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AutoMechanic.CarAPI.Models
{
    public class CarAPIVINResponse
    {
        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("make")]
        public string Make { get; set; } = string.Empty;

        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;

        [JsonPropertyName("trim")]
        public string Trim { get; set; } = string.Empty;

        [JsonPropertyName("specs")]
        public CarAPIVINSpecs Specs { get; set; } = new CarAPIVINSpecs();

        [JsonPropertyName("trims")]
        public List<CarAPITrim> Trims { get; set; } = new List<CarAPITrim>();
    }
}
