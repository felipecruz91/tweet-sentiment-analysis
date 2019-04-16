using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Tweetinvi.Events;
using TweetSentimentAnalysis.DataAcessLayer;
using TweetSentimentAnalysis.Domain.Models.SentimentAnalysis;

namespace TweetSentimentAnalysis.BusinessLogic
{
    public class TweetProcessor : ITweetProcessor
    {
        private readonly HttpClient _httpClient;
        private readonly ITweetsRepository _tweetsRepository;

        public TweetProcessor(ITweetsRepository tweetsRepository, ITextAnalyticsConfiguration configuration,
            HttpClient httpClient)
        {
            _tweetsRepository = tweetsRepository;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{configuration.Key1}");
        }

        public async Task ProcessTweetAsync(string track, MatchedTweetReceivedEventArgs args)
        {
            var responseDocument = await MakeRequest(args.Tweet.FullText);

            var tweetSentiment = new TweetSentiment
            {
                FullText = args.Tweet.FullText,
                Score = responseDocument.Score
            };

            Console.WriteLine($"[{DateTime.Now}] - Tweet: {tweetSentiment.FullText}. Sentiment: {tweetSentiment.Score}");

            await _tweetsRepository.SaveTweetAsync(args, tweetSentiment);
        }


        private async Task<ResponseDocument> MakeRequest(string tweetFullText)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            var uri = "https://westeurope.api.cognitive.microsoft.com/text/analytics/v2.0/sentiment?" + queryString;

            HttpResponseMessage response;

            // Request body
            var requestBody = new RequestRootObject
            {
                Documents = new List<RequestDocument>
                {
                    new RequestDocument
                    {
                        Language = "en",
                        Id = "1",
                        Text = tweetFullText
                    }
                }
            };

            var serializedRequestBody = JsonConvert.SerializeObject(requestBody);
            var byteData = Encoding.UTF8.GetBytes(serializedRequestBody);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await _httpClient.PostAsync(uri, content);
            }

            return await GetResponseDocument(response);
        }

        private static async Task<ResponseDocument> GetResponseDocument(HttpResponseMessage response)
        {
            // Asynchronously get the JSON response.
            var contentString = await response.Content.ReadAsStringAsync();

            // Deserialize the content string.
            var deserializedContent = JsonConvert.DeserializeObject<ResponseRootObject>(contentString);

            var errors = GetErrorsFromResponse(response, deserializedContent);
           
            if (errors.Any())
            {
                errors.ForEach(error => Console.Error.WriteLine($"Error - Id: {error.Id}. Message: {error.Message}"));
            }

            return deserializedContent.Documents.FirstOrDefault();
        }

        private static List<Error> GetErrorsFromResponse(HttpResponseMessage response, ResponseRootObject deserializedContent)
        {
            var errors = new List<Error>();

            if (!response.IsSuccessStatusCode)
            {
                if (deserializedContent.Error != null)
                {
                    errors.Add(deserializedContent.Error);
                }
                if (deserializedContent.Errors != null)
                {
                    errors.AddRange(deserializedContent.Errors);
                }
            }
            return errors;
        }
    }
}