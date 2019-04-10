using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace TweetSentimentAnalysis.FunctionApp
{
    public static class TweetSentimentTriggerFunction
    {
        [FunctionName("TweetSentimentTrigger")]
        public static void Run([CosmosDBTrigger(
            databaseName: "TweetAnalysis",
            collectionName: "TweetSentiment",
            ConnectionStringSetting = "CosmosDbConnectionString",
            CreateLeaseCollectionIfNotExists = true,
            LeaseCollectionName = "leases")]IReadOnlyList<Document> documents, TraceWriter log)
        {
            if (documents != null && documents.Count > 0)
            {
                log.Verbose("Documents modified " + documents.Count);
                log.Verbose("First document Id " + documents[0].Id);
            }
        }
    }
}
