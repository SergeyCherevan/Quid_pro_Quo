using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Quid_pro_Quo.WebApiModels;


namespace Quid_pro_Quo.Services.Interfaces
{
    public interface IMessagingService
    {
        Task<IEnumerable<MessagingCardApiModel>> GetMessagingCards(string userName);
        Task<MessagingApiModel> GetMessaging(string user1Name, string user2Name);
        Task SendMessage(string authorName, SendMessageApiModel message, DateTime postedAt);
        Task<IEnumerable<MessageApiModel>> GetNewMessages(string user1Name, string user2Name);
        Task MessagesIsViewed(string user1Name, string user2Name, IList<int> messageIDs);
    }
}
