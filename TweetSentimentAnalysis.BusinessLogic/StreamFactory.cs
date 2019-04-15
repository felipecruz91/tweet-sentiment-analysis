using System;
using System.Net;
using System.Threading;
using Tweetinvi;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Tweetinvi.Streaming;

namespace TweetSentimentAnalysis.BusinessLogic
{
    public class StreamFactory
    {
        private readonly IFilteredStream _stream;
        private readonly ITweetProcessor _tweetProcessor;
        private readonly int? _tweetsPerMinute;
        private int _lastProcessedTime;

        public StreamFactory(ITweetProcessor tweetProcessor, ITwitterCredentials credentials, IFilteredStream stream,
            int? tweetsPerMinute)
        {
            var result = Auth.SetUserCredentials(credentials.ConsumerKey, credentials.ConsumerSecret,
                credentials.AccessToken, credentials.AccessTokenSecret);
            _tweetProcessor = tweetProcessor;
            _tweetsPerMinute = tweetsPerMinute;
            _stream = stream ?? Stream.CreateFilteredStream();
        }

        public void StartStream(string keyword)
        {
            Console.WriteLine(
                $"[{DateTime.Now}] - Starting listening for tweets that contains the keyword '{keyword}'...");
            try
            {
                // Disable the exception swallowing to allow exception to be thrown by Tweetinvi
                ExceptionHandler.SwallowWebExceptions = false;
                var authenticatedUser = User.GetAuthenticatedUser(null, new GetAuthenticatedUserParameters());

                if (authenticatedUser != null) // Something went wrong but we don't know what
                {
                    _stream.AddTrack(keyword);
                    _stream.MatchingTweetReceived += (sender, args) =>
                    {
                    // Exclude RTs
                    if (args.Tweet.IsRetweet)
                        {
                            return;
                        }

                        if (_tweetsPerMinute.HasValue && _tweetsPerMinute.Value != default(int))
                        {
                            Throttle();
                        }

                        _tweetProcessor.ProcessTweetAsync(keyword, args);

                        _lastProcessedTime = Environment.TickCount;
                        Console.WriteLine($"[{DateTime.Now}] - Last processed time {_lastProcessedTime} ms.");
                    };
                    _stream.StartStreamMatchingAllConditions();
                }
            }
            catch (TwitterException ex)
            {
                Console.WriteLine($"TwitterException Message : {ex.Message} - description : {ex.TwitterDescription}");
                throw ex;
            }
        }

        private void Throttle()
        {
            var elapsedTime = Environment.TickCount - _lastProcessedTime;
            var pause = 60 / _tweetsPerMinute.Value * 1000;
            var wait = pause - elapsedTime;
            if (wait > 0)
            {
                Console.WriteLine($"[{DateTime.Now}] - Waiting {wait} ms to receive next tweet.");
                Thread.Sleep(wait);
            }
        }
    }
}