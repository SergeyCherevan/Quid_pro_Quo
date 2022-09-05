using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Quid_pro_Quo.Database.Ralational;
using Quid_pro_Quo.Services.Interfaces;
using Quid_pro_Quo.WebApiModels;
using Quid_pro_Quo.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace Quid_pro_Quo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeOfServicesController : ControllerBase
    {
        IExchangeOfServicesService _exchangeOfServicesService { get; set; }
        public ExchangeOfServicesController(IExchangeOfServicesService exchangeOfServicesService)
        {
            _exchangeOfServicesService = exchangeOfServicesService;
        }

        [HttpGet]
        [Route("getBySender/{senderName}/{proposalStatus}")]
        public async Task<ActionResult> GetBySender(string senderName, int proposalStatus)
        {
            try
            {
                return Ok(await _exchangeOfServicesService.GetBySender(senderName, (StatusEnum)proposalStatus));
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
        [Route("getByDestination/{destinationName}/{proposalStatus}")]
        public async Task<ActionResult> GetByDestination(string destinationName, int proposalStatus)
        {
            try
            {
                return Ok(await _exchangeOfServicesService.GetByDestination(destinationName, (StatusEnum)proposalStatus));
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
        [Route("getConfirmed/{userName}/{doneStatus}")]
        public async Task<ActionResult> GetConfirmed(string userName, int doneStatus)
        {
            try
            {
                return Ok(await _exchangeOfServicesService.GetConfirmed(userName, (StatusEnum)doneStatus));
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
        [Route("sendProposal")]
        public async Task<ActionResult<ExchangeOfServicesApiModel>> SendProposal(SendProposalToExchangeApiModel model)
        {
            try
            {
                return await _exchangeOfServicesService.SendProposal(model);
            }
            catch (Exception exp)
            {
                return BadRequest(new
                {
                    Error = exp.Message
                });
            }
        }

        public record IdRequestRecord(int ExchangeId);

        [Authorize]
        [HttpPost]
        [Route("confirmProposal")]
        public async Task<ActionResult<ExchangeOfServicesApiModel>> ConfirmProposal([FromBody] IdRequestRecord model)
        {
            try
            {
                return await _exchangeOfServicesService.ConfirmProposal(model.ExchangeId);
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
        [Route("cancelProposal")]
        public async Task<ActionResult<ExchangeOfServicesApiModel>> CancelProposal([FromBody] IdRequestRecord model)
        {
            try
            {
                return await _exchangeOfServicesService.CancelProposal(model.ExchangeId);
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
