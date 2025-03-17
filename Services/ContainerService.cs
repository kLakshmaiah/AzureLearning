using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureLearning.IServices;

namespace AzureLearning.Services
{
    public class ContainerService : IContainerService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public ContainerService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        async Task IContainerService.AddContainer(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);//blobcontainerclient is reprsent the container
            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
        }

        BlobContainerClient IContainerService.GetContainer(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return null;
        }

        async Task<IEnumerable<string>> IContainerService.GetContainers()
        {
            List<string> containers = new List<string>();
            await foreach (BlobContainerItem item in _blobServiceClient.GetBlobContainersAsync())
            {
                containers.Add(item.Name);
            }
            return containers;
        }

        async Task<bool> IContainerService.RemoveContainer(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return await blobContainerClient.DeleteIfExistsAsync();
        }
    }
}
