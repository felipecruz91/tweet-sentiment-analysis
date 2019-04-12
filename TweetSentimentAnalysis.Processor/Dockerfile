FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY TweetSentimentAnalysis.Processor/TweetSentimentAnalysis.Processor.csproj TweetSentimentAnalysis.Processor/
COPY TweetSentimentAnalysis.BusinessLogic/TweetSentimentAnalysis.BusinessLogic.csproj TweetSentimentAnalysis.BusinessLogic/
COPY TweetSentimentAnalysis.Domain/TweetSentimentAnalysis.Domain.csproj TweetSentimentAnalysis.Domain/
COPY TweetSentimentAnalysis.DataAcessLayer/TweetSentimentAnalysis.DataAcessLayer.csproj TweetSentimentAnalysis.DataAcessLayer/
RUN dotnet restore TweetSentimentAnalysis.Processor/TweetSentimentAnalysis.Processor.csproj
COPY . .
WORKDIR /src/TweetSentimentAnalysis.Processor
RUN dotnet build TweetSentimentAnalysis.Processor.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish TweetSentimentAnalysis.Processor.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TweetSentimentAnalysis.Processor.dll"]
