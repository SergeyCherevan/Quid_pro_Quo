using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

using Quid_pro_Quo.WebApiModels;
using Quid_pro_Quo.Services.Interfaces;
using Quid_pro_Quo.Exceptions;

namespace Quid_pro_Quo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        IAccountService _accountService { get; set; }
        IUserService _userService { get; set; }
        public AccountController(IAccountService accountService, IUserService userService)
        {
            _accountService = accountService;
            _userService = userService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginApiModel model)
        {
            try
            {
                return await _accountService.Login(model.UserName, model.Password);
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

        [HttpPost]
        [Route("registration")]
        public async Task<ActionResult<UserApiModel>> Registration([FromBody] LoginApiModel model)
        {
            try
            {
                return await _accountService.Registration(model.UserName, model.Password);
            }
            catch (AlreadyExistAppException)
            {
                return BadRequest(new
                {
                    Error = "User already exist"
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

        [HttpPost]
        [Route("currentUser/{username}")]
        public async Task<ActionResult<UserApiModel>> CurrentUser(string username)
        {
            try
            {
                return await _userService.GetUserByName(username);
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
    }
}
