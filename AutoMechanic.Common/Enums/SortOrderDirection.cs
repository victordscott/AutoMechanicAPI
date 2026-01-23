using System.Text.Json.Serialization;

namespace AutoMechanic.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SortOrderDirection
    {
        Ascending = 0,
        Descending = 1
    }
}
