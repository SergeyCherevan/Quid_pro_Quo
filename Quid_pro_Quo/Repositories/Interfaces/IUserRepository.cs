using System.Collections.Generic;
using System.Threading.Tasks;

using Quid_pro_Quo.Database.Ralational;

namespace Quid_pro_Quo.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<UserEntity, int>
    {
        Task<UserEntity> GetByName(string userName);
        Task<string> GetHashPasswordById(int id);
        Task<IEnumerable<UserEntity>> GetByFilter(string keywords, int pageNumber, int pageSize);
        Task<int> GetCountByFilter(string keywords);
    }
}
