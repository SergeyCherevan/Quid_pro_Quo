using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using System.Threading.Tasks;
using System;

using Quid_pro_Quo.WebApiModels;
using Quid_pro_Quo.Services.Interfaces;
using Quid_pro_Quo.Exceptions;
using Quid_pro_Quo.Services;
using Quid_pro_Quo.Mappings;

namespace Quid_pro_Quo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        IAccountService _accountService { get; set; }
        IUserService _userService { get; set; }
        public AccountController(IAccountService accountService, IUserService userService, IWebHostEnvironment env)
        {
            _accountService = accountService;
            _userService = userService;

            (_accountService as AccountService).SetProperties(this, env);
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
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordApiModel model)
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

        [Authorize]
        [HttpPut]
        [Route("edit")]
        public async Task<ActionResult<UserApiModel>> Edit([FromForm] AccountFormApiModel model)
        {
            try
            {
                return await _accountService.Edit(model.ToAccountFormDTO(User.Identity.Name));
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



        [Authorize]
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
