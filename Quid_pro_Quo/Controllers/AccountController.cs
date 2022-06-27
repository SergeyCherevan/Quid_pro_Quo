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
        public async Task<ActionResult<object>> Login([FromBody] LoginApiModel model)
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
        [Route("changePassword")]
        public async Task<ActionResult<UserApiModel>> ChangePassword([FromBody] ChangePasswordModel model)
        {
            try
            {
                await _accountService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                return Ok(new
                {
                    Message = "Password was changed"
                });
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

        [HttpGet]
        [Route("currentUser")]
        public async Task<ActionResult<UserApiModel>> CurrentUser()
        {
            try
            {
                return await _userService.GetUserByName(User.Identity.Name);
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
