using Newtonsoft.Json;

namespace TweetSentimentAnalysis.Domain.Models.SentimentAnalysis
{
    public class ResponseDocument
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("score")] public double Score { get; set; }
    }
}