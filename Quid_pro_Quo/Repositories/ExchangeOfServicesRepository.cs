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



        public async Task<IEnumerable<ExchangeOfServicesEntity>> GetBySender(int senderId, int pageNumber, int pageSize)
        {
            await Task.Run(() => { });

            string sqlQuery = $@"SELECT * FROM [ExchangesOfServices] AS prop
                                     INNER JOIN [Posts] AS post ON prop.RequestingPost = post.Id
                                     WHERE post.AuthorId = {senderId}
                                     LIMIT {pageSize} OFFSET {pageNumber * pageSize}";

            return _db.ExchangesOfServicess.FromSqlRaw(sqlQuery);

            //return _db.ExchangesOfServices.Include(e => e.RequestingPostId).Where(e => e.RequestingPost.Id == senderId);
        }

        public async Task<IEnumerable<ExchangeOfServicesEntity>> GetByDestination(int destinationId, int pageNumber, int pageSize)
        {
            await Task.Run(() => { });

            string sqlQuery = $@"SELECT * FROM [ExchangesOfServices] AS prop
                                     INNER JOIN [Posts] AS post ON prop.RequestedPost = post.Id
                                     WHERE post.AuthorId = {destinationId}
                                     LIMIT {pageSize} OFFSET {pageNumber * pageSize}";

            return _db.ExchangesOfServicess.FromSqlRaw(sqlQuery);

            //return _db.ExchangesOfServices.Include(e => e.RequestedPostId).Where(e => e.RequestedPost.Id == destinationId);
        }
    }
}
