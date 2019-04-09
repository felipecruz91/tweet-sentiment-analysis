# Tweet Sentiment Analysis 💬➡🧠➡[😊|😐|😥]

## Summary

 *Real-time* tweet sentiment analysis (positive/neutral/negative) and visualization in a .NET Core Web App.
 
 The tweets that are retrieved in *real-time* from the Twitter Stream will be processed and analyzed by the Azure Sentiment Analysis API and persisted in Cosmos DB. 
 Finally, both the tweet text and sentiment will be displayed in the Web App as they come through.

## Architecture diagram

![architecture-diagram](./docs/images/architecture-diagram.PNG)

## Requirements

You will need the following tools to complete the hands-on lab.

- Visual Studio 2017
- Docker
- Azure account

## Steps

1. Create a resource group with name `rg-azure-bootcamp`

    ![architecture-diagram](./docs/images/create-resource-group-step.PNG)

2. Create an Azure Cosmos DB Account under the `rg-azure-bootcamp` resource group that you have created in the previous step.

    ![architecture-diagram](./docs/images/create-azure-cosmosdb-account.PNG)

        Resource Group  rg-azure-bootcamp
        Location        West Europe
        Account Name    (new) felipecruz-cosmosdb
        API             Core (SQL)
        Geo-Redundancy  Disable
        Multi-region Writes Disable    