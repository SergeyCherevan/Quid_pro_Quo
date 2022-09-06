using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Quid_pro_Quo.Repositories.Interfaces;
using Quid_pro_Quo.Database.Ralational;

namespace Quid_pro_Quo.Repositories
{
    public class ExchangeOfServicesRepository : BaseRelationalRepository<ExchangeOfServicesEntity, int>, IExchangeOfServicesRepository
    {
        public ExchangeOfServicesRepository(QuidProQuoRelationalDbContext db) : base(db) { }



        public async Task<IEnumerable<ExchangeOfServicesEntity>> GetBySender(int senderId, StatusEnum proposalStatus)
        {
            await Task.Run(() => { });

            return _db.ExchangesOfServicess
                .Include(e => e.RequestingPost)
                .Where(e => e.RequestingPost.AuthorId == senderId && e.ProposalStatus == proposalStatus)
                .OrderByDescending(e => e.Time);
        }
        public async Task<IEnumerable<ExchangeOfServicesEntity>> GetByDestination(int destinationId, StatusEnum proposalStatus)
        {
            await Task.Run(() => { });

            return _db.ExchangesOfServicess
                .Include(e => e.RequestedPost)
                .Where(e => e.RequestedPost.AuthorId == destinationId && e.ProposalStatus == proposalStatus)
                .OrderByDescending(e => e.Time);
        }
        public async Task<IEnumerable<ExchangeOfServicesEntity>> GetConfirmed(int userId, StatusEnum doneStatus)
        {
            await Task.Run(() => { });

            return _db.ExchangesOfServicess
                .Include(e => e.RequestedPost)
                .Where(e => e.ProposalStatus == StatusEnum.Yes && (e.RequestingPost.AuthorId == userId || e.RequestedPost.AuthorId == userId))
                .ToList()
                .Where(e =>
                {
                    Predicate<ExchangeOfServicesEntity>
                        predYes = e => e.DoneStatus1 == StatusEnum.Yes && e.DoneStatus2 == StatusEnum.Yes,
                        predNo = e => e.DoneStatus1 == StatusEnum.No || e.DoneStatus2 == StatusEnum.No,
                        predNoInfo = e => !predYes(e) && !predNo(e);

                    Predicate<ExchangeOfServicesEntity> pred = doneStatus switch
                    {
                        StatusEnum.Yes => e => predYes(e),
                        StatusEnum.No => e => predNo(e),
                        StatusEnum.NoInfo => e => predNoInfo(e),
                    };

                    return pred(e);
                })
                .OrderByDescending(e => e.Time);
        }
    }
}
