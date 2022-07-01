using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using Quid_pro_Quo.Repositories;

namespace Quid_pro_Quo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        FileRepository _fileRepository { get; set; }
        public FileController(IWebHostEnvironment env)
        {
            _fileRepository = new FileRepository(this, env);
        }

        [HttpGet]
        [Route("file/{fileName}")]
        public async Task<ActionResult> File(string fileName)
            => await _fileRepository.GetByName("Files", fileName);

        [HttpGet]
        [Route("image/{fileName}")]
        public async Task<ActionResult> Image(string fileName)
            => await _fileRepository.GetByName("Images", fileName);

        [HttpGet]
        [Route("avatar/{fileName}")]
        public async Task<ActionResult> Avatar(string fileName)
            => await _fileRepository.GetByName("Avatars", fileName);
    }
}
