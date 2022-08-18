using System.Collections.Generic;
using System.Threading.Tasks;

using Quid_pro_Quo.Database.Ralational;

namespace Quid_pro_Quo.Repositories.Interfaces
{
    public interface IExchangeOfServicesRepository : IBaseRepository<ExchangeOfServicesEntity, int>
    {
        Task<IEnumerable<ExchangeOfServicesEntity>> GetBySender(int senderId, int pageNumber, int pageSize);
        Task<IEnumerable<ExchangeOfServicesEntity>> GetByDestination(int destinationId, int pageNumber, int pageSize);
    }
}
