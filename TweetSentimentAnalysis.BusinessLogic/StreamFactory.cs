using System;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Streaming;

namespace TweetSentimentAnalysis.BusinessLogic
{
    public class StreamFactory
    {
        private readonly ITweetProcessor _tweetProcessor;
        private readonly IFilteredStream _stream;

        public StreamFactory(ITweetProcessor tweetProcessor, ITwitterCredentials credentials, IFilteredStream stream)
        {
            Auth.SetUserCredentials(credentials.ConsumerKey, credentials.ConsumerSecret,
                credentials.AccessToken, credentials.AccessTokenSecret);
            _tweetProcessor = tweetProcessor;
            _stream = stream ?? Stream.CreateFilteredStream();
        }

        public void StartStream(string keyword)
        {
            Console.WriteLine(
                $"[{DateTime.Now}] - Starting listening for tweets that contains the keyword '{keyword}'...");

            _stream.AddTrack(keyword);
            _stream.MatchingTweetReceived += (sender, args) => { _tweetProcessor.ProcessTweetAsync(keyword, args); };
            _stream.StartStreamMatchingAllConditions();
        }
    }
}