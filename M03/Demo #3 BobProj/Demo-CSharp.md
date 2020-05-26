# AZ-204 Demo: Managing Azure Blob from code

In this demo you demonstrate code sniped to upload, list, update, download and delete images in Azure Storage Account. 
The code was taken from real project collecting and processing dumps of customer application. 

## Before delivery:

- VSCode must be installed

## In class:

1. Open file `BlobRepo.cs`.
2. Observe how used and configured `_blobaccount`. OIt is build form connection string. 
3. Observe function **UploadFile** which is used for uploading blobs. Notice how we use `blob.Metadata` for search and `blob.StreamWriteSizeInBytes` for uploads.
4. Observe function **DownLoadFile** to downloading files.
5. Observe function for getting the blob list **GetBlobList** and etc.