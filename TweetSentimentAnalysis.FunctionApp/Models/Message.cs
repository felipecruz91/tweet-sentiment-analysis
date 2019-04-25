using Newtonsoft.Json;

namespace TweetSentimentAnalysis.FunctionApp.Models
{
    public class Message
    {
        [JsonProperty("sender")] public string Sender { get; set; }
        [JsonProperty("text")] public string Text { get; set; }
        [JsonProperty("score")] public string Score { get; set; }
        [JsonProperty("keyword")] public string Keyword { get; set; }
    }
}