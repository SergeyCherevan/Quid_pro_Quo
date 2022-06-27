using System.Linq;
using System.Threading.Tasks;

using Quid_pro_Quo.Database.NoSql;
using Quid_pro_Quo.Repositories.Interfaces;
using Quid_pro_Quo.WebApiModels;

namespace Quid_pro_Quo.Mappings
{
    public class MessagingMapping
    {
        public IUserRepository UserRepository { get; set; }

        public MessagingMapping(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        public async Task<MessagingCardApiModel> ToMessagingCardAM(MessagingEntity messaging, int CompanionId)
        {
            var messagingCard = new MessagingCardApiModel()
            {
                CompanionId = CompanionId,
                UserName = (await UserRepository.GetById(CompanionId)).UserName,
                Viewed = messaging.MessagesList.All(m => m.Viewed)
            };

            return messagingCard;
        }
    }
}
