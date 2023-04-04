using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using XmlDataExtractManager.Interfaces;

namespace XmlDataExtractManager.Services
{
    public class BufferedFileUploadService : IBufferedFileUploadService
    {
        public async Task<(bool, string)> UploadFile(IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadedFiles"));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    var filePath = Path.Combine(path, file.FileName);
                    return (true, filePath);
                }
                else
                {
                    return (false, "");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("File Copy Failed", ex);
            }
        }
    }
}
