using System.Text.Json.Serialization;

namespace AutoMechanic.CarAPI.Models
{
    public class CarAPICollection
    {
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("pages")]
        public int Pages { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("next")]
        public string Next { get; set; } = string.Empty;

        [JsonPropertyName("prev")]
        public string Prev { get; set; } = string.Empty;

        [JsonPropertyName("first")]
        public string First { get; set; } = string.Empty;

        [JsonPropertyName("last")]
        public string Last { get; set; } = string.Empty;
    }
}
