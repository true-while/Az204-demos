# AZ-204 Demo: C# Azure Functions project

### In the demo you will run locally functions which is monitoring storage account. When you upload the file in the storage account the function pull this file and split on the small files by airline and upload the files in Cosmos DB

## Before delivery:

1. Provision storage account, function app and cosmos db.
2. Create a target container in storage account. Name: flightdata
3. Cosmos DB collection Docs with DBName Docs should be created

![Container](/CSharp/StorageIntegration/container.png)

1. Install VS 2017/19 or VSCode
2. Install extensions for Azure Function to run locally.
3. Open the project _ **StorageToCosmos.sln** _
4. Update _ **local.settings.json** _ with link to your storage account.
5. Run the application in debug mode and upload the file from _ **DataSample** _ folder to monitoring container in Azure.
6. Check the Cosmos DB collection for new JSON documents.

## In class:

1. Run the project.
2. Upload file from _ **DataSample** _ folder to monitored container in Azure

![Processing file](/CSharp/StorageIntegration/processed.png)

1. Demonstrate the result in Cosmos DB. Documents has TTL – 5min

![Result in cosmos](/CSharp/StorageIntegration/cosmos.png)