using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

using Quid_pro_Quo.Database.NoSql;
using Quid_pro_Quo.Database.Ralational;
using Quid_pro_Quo.Mappings;
using Quid_pro_Quo.Repositories;
using Quid_pro_Quo.Repositories.Interfaces;
using Quid_pro_Quo.Services.Interfaces;
using Quid_pro_Quo.WebApiModels;

namespace Quid_pro_Quo.Services
{
    public class MessagingService : IMessagingService
    {
        protected IUnitOfWork _UoW { get; set; }
        public MessagingService(IUnitOfWork uow)
        {
            _UoW = uow;
        }

        protected FileRepository _fileRepository { get; set; }
        public void SetProperties(ControllerBase controller, IWebHostEnvironment env)
        {
            _fileRepository = new FileRepository(controller, env);
        }

        public async Task<IEnumerable<MessagingCardApiModel>> GetMessagingCards(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException($"{nameof(userName)} not specified");
            }

            UserEntity user = await _UoW.UserRepository.GetByName(userName);

            return await _UoW.MessagingRepository.GetMessagingCardsOfCompanions(user.Id);
        }

        public async Task<MessagingApiModel> GetMessaging(string user1Name, string user2Name)
        {
            if (string.IsNullOrEmpty(user1Name))
            {
                throw new ArgumentException($"{nameof(user1Name)} not specified");
            }
            if (string.IsNullOrEmpty(user2Name))
            {
                return new MessagingApiModel()
                {
                    User1Name = user1Name,
                    User2Name = user2Name,
                    MessagesList = new List<MessageApiModel>(),
                };
            }

            UserEntity user1 = await _UoW.UserRepository.GetByName(user1Name),
                       user2 = await _UoW.UserRepository.GetByName(user2Name);

            return await (await _UoW.MessagingRepository.GetByUsersId(user1.Id, user2.Id))
                .ToMessagingApiModel(_UoW.UserRepository);
        }

        public async Task SendMessage(string authorName, SendMessageApiModel message, DateTime postedAt)
        {
            UserEntity user1 = await _UoW.UserRepository.GetByName(authorName),
                       user2 = await _UoW.UserRepository.GetByName(message.DestinationName);

            MessageEntity entity = message.ToMessageEntity(postedAt);

            if (message.ImageFile is not null)
            {
                entity.ImageFileName = await _fileRepository.Add("Images", message.ImageFile);
            }
            if (message.File is not null)
            {
                entity.FileName = await _fileRepository.Add("Files", message.File);
            }

            await _UoW.MessagingRepository.Add(user1.Id, user2.Id, entity);
        }

        public async Task<IEnumerable<MessageApiModel>> GetNewMessages(string user1Name, string user2Name)
        {
            UserEntity user1 = await _UoW.UserRepository.GetByName(user1Name),
                       user2 = await _UoW.UserRepository.GetByName(user2Name);

            return (await _UoW.MessagingRepository.GetNewMessagesInMessaging(user1.Id, user2.Id))
                .Select(m => m.ToMessageApiModel());
        }

        public async Task MessagesIsViewed(string user1Name, string user2Name, IList<int> messageIDs)
        {
            UserEntity user1 = await _UoW.UserRepository.GetByName(user1Name),
                       user2 = await _UoW.UserRepository.GetByName(user2Name);
            
            foreach (int idMessage in messageIDs)
            {
                await _UoW.MessagingRepository.SetViewed(user1.Id, user2.Id, idMessage);
            }
        }
    }
}
