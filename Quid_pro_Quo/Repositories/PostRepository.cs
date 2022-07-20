using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Quid_pro_Quo.Repositories.Interfaces;
using Quid_pro_Quo.Database.Ralational;
using System.Globalization;

namespace Quid_pro_Quo.Repositories
{
    public class PostRepository : BaseRelationalRepository<PostEntity, int>, IPostRepository
    {
        public PostRepository(QuidProQuoRelationalDbContext db) : base(db) { }



        public async Task<IEnumerable<PostEntity>> GetByAuthorId(int id)
            => await Task.Run(() => _db.Posts.Where(e => e.Id == id));
        public async Task<IEnumerable<PostEntity>> GetByFilter(string keywords, string geomarker, int pageNumber, int pageSize)
        {
            await Task.Run(() => { });

            List<string> conditions = new List<string>();

            if (keywords != "")
            {
                conditions.Add(String.Join(" AND ", keywords.Split(" ").Select(e =>
                    $"(    LOWER([Title]) LIKE LOWER(\"%{e}%\")    OR    LOWER([Text]) LIKE LOWER(\"%{e}%\")    )")));
            }

            if (geomarker != "")
            {
                string lat = geomarker.Split(",")[0],
                       lng = geomarker.Split(",")[1];

                conditions.Add($@"
                    ([PerformServiceInPlaceLat] - {lat}) * ([PerformServiceInPlaceLat] - {lat})
                    +
                    ([PerformServiceInPlaceLng] - {lng}) * ([PerformServiceInPlaceLng] - {lng})
                    <= 1"
                );
            }

            if (conditions.Count > 0)
            {
                conditions[0] = "WHERE " + conditions[0];
            }

            string sqlQuery = $"SELECT * FROM [Posts] {String.Join(" AND ", conditions)} LIMIT {pageSize} OFFSET {pageNumber * pageSize}";

            return _db.Posts.FromSqlRaw(sqlQuery);
        }
        public async Task<int> GetCountByFilter(string keywords, string geomarker)
        {
            await Task.Run(() => { });

            List<string> conditions = new List<string>();

            if (keywords != "")
            {
                conditions.Add(String.Join(" AND ", keywords.Split(" ").Select(e =>
                    $"(    LOWER([Title]) LIKE LOWER(\"%{e}%\")    OR    LOWER([Text]) LIKE LOWER(\"%{e}%\")    )")));
            }

            if (geomarker != "")
            {
                string lat = geomarker.Split(",")[0],
                       lng = geomarker.Split(",")[1];

                conditions.Add($@"
                    ([PerformServiceInPlaceLat] - {lat}) * ([PerformServiceInPlaceLat] - {lat})
                    +
                    ([PerformServiceInPlaceLng] - {lng}) * ([PerformServiceInPlaceLng] - {lng})
                    <= 1"
                );
            }

            if (conditions.Count > 0)
            {
                conditions[0] = "WHERE " + conditions[0];
            }

            string sqlQuery = $"SELECT COUNT(*) AS Value FROM [Posts] {String.Join(" AND ", conditions)}";

            return _db.Set<ScalarReturn<int>>()
                .FromSqlRaw(sqlQuery)
                .AsEnumerable()
                .First().Value;
        }
    }
}
