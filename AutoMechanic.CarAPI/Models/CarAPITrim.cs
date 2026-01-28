using System;
using System.Text.Json.Serialization;

namespace AutoMechanic.CarAPI.Models
{
    public class CarAPITrim
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("make_model_id")]
        public int MakeModelId { get; set; }

        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("msrp")]
        public int Msrp { get; set; }

        [JsonPropertyName("invoice")]
        public int Invoice { get; set; }

        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("modified")]
        public DateTime Modified { get; set; }

        [JsonPropertyName("make_model")]
        public CarAPIMakeModel MakeModel { get; set; } = new CarAPIMakeModel();

        [JsonPropertyName("make_model_submodel")]
        public CarAPISubModel MakeModelSubModel { get; set; } = new CarAPISubModel();
    }
}
