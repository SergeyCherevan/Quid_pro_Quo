using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Quid_pro_Quo.Repositories.Interfaces;
using Quid_pro_Quo.Database.Ralational;

namespace Quid_pro_Quo.Repositories
{
    public class IoTRepository : BaseRelationalRepository<IoTEntity, int>, IIoTRepository
    {
        protected BaseRelationalRepository<LoginInfoEntity, int> _loginInfoRepository;
        public IoTRepository(QuidProQuoRelationalDbContext db) : base(db)
        {
            _loginInfoRepository = new BaseRelationalRepository<LoginInfoEntity, int>(db);
        }

        public async Task<IoTEntity> GetByCode(int code)
            => await _db.IoTs.FirstOrDefaultAsync(u => u.IoTCode == code);

        public async Task<IoTEntity> GetByOwnerId(int ownerId)
            => await _db.IoTs.FirstOrDefaultAsync(u => u.OwnerId == ownerId);

        public async Task<IoTEntity> GetByOwnerName(string ownerName)
            => await _db.IoTs.Include(u => u.Owner).FirstOrDefaultAsync(u => u.Owner.UserName == ownerName);
    }
}
