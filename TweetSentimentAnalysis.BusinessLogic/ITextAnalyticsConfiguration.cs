namespace TweetSentimentAnalysis.BusinessLogic
{
    public interface ITextAnalyticsConfiguration
    {
        string Name { get; }
        string Key1 { get; }
        string Key2 { get; }
    }
}