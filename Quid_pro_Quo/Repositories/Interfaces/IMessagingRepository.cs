using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Quid_pro_Quo.Database.NoSql;
using Quid_pro_Quo.WebApiModels;

namespace Quid_pro_Quo.Repositories.Interfaces
{
    public interface IMessagingRepository
    {
        Task<MessagingEntity> GetByUsersId(int id1, int id2);
        Task<IEnumerable<MessagingCardApiModel>> GetMessagingCardsOfCompanions(int id);
        Task<IEnumerable<MessageEntity>> GetNewMessagesInMessaging(int id1, int id2);

        Task<MessagingEntity> Add(int id1, int id2, MessageEntity message);
        Task<MessagingEntity> SetViewed(int id1, int id2, int idMessage);

        Task<IEnumerable<MessagingEntity>> GetByPredicate(Expression<Func<MessagingEntity, bool>> predicate);
        Task<MessagingEntity> Delete(int id1, int id2);
        Task<MessagingEntity> Delete(int id1, int id2, int idMessage);
    }
}
