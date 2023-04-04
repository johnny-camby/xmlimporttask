using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace XmlDataExtractManager.Interfaces
{
    public interface IBufferedFileUploadService
    {
        Task<(bool, string)> UploadFile(IFormFile file);
    }
}
