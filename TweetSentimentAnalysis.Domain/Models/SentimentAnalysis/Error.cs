using Newtonsoft.Json;

namespace TweetSentimentAnalysis.Domain.Models.SentimentAnalysis
{
    public class Error
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("code")] public string Code { get; set; }
        [JsonProperty("message")] public string Message { get; set; }
    }
}