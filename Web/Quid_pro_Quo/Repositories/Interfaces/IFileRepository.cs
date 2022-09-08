using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace Quid_pro_Quo.Repositories.Interfaces
{
    public interface IFileRepository
    {
        Task<PhysicalFileResult> GetByName(string folderName, string fileName);
        Task Add(string folderName, IFormFile file);
    }
}
