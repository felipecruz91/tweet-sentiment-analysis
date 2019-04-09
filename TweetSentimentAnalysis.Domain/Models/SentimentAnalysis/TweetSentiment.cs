namespace TweetSentimentAnalysis.Domain.Models.SentimentAnalysis
{
    public class TweetSentiment
    {
        public string FullText { get; set; }
        public double Score { get; set; }
    }
}