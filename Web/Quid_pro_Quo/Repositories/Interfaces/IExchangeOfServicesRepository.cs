using System.Collections.Generic;
using System.Threading.Tasks;

using Quid_pro_Quo.Database.Ralational;

namespace Quid_pro_Quo.Repositories.Interfaces
{
    public interface IExchangeOfServicesRepository : IBaseRepository<ExchangeOfServicesEntity, int>
    {
        Task<IEnumerable<ExchangeOfServicesEntity>> GetBySender(int senderId, StatusEnum proposalStatus);
        Task<IEnumerable<ExchangeOfServicesEntity>> GetByDestination(int destinationId, StatusEnum proposalStatus);
        Task<IEnumerable<ExchangeOfServicesEntity>> GetConfirmed(int userId, StatusEnum doneStatus);
    }
}
