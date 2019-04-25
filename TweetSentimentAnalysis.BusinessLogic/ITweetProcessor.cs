using System.Threading.Tasks;
using Tweetinvi.Events;

namespace TweetSentimentAnalysis.BusinessLogic
{
    public interface ITweetProcessor
    {
        Task ProcessTweetAsync(string keyword, MatchedTweetReceivedEventArgs args);
    }
}