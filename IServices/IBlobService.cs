using AzureLearning.Models;

namespace AzureLearning.IServices
{
    public interface IBlobService
    {
        Task<List<string>> GetAllBlobsAsync(string containerName);
        BlobDetails GetABlobAsync(string containerName,string blobName);
        Task<List<BlobDetails>> GetBlobsWithFullUrlAsync(string containerName);
    }
}
