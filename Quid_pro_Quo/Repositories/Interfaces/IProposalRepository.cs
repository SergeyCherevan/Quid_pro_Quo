using System.Collections.Generic;
using System.Threading.Tasks;

using Quid_pro_Quo.Database.Ralational;

namespace Quid_pro_Quo.Repositories.Interfaces
{
    public interface IProposalRepository : IBaseRepository<ProposalEntity, int>
    {
        Task<IEnumerable<ProposalEntity>> GetBySender(int senderId, int pageNumber, int pageSize);
        Task<IEnumerable<ProposalEntity>> GetByDestination(int destinationId, int pageNumber, int pageSize);
    }
}
