using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
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
                PartitionKey = args.Tweet.CreatedBy.UserIdentifier.ScreenName,
                FullText = args.Tweet.FullText,
                CreatedAt = args.Tweet.CreatedAt,
                Score = tweetSentiment.Score
            };

            try
            {
                await _documentClient.CreateDocumentAsync(_documentCollectionUri, document);
            }
            catch (DocumentClientException documentClientException)
            {
                if (documentClientException.StatusCode != null)
                {
                    var statusCode = (int) documentClientException.StatusCode;

                    // Error 429 ("Request rate is large") indicates that the application has exceeded the provisioned RU quota, and should retry the request after a small time interval.
                    if (statusCode == 429 || statusCode == (int) HttpStatusCode.ServiceUnavailable)
                    {
                        var retryAfterMilliseconds = documentClientException.RetryAfter.Milliseconds;
                        Console.WriteLine(
                            $"Status Code: {statusCode}. Retry after: {retryAfterMilliseconds} ms.");

                        Thread.Sleep(retryAfterMilliseconds);
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }
        }
    }
}