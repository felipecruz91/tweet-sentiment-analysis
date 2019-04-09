using Newtonsoft.Json;

namespace TweetSentimentAnalysis.Domain.Models.SentimentAnalysis
{
    public class RequestDocument
    {
        [JsonProperty("language")] public string Language { get; set; }

        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("text")] public string Text { get; set; }
    }
}