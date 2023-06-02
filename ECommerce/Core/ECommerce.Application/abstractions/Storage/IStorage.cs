using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Abstractions.Storage
{
    public interface IStorage
    {
        Task<List<(string FileName, string PathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files);

        Task DeleteAsync(string pathOrContainerName, string fileName);

        IEnumerable<string> GetFilesAsync(string pathOrContainerName);

        bool HasFile(string pathOrContainerName, string fileName);

    }
}
