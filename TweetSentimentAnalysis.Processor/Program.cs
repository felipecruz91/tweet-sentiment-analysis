using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Tweetinvi.Models;
using TweetSentimentAnalysis.BusinessLogic;
using TweetSentimentAnalysis.DataAcessLayer;

namespace TweetSentimentAnalysis.Processor
{
    internal class Program
    {
        private static IConfigurationRoot _configuration;
        private static readonly HttpClient HttpClient = new HttpClient();

        private static void Main(string[] args)
        {
            Console.WriteLine($"[{DateTime.Now}] - Starting program...");

            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            Console.WriteLine($"[{DateTime.Now}] - Environment: {environmentName}");

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true);

            _configuration = builder.Build();

            try
            {
                var repository = new TweetsRepository();

                // Start the Twitter Stream
                var credentials = GetTwitterCredentials();
                var keys = new TextAnalyticsConfiguration(_configuration);
                var streamFactory =
                    new StreamFactory(new TweetProcessor(repository, keys, HttpClient), credentials, null);
                var keyword = GetKeyword();
                streamFactory.StartStream(keyword);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"[{DateTime.Now}] - Exception: {e.Message}");

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        private static string GetKeyword()
        {
            var keyword = Environment.GetEnvironmentVariable("keyword");
            if (string.IsNullOrEmpty(keyword))
            {
                keyword = _configuration["Twitter:Keyword"];
                if (string.IsNullOrEmpty(keyword))
                {
                    throw new Exception("Tracking word not set. Set the env. variable 'keyword'.");
                }
            }

            return keyword;
        }


        /// <summary>
        ///     Get the Twitter credentials from environment variables.
        ///     Set up your own credentials at https://apps.twitter.com.
        /// </summary>
        /// <returns></returns>
        private static ITwitterCredentials GetTwitterCredentials()
        {
            var consumerKey = _configuration["Twitter:ConsumerKey"];
            var consumerSecret = _configuration["Twitter:ConsumerSecret"];
            var accessToken = _configuration["Twitter:AccessToken"];
            var accessTokenSecret = _configuration["Twitter:AccessTokenSecret"];

            return new TwitterCredentials(consumerKey, consumerSecret, accessToken, accessTokenSecret);
        }
    }
}