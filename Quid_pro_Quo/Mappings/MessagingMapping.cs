using System;
using System.Linq;
using System.Threading.Tasks;

using Quid_pro_Quo.Database.NoSql;
using Quid_pro_Quo.Repositories.Interfaces;
using Quid_pro_Quo.WebApiModels;

namespace Quid_pro_Quo.Mappings
{
    public static class MessagingMapping
    {
        public static async Task<MessagingCardApiModel> ToMessagingCardApiModel(this MessagingEntity entity, int CompanionId, IUserRepository userRepository)
        {
            var messagingCard = new MessagingCardApiModel()
            {
                UserName = (await userRepository.GetById(CompanionId)).UserName,
                CountOfNotViewedMessages = entity.MessagesList.Sum(m => (m.NotViewed ?? false) ? 1 : 0)
            };

            return messagingCard;
        }

        public static async Task<MessagingApiModel> ToMessagingApiModel(this MessagingEntity entity, IUserRepository userRepository)
           => new MessagingApiModel()
           {
               User1Name = (await userRepository.GetById(entity.User1Id)).UserName,
               User2Name = (await userRepository.GetById(entity.User2Id)).UserName,
               MessagesList = entity.MessagesList.ToList()
                   .Select(async m => await m.ToMessageApiModel(userRepository))
                   .Select(t => t.Result).ToList(),
           };

        public static async Task<MessageApiModel> ToMessageApiModel(this MessageEntity entity, IUserRepository userRepository)
           => new MessageApiModel()
           {
               Id = entity.Id,
               AuthorName = (await userRepository.GetById(entity.AuthorId)).UserName,
               Text = entity.Text,
               ImageFileName = entity.ImageFileName,
               FileName = entity.FileName,
               PostedAt = entity.PostedAt,
               NotViewed = entity.NotViewed ?? false,
           };

        public static async Task<MessageEntity> ToMessageEntity(this SendMessageApiModel model, IUserRepository userRepository, string userName, DateTime postedAt)
           => new MessageEntity()
           {
               Id = 0,
               AuthorId = (await userRepository.GetByName(userName)).Id,
               Text = model.Text,
               ImageFileName = model.ImageFile?.FileName,
               FileName = model.File?.FileName,
               PostedAt = postedAt,
               NotViewed = true,
           };

        public static MessagingEntity OrderByMyAndCompanion(this MessagingEntity messaging, int myId, int companionId)
        {
            bool haveToRevert = messaging.User1Id != myId;

            if (haveToRevert)
            {
                return new MessagingEntity
                {
                    User1Id = myId,
                    User2Id = companionId,
                    MessagesList = messaging.MessagesList.ToList(),
                };  
            }

            return messaging;
        }
    }
}
