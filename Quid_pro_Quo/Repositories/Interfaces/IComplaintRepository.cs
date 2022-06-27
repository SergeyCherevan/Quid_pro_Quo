using System.Collections.Generic;
using System.Threading.Tasks;

using Quid_pro_Quo.Database.Ralational;

namespace Quid_pro_Quo.Repositories.Interfaces
{
    public interface IComplaintRepository : IBaseRepository<ComplaintEntity, int>
    {
        Task<IEnumerable<ComplaintEntity>> GetByPage(int pageNumber, int pageSize);
    }
}
