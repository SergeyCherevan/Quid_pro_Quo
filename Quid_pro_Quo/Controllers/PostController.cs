using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

using Quid_pro_Quo.Services;
using Quid_pro_Quo.Services.Interfaces;
using Quid_pro_Quo.WebApiModels;
using Quid_pro_Quo.Exceptions;
using Quid_pro_Quo.Mappings;

namespace Quid_pro_Quo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        IPostService _postService { get; set; }
        public PostController(IPostService postService, IWebHostEnvironment env)
        {
            _postService = postService;
            (_postService as PostService).SetProperties(this, env);
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<ActionResult<PostGetApiModel>> Get(int id)
        {
            try
            {
                return await _postService.GetById(id);
            }
            catch (NotFoundAppException)
            {
                return BadRequest(new
                {
                    Error = "Post not exists"
                });
            }
            catch (Exception exp)
            {
                return BadRequest(new
                {
                    Error = exp.Message
                });
            }
        }

        [HttpGet]
        [Route("getByFilter")]
        public async Task<ActionResult<PostsPageApiModel>> GetByFilter(string keywords, string geomarker, int pageNumber, int pageSize)
        {
            keywords ??= ""; geomarker ??= "";

            if (pageSize == 0)
            {
                return BadRequest(new
                {
                    Error = "Page size can't be equal 0"
                });
            }

            try
            {
                return await _postService.GetByFilter(keywords, geomarker, pageNumber, pageSize);
            }
            catch (Exception exp)
            {
                return BadRequest(new
                {
                    Error = exp.Message
                });
            }
        }

        [HttpPost]
        [Route("publish")]
        public async Task<ActionResult<PostGetApiModel>> Publish([FromForm] PostFormApiModel model)
        {
            try
            {
                return await _postService.Publish(model.ToPostFormDTO(User.Identity.Name, DateTime.Now));
            }
            catch (Exception exp)
            {
                return BadRequest(new
                {
                    Error = exp.Message
                });
            }
        }
    }
}
