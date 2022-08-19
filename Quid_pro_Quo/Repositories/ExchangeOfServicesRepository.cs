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
                .Where(e => e.RequestingPost.AuthorId == senderId && e.ProposalStatus == proposalStatus);
        }
        public async Task<IEnumerable<ExchangeOfServicesEntity>> GetByDestination(int destinationId, StatusEnum proposalStatus)
        {
            await Task.Run(() => { });

            return _db.ExchangesOfServicess
                .Include(e => e.RequestedPost)
                .Where(e => e.RequestedPost.AuthorId == destinationId && e.ProposalStatus == proposalStatus);
        }
        public async Task<IEnumerable<ExchangeOfServicesEntity>> GetConfirmed(int userId, StatusEnum doneStatus)
        {
            await Task.Run(() => { });

            return _db.ExchangesOfServicess
                .Include(e => e.RequestedPost)
                .Where(e => e.RequestingPost.AuthorId == userId || e.RequestedPost.AuthorId == userId)
                .ToList()
                .Where(e =>
                {
                    Predicate<ExchangeOfServicesEntity> pred = doneStatus switch
                    {
                        StatusEnum.NoInfo => (e => (e.DoneStatus1 == StatusEnum.NoInfo || e.DoneStatus1 == StatusEnum.NoInfo) &&
                            (e.DoneStatus1 != StatusEnum.No && e.DoneStatus1 != StatusEnum.No)),
                        StatusEnum.Yes => (e => e.DoneStatus1 == StatusEnum.Yes && e.DoneStatus1 == StatusEnum.Yes),
                        StatusEnum.No => (e => e.DoneStatus1 == StatusEnum.No || e.DoneStatus1 == StatusEnum.No),
                    };

                    return pred(e);
                });
        }
    }
}
