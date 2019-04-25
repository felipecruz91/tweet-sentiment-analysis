using Microsoft.Extensions.Configuration;
using TweetSentimentAnalysis.BusinessLogic;

namespace TweetSentimentAnalysis.Processor
{
    internal class TextAnalyticsConfiguration : ITextAnalyticsConfiguration
    {
        private readonly IConfigurationRoot _configuration;

        public TextAnalyticsConfiguration(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public string Name => _configuration["TextAnalytics:Name"];
        public string Key1 => _configuration["TextAnalytics:Key1"];
        public string Key2 => _configuration["TextAnalytics:Key2"];
        public string Language => _configuration["TextAnalytics:Language"];
    }
}