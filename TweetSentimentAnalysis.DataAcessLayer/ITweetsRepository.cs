using Tweetinvi.Events;
using TweetSentimentAnalysis.Domain.Models.SentimentAnalysis;

namespace TweetSentimentAnalysis.DataAcessLayer
{
    public interface ITweetsRepository
    {
        void SaveTweet(MatchedTweetReceivedEventArgs args, TweetSentiment tweetSentiment);
    }
}