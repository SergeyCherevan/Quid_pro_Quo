using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Quid_pro_Quo.SignalRHubs;
using Quid_pro_Quo.Services.Interfaces;
using Quid_pro_Quo.WebApiModels;

namespace Quid_pro_Quo.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MessengerController : ControllerBase
    {
        IHubContext<MessengerHub> _messengerContext { get; set; }
        IMessagingService _messagingService { get; set; }
        public MessengerController(IHubContext<MessengerHub> messengerContext, IMessagingService messagingService)
        {
            _messengerContext = messengerContext;
            _messagingService = messagingService;
        }

        [HttpPost]
        [Route("sendMessage")]
        public async Task<IActionResult> SendMessage([FromForm] SendMessageApiModel message)
        {
            string userName = User.Identity.Name;

            await _messagingService.SendMessage(userName, message, DateTime.Now);
            IEnumerable<MessagingCardApiModel> messagingCards;

            if (userName != message.DestinationName)
            {
                messagingCards = await _messagingService.GetMessagingCards(userName);
                await _messengerContext.Clients.User(userName).SendAsync("MessagingsIsUpdated", messagingCards);
            }

            messagingCards = await _messagingService.GetMessagingCards(message.DestinationName);
            await _messengerContext.Clients.User(message.DestinationName).SendAsync("MessagingsIsUpdated", messagingCards);

            return Ok(new
            {
                Message = "Message was sended"
            });
        }
    }
}
