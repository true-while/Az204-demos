# AZ-204 Demo: C# Azure Functions project

### In the demo you will start Azure Function locally and will reach out the external API with text provided in request. Text will be translated to the Yoddish language.

![Talk like Yoda!](\Nodejs\yodish.png)

## Before delivery:

1. Obtain storage account connection string and update file `local.settings.json`
1. Open the folder in VS Code _ **..\Nodejs\HttpIntegration** _
1. In terminal run command **npm install**
1. Run the project by use play button and make sure it is successfully started. Function console appears without errors. 


## In class:

1. Run the project from folder.
1. By using postman navigate to the local address: **http://localhost:7071/api/TranslateToPiratish**

1. Provide following query in request body: 

```json
{"text": "Master Obiwan has lost a planet!" }
```
1. Response should looks like: 

![Response](\Nodejs\screen2.png)

1. You also can publish and run function from Azure Portal: 

![Response](\Nodejs\screen.png)

> Note the external API is **limited by 5 calls** in hour!