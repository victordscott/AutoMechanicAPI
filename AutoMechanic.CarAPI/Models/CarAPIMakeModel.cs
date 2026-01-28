using System.Text.Json.Serialization;

namespace AutoMechanic.CarAPI.Models
{
    public class CarAPIMakeModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("make_id")]
        public int MakeId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("make")]
        public CarAPIMake Make { get; set; } = new CarAPIMake();
    }
}
