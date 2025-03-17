using Azure.Storage.Blobs;

namespace AzureLearning.IServices
{
    public interface IContainerService
    {
        Task AddContainer(string containerName);
        Task<bool> RemoveContainer(string containerName);
        Task<IEnumerable<string>> GetContainers();
        BlobContainerClient GetContainer(string containerName);
    }
}
