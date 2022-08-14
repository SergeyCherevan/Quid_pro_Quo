using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Quid_pro_Quo.WebApiModels;
using Quid_pro_Quo.Services.Interfaces;
using Quid_pro_Quo.Services;

namespace Quid_pro_Quo.SignalRHubs
{
    [Authorize]
    public class MessengerHub : Hub
    {
        IMessagingService _messagingService { get; set; }
        public MessengerHub(IMessagingService messagingService, IWebHostEnvironment env)
        {
            _messagingService = messagingService;
            (_messagingService as MessagingService).SetProperties(null, env);
        }

        public async Task GetMessagingCardsRequest()
        {
            string userName = Context.User.Identity.Name;

            IEnumerable<MessagingCardApiModel> messagingCards
                = await _messagingService.GetMessagingCards(userName);

            await Clients.Caller.SendAsync("GetMessagingCardsResponse", messagingCards);
        }

        public async Task GetMessagingRequest(string companionName)
        {
            string userName = Context.User.Identity.Name;

            MessagingApiModel messaging
                = await _messagingService.GetMessaging(userName, companionName);

            await Clients.Caller.SendAsync("GetMessagingResponse", messaging);
        }


        public async Task GetNewMessagesRequest(string companionName)
        {
            string userName = Context.User.Identity.Name;

            IEnumerable<MessageApiModel> messages
                = await _messagingService.GetNewMessages(userName, companionName);

            await Clients.Caller.SendAsync("GetNewMessagesResponse", messages);
        }

        public async Task MessagesIsViewed(string companionName, IList<int> messageIDs)
        {
            string userName = Context.User.Identity.Name;

            await _messagingService.MessagesIsViewed(userName, companionName, messageIDs);
            IEnumerable<MessagingCardApiModel> messagingCards;

            messagingCards = await _messagingService.GetMessagingCards(userName);
            await Clients.Caller.SendAsync("MessagingsIsUpdated", messagingCards);

            messagingCards = await _messagingService.GetMessagingCards(companionName);
            await Clients.User(companionName).SendAsync("MessagingsIsUpdated", messagingCards);
        }
    }
}
