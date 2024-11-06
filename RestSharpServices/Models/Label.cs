using System.Text.Json.Serialization;

namespace RestSharpServices.Models
{
    public class Label
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}
