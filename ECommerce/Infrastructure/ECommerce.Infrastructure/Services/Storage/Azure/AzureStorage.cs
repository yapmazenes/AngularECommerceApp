using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ECommerce.Application.Abstractions.Storage.Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Infrastructure.Services.Storage.Azure
{
    public class AzureStorage : Storage, IAzureStorage
    {
        private readonly BlobServiceClient _blobServiceClient;
        private BlobContainerClient _blobContainerClient;

        public AzureStorage(IConfiguration configuration)
        {
            _blobServiceClient = new BlobServiceClient(configuration["Storage:Azure"]);
        }

        public async Task DeleteAsync(string pathOrContainerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(pathOrContainerName);
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);

            await blobClient.DeleteAsync();
        }

        public IEnumerable<string> GetFilesAsync(string pathOrContainerName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(pathOrContainerName);

            return _blobContainerClient.GetBlobs().Select(b => b.Name);
        }

        public bool HasFile(string pathOrContainerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(pathOrContainerName);
            return _blobContainerClient.GetBlobs().Any(b => b.Name == fileName);
        }

        public async Task<List<(string FileName, string PathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(pathOrContainerName);
            await _blobContainerClient.CreateIfNotExistsAsync();
            await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);
            List<(string fileName, string pathOrContainerName)> datas = new();
            foreach (IFormFile file in files)
            {
                string fileNewName = await FileRenameAsync(pathOrContainerName, file.Name, HasFile);

                BlobClient blobClient = _blobContainerClient.GetBlobClient(fileNewName);
                await blobClient.UploadAsync(file.OpenReadStream());
                datas.Add((fileNewName, pathOrContainerName));
            }

            return datas;
        }
    }
}
