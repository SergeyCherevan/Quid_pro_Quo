using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quid_pro_Quo.Services.Interfaces;
using Quid_pro_Quo.WebApiModels;
using System;
using System.Threading.Tasks;

namespace Quid_pro_Quo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IoTController : ControllerBase
    {
        IExchangeOfServicesService _exchangeOfServicesService { get; set; }
        public IoTController(IExchangeOfServicesService exchangeOfServicesService)
        {
            _exchangeOfServicesService = exchangeOfServicesService;
        }

        //[Authorize]
        [HttpPost]
        [Route("confirmServiceCompletion")]
        public async Task<ActionResult<ExchangeOfServicesApiModel>> ConfirmServiceCompletion(ConfirmServiceCompletionApiModel model)
        {
            try
            {
                int iotCode = 987654321;
                return await _exchangeOfServicesService.ConfirmServiceCompletion(iotCode, model);
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
