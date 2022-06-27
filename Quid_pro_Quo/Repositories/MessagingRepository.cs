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

        public async Task<MessagingEntity> GetById(int id1, int id2)
            => (await _db.Messagings.FindAsync(
                e => e.User1Id == id1 && e.User2Id == id2 || e.User1Id == id2 && e.User2Id == id1
            )).FirstOrDefault();

        public async Task<ICollection<MessagingCardApiModel>> GetListOfCompanions(int id)
        {
            var messagings = (ICollection<MessagingEntity>)await _db.Messagings.FindAsync(
                e => e.User1Id == id || e.User2Id == id
            );

            IEnumerable<MessagingCardApiModel> messagingCards
                = messagings
                .Select(async e => await new MessagingMapping(_userRepository)
                    .ToMessagingCardAM(e, e.User1Id == id ? e.User2Id : e.User1Id))
                .Select(e => e.Result);


            return (ICollection<MessagingCardApiModel>)messagingCards;
        }

        public async Task<IQueryable<MessagingEntity>> GetByPredicate(Expression<Func<MessagingEntity, bool>> predicate)
            => (IQueryable<MessagingEntity>)await _db.Messagings.FindAsync(
                predicate ?? (e => true)
            );

        public async Task<MessagingEntity> Add(int id1, int id2, MessageEntity message)
        {
            MessagingEntity messaging;

            if ((messaging = await GetById(id1, id2)) == null)
            {
                 messaging = new MessagingEntity()
                 {
                     User1Id = id1,
                     User2Id = id2,
                     MessagesList = new List<MessageEntity>() { message }
                 };

                 _db.Messagings.InsertOne(messaging);
            }
            else
            {
                messaging.MessagesList.Add(message);
            }

            _db.Messagings.UpdateOne(
                e => e.User1Id == messaging.User1Id && e.User2Id == messaging.User2Id,
                BsonDocument.Create(messaging)
            );

            return messaging;
        }

        public async Task<MessagingEntity> Delete(int id1, int id2)
        {
            var messaging = await GetById(id1, id2);

            _db.Messagings.DeleteOne(
                e => e.User1Id == messaging.User1Id && e.User2Id == messaging.User2Id
            );

            return messaging;
        }

        public async Task<MessagingEntity> Delete(int id1, int id2, int idMessage)
        {
            var messaging = await GetById(id1, id2);
            var message = messaging.MessagesList.FirstOrDefault(e => e.Id == idMessage);

            messaging.MessagesList.Remove(message);

            _db.Messagings.UpdateOne(
                e => e.User1Id == messaging.User1Id && e.User2Id == messaging.User2Id,
                BsonDocument.Create(messaging)
            );

            return messaging;
        }

        public async Task<MessagingEntity> SetViewed(int id1, int id2)
        {
            var messaging = await GetById(id1, id2);

            messaging.MessagesList = messaging.MessagesList.Select(m => { m.Viewed = true; return m; }).ToList();

            _db.Messagings.UpdateOne(
                e => e.User1Id == messaging.User1Id && e.User2Id == messaging.User2Id,
                BsonDocument.Create(messaging)
            );

            return messaging;
        }
    }
}
