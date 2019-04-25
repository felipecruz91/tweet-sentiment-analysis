using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using TweetSentimentAnalysis.FunctionApp.Models;

namespace TweetSentimentAnalysis.FunctionApp
{
    public static class TweetSentimentTriggerFunction
    {
        /// <summary>
        ///     This function will be called by our client to retrieve the SignalR Service endpoint.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="connectionInfo"></param>
        /// <returns></returns>
        [FunctionName("negotiate")]
        public static SignalRConnectionInfo GetSignalRInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")]
            HttpRequest req,
            [SignalRConnectionInfo(HubName = "tweetsHub")]
            SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }

        [FunctionName("TweetSentimentTrigger")]
        public static Task Run([CosmosDBTrigger(
                "TweetAnalysis",
                "TweetSentiment",
                ConnectionStringSetting = "CosmosDbConnectionString",
                MaxItemsPerInvocation = 5,
                CreateLeaseCollectionIfNotExists = true,
                LeaseCollectionName = "leases")]
            IReadOnlyList<Document> documents
            , ILogger log
            , [SignalR(HubName = "tweetsHub")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            if (documents != null && documents.Count > 0)
            {
                log.LogInformation("Documents modified " + documents.Count);

                foreach (var document in documents)
                {
                    log.LogInformation("Document Id " + document.Id);

                    var fullText = document.GetPropertyValue<string>("fullText");
                    var score = document.GetPropertyValue<double>("score");
                    var keyword = document.GetPropertyValue<string>("keyword");

                    var message = new Message
                    {
                        Sender = nameof(TweetSentimentTriggerFunction),
                        Text = fullText,
                        Score = $"{score}",
                        Keyword = keyword
                    };

                    return signalRMessages.AddAsync(
                        new SignalRMessage
                        {
                            Target = "newMessage",
                            Arguments = new[] {message}
                        });
                }
            }

            return null;
        }
    }
}