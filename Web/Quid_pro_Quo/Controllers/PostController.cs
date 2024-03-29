﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using System;
using System.Threading.Tasks;
using System.Globalization;

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
        public async Task<ActionResult<PostsPageApiModel>> GetByFilter([FromQuery] GetPostByFilterApiModel model)
        {
            model.keywords ??= ""; model.geomarker ??= ""; model.date ??= "";

            try
            {
                return await _postService.GetByFilter(model);
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
        [Route("getByAuthor/{authorName}")]
        public async Task<ActionResult<PostsPageApiModel>> GetByAuthor
            ([FromRoute] string authorName, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                return await _postService.GetByAuthor(authorName, pageNumber, pageSize);
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

        [Authorize]
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
