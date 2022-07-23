using System.Collections.Generic;
using System.Threading.Tasks;

using Quid_pro_Quo.Database.Ralational;
using Quid_pro_Quo.WebApiModels;

namespace Quid_pro_Quo.Repositories.Interfaces
{
    public interface IPostRepository : IBaseRepository<PostEntity, int>
    {
        Task<IEnumerable<PostEntity>> GetByAuthorId(int id);
        Task<IEnumerable<PostEntity>> GetByFilter(GetPostByFilterApiModel model);
        Task<int> GetCountByFilter(GetPostByFilterApiModel model);
    }
}
