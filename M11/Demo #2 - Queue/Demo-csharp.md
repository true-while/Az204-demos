# AZ-204 Demo: Processing Images with WebJobs

In the demo you will send and receive messages from Azure Storage Queue to process images.

![schema](CSharp/schema.png)

## Before delivery:

- Open project `WebJob\ContosoAdsWebJobsSDK.sln` from VS
- Provision Web APP + SQL template
- Deploy project to Azure Web App 
- Create an Azure Storage account
- Update connection string with your values to SQL and Storage Account.
- Update settings in files **App.config** and **Web.config**

## In class:

1. Navigate web App
2. Create new Add and upload huge image.

![Portal](CSharp/portal.png)

1. Demonstrate that thumbnails NOT created on the time you upload.
2. Get connected to the Storage Message to demonstrate the message in the queue.
3. Open Web Job Settings on the portal and demonstrate that Web Job is running. Demonstrate Web Job log.
4. Return back to the portal. thumbnail image should be ready and appears on the page.

![thumbnail](CSharp/thumbnail.png)
