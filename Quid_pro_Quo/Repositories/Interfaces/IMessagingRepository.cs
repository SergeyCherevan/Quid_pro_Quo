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
        Task<MessagingEntity> GetById(int id1, int id2);
        Task<ICollection<MessagingCardApiModel>> GetListOfCompanions(int id);
        Task<IQueryable<MessagingEntity>> GetByPredicate(Expression<Func<MessagingEntity, bool>> predicate);

        Task<MessagingEntity> Add(int id1, int id2, MessageEntity message);

        Task<MessagingEntity> Delete(int id1, int id2);
        Task<MessagingEntity> SetViewed(int id1, int id2);
    }
}
