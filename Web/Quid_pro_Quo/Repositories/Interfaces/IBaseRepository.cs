using System;
using System.Linq;
using System.Threading.Tasks;

using Quid_pro_Quo.Database.Ralational;

namespace Quid_pro_Quo.Repositories.Interfaces
{
    public interface IBaseRepository<Entity, IdType> where Entity : BaseEntity<IdType>
    {
        Task<Entity> GetById(IdType id);
        Task<IQueryable<Entity>> GetByPredicate(Func<Entity, bool> predicate);

        Task<Entity> Add(Entity entity);
        Task<Entity> Update(Entity entity);
        Task<Entity> Delete(IdType id);
    }
}
