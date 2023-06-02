using ECommerce.Application.Abstractions.Storage;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Infrastructure.Services.Storage
{
    public class StorageService : IStorageService
    {
        private readonly IStorage _storage;

        public StorageService(IStorage storage)
        {
            _storage = storage;
        }

        public string StorageName => _storage.GetType().Name;

        public async Task DeleteAsync(string pathOrContainerName, string fileName) => await _storage.DeleteAsync(pathOrContainerName, fileName);


        public async Task<IEnumerable<string>> GetFilesAsync(string pathOrContainerName) => await _storage.GetFilesAsync(pathOrContainerName);


        public bool HasFile(string pathOrContainerName, string fileName) => _storage.HasFile(pathOrContainerName, fileName);


        public async Task<List<(string FileName, string PathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files)
        {
            return await _storage.UploadAsync(pathOrContainerName, files);
        }
    }
}
