    
docker build -t twitterfeedprocessor:%2 -t twitterfeedprocessor:latest -f TweetSentimentAnalysis.Processor/Dockerfile .

docker tag twitterfeedprocessor:%2 %1/twitterfeedprocessor:%2
docker tag twitterfeedprocessor:%2 %1/twitterfeedprocessor:latest

docker push %1/twitterfeedprocessor:%2
docker push %1/twitterfeedprocessor:latest