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
               MessagesList = entity.MessagesList.ToList().Select(m => m.ToMessageApiModel()),
           };

        public static MessageApiModel ToMessageApiModel(this MessageEntity entity)
           => new MessageApiModel()
           {
               Id = entity.Id,
               AuthorNumber = entity.AuthorNumber,
               Text = entity.Text,
               ImageFileName = entity.ImageFileName,
               FileName = entity.FileName,
               PostedAt = entity.PostedAt,
           };

        public static MessageEntity ToMessageEntity(this SendMessageApiModel model, DateTime postedAt)
           => new MessageEntity()
           {
               Id = 0,
               AuthorNumber = false,
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
                    MessagesList = messaging.MessagesList.ToList().Select(message =>
                    {
                        message.AuthorNumber = !message.AuthorNumber;
                        return message;
                    }).ToList(),
                };
            }

            return messaging;
        }
    }
}
