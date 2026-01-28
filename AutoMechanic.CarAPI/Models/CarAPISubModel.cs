using System.Text.Json.Serialization;

namespace AutoMechanic.CarAPI.Models
{
    public class CarAPISubModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("oem_make_model_id")]
        public int OemMakeModelId { get; set; }

        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("make")]
        public string Make { get; set; } = string.Empty;

        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;

        [JsonPropertyName("submodel")]
        public string Submodel { get; set; } = string.Empty;
    }
}
