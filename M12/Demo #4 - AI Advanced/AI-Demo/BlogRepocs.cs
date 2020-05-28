using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage.RetryPolicies;

namespace AI_Demo
{
    public class BlobRepository
    {

        private CloudBlobClient _cloudBlobClient;
        private CloudStorageAccount _blobaccount;
        private BlobRequestOptions _bro;
        public string FileContainer = "files";

        /// <summary>
        /// Initialize controller
        /// </summary>
        public BlobRepository(string conString)
        {
            _blobaccount = CloudStorageAccount.Parse(conString); //read connection string

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
        public bool DownLoadFile(MemoryStream stream, string ContainerName, string fileName)
        {
            var cloudBlobContainer = _cloudBlobClient.GetContainerReference(ContainerName.ToLower());
            cloudBlobContainer.CreateIfNotExists();
            var blobs = cloudBlobContainer.ListBlobs(null, true, BlobListingDetails.Metadata, _bro, null);

            ICloudBlob blob = blobs.Where(x => x is ICloudBlob && x.Uri.ToString().Contains(Path.GetFileNameWithoutExtension(fileName))).Cast<ICloudBlob>().OrderByDescending(x => ((ICloudBlob)x).Properties.LastModified.Value.Ticks).FirstOrDefault();


            if (blob != null)
            {
                blob.DownloadToStream(stream);
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Upload file in blob storage. If it exists will override it
        /// </summary>
        /// <param name="fileName">File in this location will be stored in cloud</param>
        /// <param name="metadata"></param>
        public Uri UploadFile(string ContainerName, string fileName, Dictionary<string, string> metadata = null)
        {
            //try
            //{
            var cloudBlobContainer = _cloudBlobClient.GetContainerReference(ContainerName.ToLower());
            cloudBlobContainer.CreateIfNotExists();
            CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(Path.GetFileName(fileName));

            blob.StreamWriteSizeInBytes = 256 * 1024;
            blob.UploadFromFile(fileName, new AccessCondition(), new BlobRequestOptions(), new OperationContext());

            blob.Metadata.Add("FileName", fileName);
            blob.Metadata.Add("RunDate", DateTime.Now.Date.Ticks.ToString());

            if (metadata != null)
                metadata.Keys.ToList().ForEach(key =>
                {
                    var value = metadata[key];
                    value = Regex.Replace(value, @"[,\(\)" + Regex.Escape(new string(Path.GetInvalidFileNameChars())) + "]", "_");
                    if (blob.Metadata.ContainsKey(key))
                    {
                        blob.Metadata.Add(key, value);
                    }
                    else
                    {
                        blob.Metadata[key] = value;
                    }
                });


            blob.SetMetadata();

            return blob.Uri;

        }
    }
}
