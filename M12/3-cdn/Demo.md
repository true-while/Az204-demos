# AZ-204 Demo: Provision and test Azure CDN

In the demo you will create CDN account to cache the images from your storage account. Then you will measure performance of downloading image from storage account and CDN

## Technical requirements:

- Azure CLI or Cloud Shell.

## Demonstration:

1. Open [**demo.azcli**](demo.azcli) in VS code and run commands line by line.
1. Stop after creating the CDN profile to observe the results on the portal.
1. Be aware that caching the files first time after provisioning CDN profile might takes about 5-7 minutes.
1. When you testing download speed you might need to run serval tests from CDN because the first request will wait to downloading from origin.

![screen](screen.png)