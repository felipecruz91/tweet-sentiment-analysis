using System;
using Newtonsoft.Json;

namespace TweetSentimentAnalysis.DataAcessLayer
{
    public class TweetSentimentDocument
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }


        [JsonProperty(PropertyName = "partitionKey")]
        public string PartitionKey { get; set; }

        [JsonProperty(PropertyName = "fullText")]
        public string FullText { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty(PropertyName = "score")] public double Score { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}