using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;
using System;

using Quid_pro_Quo.Services.Interfaces;
using Quid_pro_Quo.WebApiModels;
using Quid_pro_Quo.Exceptions;

namespace Quid_pro_Quo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService _userService { get; set; }
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("get/{userName}")]
        public async Task<ActionResult<UserApiModel>> Get(string userName)
        {
            try
            {
                return await _userService.GetUserByName(userName);
            }
            catch (NotFoundAppException)
            {
                return BadRequest(new
                {
                    Error = "User not exists"
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
        public async Task<ActionResult<UsersPageApiModel>> GetByFilter(string keywords, int pageNumber, int pageSize)
        {
            keywords ??= "";

            if (pageSize == 0)
            {
                return BadRequest(new
                {
                    Error = "Page size can't be equal 0"
                });
            }

            try
            {
                return await _userService.GetUsersByFilter(keywords, pageNumber, pageSize);
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

