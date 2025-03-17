using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using AzureLearning.IServices;
using AzureLearning.Models;
using NuGet.Common;
using System;

namespace AzureLearning.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        BlobDetails IBlobService.GetABlobAsync(string containerName, string blobName)
        {
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobDetails? blobDetails = new BlobDetails();
            foreach (BlobItem blob in container.GetBlobs())
            {
                if (blob.Name == blobName)
                {

                    blobDetails.BlobName = blob.Name;
                    blobDetails.BlobType = blob.Properties.ContentType;
                    blobDetails.Url = GenerateUrlSasTokenForBlob(blob.Name, container);
                }
            }
            return blobDetails;
        }

        async Task<List<string>> IBlobService.GetAllBlobsAsync(string containerName)
        {
            List<string> blobnames = new List<string>();
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(containerName);
            string token = string.Empty;
            if (container.CanGenerateSasUri)
            {
                //BlobSasBuilder builder = new BlobSasBuilder();
                //builder.BlobContainerName= container.Name;
                //builder.ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(10);
                //token= container.GenerateSasUri(builder).AbsolutePath.Split('?')[1].ToString();
            }
            await foreach (BlobItem blobItem in container.GetBlobsAsync())
            {
                blobnames.Add(blobItem.Name);
            }
            return blobnames;
        }


        string GenerateUrlSasTokenForBlob(string blobName, BlobContainerClient blobContainerClient)
        {
            // we will generate the token blob-item wise.
            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);
            string tokenStr = string.Empty;
            if (blobClient.CanGenerateSasUri)
            {
                BlobSasBuilder blobSasBuilder = new BlobSasBuilder();
                blobSasBuilder.BlobContainerName = blobContainerClient.Name;
                blobSasBuilder.BlobName = blobName;
                blobSasBuilder.Resource = "b";// when will generate the Blob wise token we will use 'b' in resource or container wise we will 'c'
                blobSasBuilder.ExpiresOn = DateTime.UtcNow.AddMinutes(10);
                blobSasBuilder.SetPermissions(BlobSasPermissions.Read | BlobSasPermissions.Write);
                tokenStr = blobClient.GenerateSasUri(blobSasBuilder).AbsoluteUri;
            }
            return tokenStr;
        }

        async Task<List<BlobDetails>> IBlobService.GetBlobsWithFullUrlAsync(string containerName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            List<BlobDetails> blobDetails = new List<BlobDetails>();
            string token = string.Empty;
            if (containerClient.CanGenerateSasUri && !string.IsNullOrEmpty(containerClient.Name))
            {
                BlobSasBuilder blobSasBuilder = new BlobSasBuilder();
                blobSasBuilder.BlobContainerName = containerClient.Name;
                blobSasBuilder.Resource = "c";
                blobSasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(10);
                blobSasBuilder.SetPermissions(BlobSasPermissions.Read);
                token=containerClient.GenerateSasUri(blobSasBuilder).AbsoluteUri.Split('?')[1].ToString();
            }
            await foreach (BlobItem blob in containerClient.GetBlobsAsync())
            {
                BlobClient blobClient= containerClient.GetBlobClient(blob.Name);
                blobDetails.Add(new BlobDetails { BlobName= blobClient.Name, Url=string.Join("?",blobClient.Uri.AbsoluteUri,token),BlobType=blobClient.GetProperties().Value.ContentType });
            }
            return blobDetails;
        }
    }
}
