using System.Threading.Tasks;

using Quid_pro_Quo.Database.Ralational;

namespace Quid_pro_Quo.Repositories.Interfaces
{
    public interface IIoTRepository : IBaseRepository<IoTEntity, int>
    {
        Task<IoTEntity> GetByCode(int code);
        Task<IoTEntity> GetByOwnerId(int ownerId);
        Task<IoTEntity> GetByOwnerName(string ownerName);
    }
}
