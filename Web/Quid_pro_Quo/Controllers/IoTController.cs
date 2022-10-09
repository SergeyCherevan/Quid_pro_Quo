using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;

using Quid_pro_Quo.WebApiModels;
using Quid_pro_Quo.Services.Interfaces;
using Quid_pro_Quo.Exceptions;

namespace Quid_pro_Quo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IoTController : ControllerBase
    {
        IIoTService _iotService { get; set; }
        IExchangeOfServicesService _exchangeOfServicesService { get; set; }
        public IoTController(IExchangeOfServicesService exchangeOfServicesService, IIoTService ioTService)
        {
            _iotService = ioTService;
            _exchangeOfServicesService = exchangeOfServicesService;
        }



        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<JwtApiModel>> Login([FromBody] IoTLoginApiModel model)
        {
            try
            {
                return await _iotService.Login(model.IoTCode, model.Password);
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
        [Route("attach")]
        public async Task<ActionResult<JwtApiModel>> Attach()
        {
            try
            {
                string iotCodeStr = User.Claims.ToList().Find(claim => claim.Type == "IoTCode").Value;
                int iotCode = Convert.ToInt32(iotCodeStr);

                return await _iotService.AttachToUser(iotCode);
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

        [Authorize]
        [HttpPost]
        [Route("confirmServiceCompletion")]
        public async Task<ActionResult<ExchangeOfServicesApiModel>> ConfirmServiceCompletion(ConfirmServiceCompletionApiModel model)
        {
            try
            {
                string iotCodeStr = User.Claims.ToList().Find(claim => claim.Type == "IoTCode").Value;
                int iotCode = Convert.ToInt32(iotCodeStr);

                ExchangeOfServicesApiModel exchange = await _exchangeOfServicesService.ConfirmServiceCompletion(iotCode, model);

                if (exchange != null)
                {
                    return exchange;
                }
                
                return Ok("Any exchanges were not confirmed");
            }
            catch (Exception exp)
            {
                return BadRequest(new
                {
                    Error = exp.Message
                });
            }
        }

        public record IoTCodeRecord(int IoTCode);

        [Authorize]
        [HttpPost]
        [Route("addRequestToAttach")]
        public async Task<ActionResult<ExchangeOfServicesApiModel>> AddRequestToAttach(IoTCodeRecord model)
        {
            try
            {
                string ownerName = User.Identity.Name;
                int iotCode = model.IoTCode;

                await _iotService.AddRequestToAttach(ownerName, iotCode);

                return Ok("Request to attach IoT to user was added");
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
