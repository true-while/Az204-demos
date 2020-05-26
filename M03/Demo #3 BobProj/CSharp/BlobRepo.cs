using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;


    public class BlobRepository
    {
        private static BlobRepository singletoneInstance;

        public static BlobRepository GetInstance
        {
            get
            {
                if (singletoneInstance == null)
                    singletoneInstance = new BlobRepository(ConfigurationManager.AppSettings["CloudBlob"]);
           
                return singletoneInstance;
            }
        }

        private CloudBlobClient _cloudBlobClient;
        public string DumpContainerName = "UnAnalizedDumps";
        public string LogContainerName = "UnAnalizedLogs";
        private CloudStorageAccount _blobaccount;
        private BlobRequestOptions _bro;
        public string NewContainerDumps = "NewDumps";
        public string NewContainerLogs = "NewLogs";

        /// <summary>
        /// Initialize controller
        /// </summary>
        public BlobRepository(string connectString)
        {
            _blobaccount = CloudStorageAccount.Parse(connectString); //read connection string

            _bro = new BlobRequestOptions()
            {
                SingleBlobUploadThresholdInBytes = 1024 * 1024, //1MB, the minimum
                ParallelOperationThreadCount = 1,
                RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(2), 3),
            };

            _cloudBlobClient = _blobaccount.CreateCloudBlobClient();
            _cloudBlobClient.DefaultRequestOptions = _bro;
        }


        /// <summary>
        /// Download file from blob storage latest or for exact date of run
        /// </summary>
        /// <param name="fileName">File from cloud would be stored in this location</param>
        /// <param name="runDate">Null if you need lates or exact date, time will be disregarded</param>
        public bool DownLoadFile(string ContainerName, string fileName, DateTime? runDate = null)
        {
            var cloudBlobContainer = _cloudBlobClient.GetContainerReference(ContainerName.ToLower());
            cloudBlobContainer.CreateIfNotExists();
            var blobs = cloudBlobContainer.ListBlobs(null, true, BlobListingDetails.Metadata, _bro, null);

            ICloudBlob blob = null;
            if (runDate.HasValue)
            {
                blob = blobs.Where(x => x is ICloudBlob && x.Uri.ToString().Contains(Path.GetFileNameWithoutExtension(fileName)) && long.Parse(((ICloudBlob)x).Metadata["RunDate"] ?? "0") == runDate.Value.Date.Ticks)
                        .Cast<ICloudBlob>().OrderByDescending(x => x.Properties.LastModified.Value.Ticks)
                        .FirstOrDefault();

            }
            else
            {
                blob = blobs.Where(x => x is ICloudBlob && x.Uri.ToString().Contains(Path.GetFileNameWithoutExtension(fileName))).Cast<ICloudBlob>().OrderByDescending(x => ((ICloudBlob)x).Properties.LastModified.Value.Ticks).FirstOrDefault();
            }

            if (blob != null)
            {
                blob.DownloadToFile(fileName, FileMode.CreateNew);
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool DeleteFile(string ContainerName, string fileName)
        {
            var cloudBlobContainer = _cloudBlobClient.GetContainerReference(ContainerName.ToLower());
            cloudBlobContainer.CreateIfNotExists();
            var blobs = cloudBlobContainer.ListBlobs(null, true, BlobListingDetails.Metadata, _bro, null);

            ICloudBlob blob = blobs.Where(x => x is ICloudBlob && String.Compare(Path.GetFileName(x.Uri.AbsolutePath), fileName, true) == 0).Cast<ICloudBlob>().FirstOrDefault();

            if (blob != null)
            {
                return blob.DeleteIfExists(DeleteSnapshotsOption.IncludeSnapshots); ;
            }
            else
            {
                return false;
            }


        }

        public List<string> GetBlobList(string ContainerName)
        {
            var cloudBlobContainer = _cloudBlobClient.GetContainerReference(ContainerName.ToLower());
            cloudBlobContainer.CreateIfNotExists();
            var blobs = cloudBlobContainer.ListBlobs(null, true, BlobListingDetails.Metadata, _bro, null);

            return blobs.Where(x => x is ICloudBlob).Select(x => {
                return Path.GetFileName(x.Uri.AbsolutePath);
            }).ToList();

        }

        /// <summary>
        /// Upload file in blob storage. If it exists will override it
        /// </summary>
        /// <param name="fileName">File in this location will be stored in cloud</param>
        /// <param name="metadata"></param>
        public string UploadFile(string ContainerName, string fileName, string TempFolder, Dictionary<string, string> metadata = null)
        {
            string tmpfileName = Path.Combine(TempFolder, Path.GetFileName(fileName));
            try
            {
                string ErrorMsg = null;
         
                var cloudBlobContainer = _cloudBlobClient.GetContainerReference(ContainerName.ToLower());
                cloudBlobContainer.CreateIfNotExists();
                //cloudBlobContainer.GetSharedAccessSignature()
                CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(Path.GetFileName(tmpfileName));

                blob.StreamWriteSizeInBytes = 256 * 1024;
                blob.UploadFromFile(tmpfileName, AccessCondition.GenerateIfExistsCondition() , new BlobRequestOptions(), new OperationContext());

                metadata.Add("FileName", tmpfileName);
                metadata.Add("RunDate", DateTime.Now.Date.Ticks.ToString());

                if (metadata != null)
                    metadata.Keys.ToList().ForEach(key =>
                    {
                        var value = metadata[key];
                        value = Regex.Replace(value,
                            @"[,\(\)" + Regex.Escape(new string(Path.GetInvalidFileNameChars())) + "]", "_");
                        if (!blob.Metadata.ContainsKey(key))
                        {
                            blob.Metadata.Add(key, value);
                        }
                        else
                        {
                            blob.Metadata[key] = value;
                        }
                    });


                blob.SetMetadata();

                return blob.Uri.ToString();
            }
            catch (Exception ex)
            {

                Console.WriteLine("Uploading fail: " + ex);
                return null;
            }


        }
    }

