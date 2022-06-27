using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Quid_pro_Quo.Repositories.Interfaces;
using Quid_pro_Quo.Database.Ralational;

namespace Quid_pro_Quo.Repositories
{
    public class PostRepository : BaseRelationalRepository<PostEntity, int>, IPostRepository
    {
        public PostRepository(QuidProQuoRelationalDbContext db) : base(db) { }



        public async Task<IEnumerable<PostEntity>> GetByAuthorId(int id)
            => await Task.Run(() => _db.Posts.Where(e => e.Id == id));
        public async Task<IEnumerable<PostEntity>> GetByFilter(string keywords, int pageNumber, int pageSize)
        {
            await Task.Run(() => { });

            string condition =
                keywords != "" ?
                    "WHERE " + String.Join(" AND ", keywords.Split(" ").Select(e =>
                    $"(    LOWER([Title]) LIKE LOWER(\"%{e}%\")    OR    LOWER([Text]) LIKE LOWER(\"%{e}%\")    )"))
                :
                    "";

            string sqlQuery = $"SELECT * FROM [Posts] {condition} LIMIT {pageSize} OFFSET {pageNumber * pageSize}";

            return _db.Posts.FromSqlRaw(sqlQuery);
        }
    }
}
