using System.Text.Json.Serialization;

namespace AutoMechanic.CarAPI.Models
{
    public class CarAPIModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("make_id")]
        public int MakeId { get; set; }

        [JsonPropertyName("make")]
        public string Make { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}
