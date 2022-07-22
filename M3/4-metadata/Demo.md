# AZ-204 Demo: Managing blob’s metadata from code

In this demo you will get connected to created previously storage account. 
Upload blobs and update its meta-data.  
You will also search for the blob by using its meta-data.

## Technical Requirements

- Visual Studio Code
- Connection string to the storage account from  #1 demo

## Demonstration:

1. To run the code open the folder from VS Code and use commands  `dotnet build` and `dotnet run`

1. The code will update metadata for blob and container.  

1. The code will search though the tags to find the blob. 

Pay attention for the usage of the following classes:

- TaggedBlobItem
- BlobUploadOptions
- BlobClient

