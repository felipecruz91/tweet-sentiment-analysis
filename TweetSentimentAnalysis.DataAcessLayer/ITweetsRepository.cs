using System.Threading.Tasks;
using Tweetinvi.Events;
using TweetSentimentAnalysis.Domain.Models.SentimentAnalysis;

namespace TweetSentimentAnalysis.DataAcessLayer
{
    public interface ITweetsRepository
    {
        Task SaveTweetAsync(MatchedTweetReceivedEventArgs args, TweetSentiment tweetSentiment);
    }
}