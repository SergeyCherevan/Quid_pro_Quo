using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Quid_pro_Quo.Repositories.Interfaces;
using Quid_pro_Quo.Database.Ralational;

namespace Quid_pro_Quo.Repositories
{
    public class ComplaintRepository : BaseRelationalRepository<ComplaintEntity, int>, IComplaintRepository
    {
        public ComplaintRepository(QuidProQuoRelationalDbContext db) : base(db) { }



        public async Task<IEnumerable<ComplaintEntity>> GetByPage(int pageNumber, int pageSize)
        {
            await Task.Run(() => { });

            string sqlQuery = $@"SELECT * FROM [Complaints] LIMIT {pageSize} OFFSET {pageNumber * pageSize}";

            return _db.Complaints.FromSqlRaw(sqlQuery);
        }
    }
}
