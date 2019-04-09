using System.Collections.Generic;
using Tweetinvi.Events;
using TweetSentimentAnalysis.Domain.Models.SentimentAnalysis;

namespace TweetSentimentAnalysis.DataAcessLayer
{
    public class TweetsRepository : ITweetsRepository
    {
        public void SaveTweet(MatchedTweetReceivedEventArgs args, TweetSentiment tweetSentiment)
        {
            var fields = new Dictionary<string, object>
            {
                {"fullText", args.Tweet.FullText},
                {"screen_name", args.Tweet.CreatedBy.UserIdentifier.ScreenName},
                {"isRetweet", args.Tweet.IsRetweet},
                {"retweetCount", args.Tweet.RetweetCount},
                {"favorited", args.Tweet.Favorited},
                {"favoriteCount", args.Tweet.FavoriteCount},
                {"created_at", args.Tweet.CreatedAt},
                {"score", tweetSentiment.Score}
            };

            //TODO: Save document in Cosmos DB
        }
    }
}