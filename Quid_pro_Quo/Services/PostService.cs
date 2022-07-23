using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Quid_pro_Quo.Database.Ralational;
using Quid_pro_Quo.DTOs;
using Quid_pro_Quo.Exceptions;
using Quid_pro_Quo.Mappings;
using Quid_pro_Quo.Repositories;
using Quid_pro_Quo.Repositories.Interfaces;
using Quid_pro_Quo.Services.Interfaces;
using Quid_pro_Quo.WebApiModels;

namespace Quid_pro_Quo.Services
{
    public class PostService : IPostService
    {
        protected IUnitOfWork _UoW { get; set; }
        public PostService(IUnitOfWork uow)
        {
            _UoW = uow;
        }

        protected FileRepository _fileRepository { get; set; }
        public void SetProperties(ControllerBase controller, IWebHostEnvironment env)
        {
            _fileRepository = new FileRepository(controller, env);
        }

        public async Task<PostGetApiModel> Publish(PostFormDTO post)
        {
            List<string> fileNames = new List<string>(10);

            foreach (IFormFile imageFile in post.ImageFiles)
            {
                fileNames.Add(await _fileRepository.Add("Images", imageFile));
            }

            PostEntity entity = await _UoW.PostRepository.Add(await post.ToPostEntity(_UoW.UserRepository, fileNames));
            PostGetApiModel returnPost = await entity.ToPostGetApiModel(_UoW.UserRepository);

            await _UoW.SaveChanges();

            return returnPost;
        }

        public async Task<PostGetApiModel> GetById(int id)
        {
            PostEntity post = await _UoW.PostRepository.GetById(id);
            if (post is null)
            {
                throw new NotFoundAppException($"post not found");
            }

            return await post.ToPostGetApiModel(_UoW.UserRepository);
        }

        public async Task<PostsPageApiModel> GetByFilter(GetPostByFilterApiModel model)
            => new PostsPageApiModel()
                {
                    Posts = (await _UoW.PostRepository.GetByFilter(model))
                                        .Select(async e => await e.ToPostGetApiModel(_UoW.UserRepository))
                                        .Select(e => e.Result),
                    PostsCount = await _UoW.PostRepository.GetCountByFilter(model),
                };

        public async Task<PostsPageApiModel> GetByAuthor(string authorName, int pageNumber, int pageSize)
        {
            UserEntity author = await _UoW.UserRepository.GetByName(authorName);
            IEnumerable<PostEntity> posts = await _UoW.PostRepository.GetByAuthorId(author.Id, pageNumber, pageSize);
            int postsCount = await _UoW.PostRepository.GetCountByAuthorId(author.Id);

            return new PostsPageApiModel()
            {
                Posts = posts.Select(async e => await e.ToPostGetApiModel(_UoW.UserRepository)).Select(e => e.Result),
                PostsCount = postsCount,
            };
        }
    }
}
