using System.Linq;
using System.Threading.Tasks;

using Quid_pro_Quo.Database.NoSql;
using Quid_pro_Quo.Repositories.Interfaces;
using Quid_pro_Quo.WebApiModels;

namespace Quid_pro_Quo.Mappings
{
    public static class MessagingMapping
    {
        public static async Task<MessagingCardApiModel> ToMessagingCardApiModel(this MessagingEntity messaging, int CompanionId, IUserRepository userRepository)
        {
            var messagingCard = new MessagingCardApiModel()
            {
                CompanionId = CompanionId,
                UserName = (await userRepository.GetById(CompanionId)).UserName,
                Viewed = messaging.MessagesList.All(m => m.Viewed)
            };

            return messagingCard;
        }
    }
}
