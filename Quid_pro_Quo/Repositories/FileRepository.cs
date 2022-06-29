using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.IO;
using System.Threading.Tasks;

namespace Quid_pro_Quo.Repositories
{
    public class FileRepository
    {
        protected ControllerBase _controller { get; set; }
        protected IWebHostEnvironment _env { get; set; }
        public FileRepository(ControllerBase controller, IWebHostEnvironment env)
        {
            _controller = controller;
            _env = env;
        }

        public async Task<PhysicalFileResult> GetByName(string folderName, string fileName)
        {
            await Task.Run(() => { });

            string file_path = Path.Combine(_env.ContentRootPath, $"FileStorage/{folderName}/{fileName}");
            string file_type = "application/" + fileName.Substring(fileName.LastIndexOf(".") + 1);
            string file_name = fileName;

            return _controller.PhysicalFile(file_path, file_type, file_name);
        }

        public async Task<string> Add(string folderName, IFormFile file)
        {
            await Task.Run(() => { });

            string id = Guid.NewGuid().ToString();
            string fileName = $"{id}.{file.FileName.Substring(file.FileName.LastIndexOf(".") + 1)}";
            string filePath = Path.Combine(_env.ContentRootPath, $"FileStorage/{folderName}/{fileName}");

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return fileName;
        }

        public async Task Delete(string folderName, string fileName)
        {
            await Task.Run(() => { });

            string filePath = Path.Combine(_env.ContentRootPath, $"FileStorage/{folderName}/{fileName}");

            File.Delete(filePath);
        }
    }
}
