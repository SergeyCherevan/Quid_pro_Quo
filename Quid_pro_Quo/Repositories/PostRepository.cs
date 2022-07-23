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
        public async Task<IEnumerable<PostEntity>> GetByFilter(string keywords, string date, string geomarker, int pageNumber, int pageSize)
        {
            await Task.Run(() => { });

            List<string> conditions = new List<string>();
            string limits = "";

            if (keywords != "")
            {
                conditions.Add(String.Join(" AND ", keywords.Split(" ").Select(e =>
                    $"(    LOWER([Title]) LIKE LOWER(\"%{e}%\")    OR    LOWER([Text]) LIKE LOWER(\"%{e}%\")    OR    LOWER(U.[UserName]) LIKE LOWER(\"%{e}%\")    )")));
            }

            if (date != "")
            {
                conditions.Add(
                    $"(    LOWER([PerformServiceOnDatesList]) LIKE LOWER(\"%{date}%\")    OR    LOWER([PostedAt]) LIKE LOWER(\"%{date}%\")    )"
                );
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

            if (pageSize != 0)
            {
                limits = $"LIMIT {pageSize} OFFSET {pageNumber * pageSize}";
            }

            string sqlQuery = $"SELECT P.* FROM [Posts] AS P INNER JOIN [Users] AS U ON P.[AuthorId] = U.[Id] {String.Join(" AND ", conditions)} ORDER BY [PostedAt] DESC {limits}";

            return _db.Posts.FromSqlRaw(sqlQuery);
        }
        public async Task<int> GetCountByFilter(string keywords, string date, string geomarker)
        {
            await Task.Run(() => { });

            List<string> conditions = new List<string>();

            if (keywords != "")
            {
                conditions.Add(String.Join(" AND ", keywords.Split(" ").Select(e =>
                    $"(    LOWER([Title]) LIKE LOWER(\"%{e}%\")    OR    LOWER([Text]) LIKE LOWER(\"%{e}%\")    )")));
            }

            if (date != "")
            {
                conditions.Add(
                    $"(    LOWER([PerformServiceOnDatesList]) LIKE LOWER(\"%{date}%\")    OR    LOWER([PostedAt]) LIKE LOWER(\"%{date}%\")    )"
                );
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
