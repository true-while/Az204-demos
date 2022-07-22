# AZ-204 Demo: Explore Azure Function project

In the demo you will start Azure Function locally and will reach out the external API with text provided in request. Text will be translated to the Piratish language.


## Requirements:

1. VS 2019/22 with Azure SDK support.
1. [Postman](https://www.postman.com/downloads/) to test Web API

## Demonstration:

1. Open and start execution of project _ **TranslateToPirate.sln** _ 
 
1. By using postman navigate to the local address: **http://localhost:7071/api/TranslateToPiratish**

1. Provide following query in request body: 

```json
{"text":"My mother goes with me to the ocean!"}
```
4. Response should looks like: 

![Processing file](CSharp/screen.png)

> Note the external API is **limited by 5 calls** in hour!