using System;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Tweetinvi.Events;
using TweetSentimentAnalysis.Domain.Models.SentimentAnalysis;

namespace TweetSentimentAnalysis.DataAcessLayer
{
    public class TweetsRepository : ITweetsRepository
    {
        private const string DatabaseId = "TweetAnalysis";
        private const string CollectionId = "TweetSentiment";
        private readonly DocumentClient _documentClient;
        private readonly Uri _documentCollectionUri;

        public TweetsRepository(DocumentClient documentClient)
        {
            _documentClient = documentClient;
            _documentCollectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId);
        }

        public async Task SaveTweetAsync(MatchedTweetReceivedEventArgs args, TweetSentiment tweetSentiment)
        {
            var document = new TweetSentimentDocument
            {
                Id = args.Tweet.FullText.GetHashCode().ToString(), //TODO
                PartitionKey = args.Tweet.CreatedBy.UserIdentifier.ScreenName,
                FullText = args.Tweet.FullText,
                CreatedAt = args.Tweet.CreatedAt,
                Score = tweetSentiment.Score
            };

            try
            {
                var x = await _documentClient.CreateDocumentAsync(_documentCollectionUri,
                    document);

                var response = x;
            }
            catch (Exception e)
            {
                //TODO: Handle possible HTTP Status Code 429 Too Many Requests and Timeouts!
                Console.WriteLine(e);
                Console.ReadKey();
            }
        }
    }
}