using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;

using Quid_pro_Quo.Database.NoSql;
using Quid_pro_Quo.Repositories.Interfaces;
using Quid_pro_Quo.WebApiModels;
using Quid_pro_Quo.Mappings;

namespace Quid_pro_Quo.Repositories
{
    public class MessagingRepository : IMessagingRepository
    {
        protected IQuidProQuoMongoDbContext _db;
        protected IUserRepository _userRepository;
        public MessagingRepository(IQuidProQuoMongoDbContext db, IUserRepository userRepository)
        {
            _db = db;
            _userRepository = userRepository;
        }

        public async Task<MessagingEntity> GetByUsersId(int id1, int id2)
        {
            MessagingEntity messaging = (await _db.Messagings.FindAsync(
                e => e.User1Id == id1 && e.User2Id == id2 || e.User1Id == id2 && e.User2Id == id1
            )).FirstOrDefault();

            if (messaging is null)
            {
                return new MessagingEntity
                {
                    User1Id = id1,
                    User2Id = id2,
                    MessagesList = new List<MessageEntity>(),
                };
            }
            else
            {
                messaging.MessagesList = messaging.MessagesList.ToList();
            }

            return messaging.OrderByMyAndCompanion(id1, id2);
        }

        public async Task<IEnumerable<MessagingCardApiModel>> GetMessagingCardsOfCompanions(int id)
        {
            IEnumerable<MessagingEntity> messagings = (await _db.Messagings.FindAsync(
                e => e.User1Id == id || e.User2Id == id
            )).ToList();

            IEnumerable<MessagingCardApiModel> messagingCards
                = messagings
                .Select(async e => await e.ToMessagingCardApiModel(e.User1Id == id ? e.User2Id : e.User1Id, _userRepository))
                .Select(e => e.Result);


            return messagingCards;
        }

        public async Task<IEnumerable<MessageEntity>> GetNewMessagesInMessaging(int id1, int id2)
        {
            MessagingEntity messaging = await GetByUsersId(id1, id2);
            
            IEnumerable<MessageEntity> messages = messaging.MessagesList.ToList().Where(e => e.NotViewed ?? false).ToList();

            return messages;
        }



        public async Task<MessagingEntity> Add(int id1, int id2, MessageEntity message)
        {
            MessagingEntity messaging = await GetByUsersId(id1, id2);

            if (messaging.MessagesList.Count() == 0)
            {
                message.Id = 0;
                messaging.MessagesList = new List<MessageEntity>() { message };

                _db.Messagings.InsertOne(messaging);
            }
            else
            {
                message.Id = messaging.MessagesList.Any() ? messaging.MessagesList.Last().Id + 1 : 0;
                messaging.MessagesList.Add(message);

                _db.Messagings.ReplaceOne(
                    new BsonDocument {
                        { "user1Id", messaging.User1Id },
                        { "user2Id", messaging.User2Id },
                    },
                    messaging
                );
            }

            return messaging;
        }

        public async Task<MessagingEntity> SetViewed(int id1, int id2, int idMessage)
        {
            var messaging = await GetByUsersId(id1, id2);
            var message = messaging.MessagesList.FirstOrDefault(e => e.Id == idMessage);

            message.NotViewed = null;

            _db.Messagings.ReplaceOne(
                new BsonDocument {
                    { "user1Id", messaging.User1Id },
                    { "user2Id", messaging.User2Id },
                },
                messaging
            );

            return messaging;
        }



        public async Task<IEnumerable<MessagingEntity>> GetByPredicate(Expression<Func<MessagingEntity, bool>> predicate)
            => (await _db.Messagings.FindAsync(
                predicate ?? (e => true)
            )).ToEnumerable();

        public async Task<MessagingEntity> Delete(int id1, int id2)
        {
            var messaging = await GetByUsersId(id1, id2);

            _db.Messagings.DeleteOne(
                e => e.User1Id == messaging.User1Id && e.User2Id == messaging.User2Id
            );

            return messaging;
        }

        public async Task<MessagingEntity> Delete(int id1, int id2, int idMessage)
        {
            var messaging = await GetByUsersId(id1, id2);
            var message = messaging.MessagesList.FirstOrDefault(e => e.Id == idMessage);

            messaging.MessagesList.Remove(message);

            _db.Messagings.UpdateOne(
                e => e.User1Id == messaging.User1Id && e.User2Id == messaging.User2Id,
                BsonDocument.Create(messaging)
            );

            return messaging;
        }
    }
}
