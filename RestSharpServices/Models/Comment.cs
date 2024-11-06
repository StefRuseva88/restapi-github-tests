using System.Text.Json.Serialization;

namespace RestSharpServices.Models
{
    public class Comment
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("body")]
        public string ? Body { get; set; }
    }
}
