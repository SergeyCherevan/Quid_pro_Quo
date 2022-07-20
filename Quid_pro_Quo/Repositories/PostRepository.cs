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
                conditions.Add("WHERE " + String.Join(" AND ", keywords.Split(" ").Select(e =>
                    $"(    LOWER([Title]) LIKE LOWER(\"%{e}%\")    OR    LOWER([Text]) LIKE LOWER(\"%{e}%\")    )")));
            }

            string sqlQuery = $"SELECT * FROM [Posts] {String.Join(" AND ", conditions)} LIMIT {pageSize} OFFSET {pageNumber * pageSize}";

            IEnumerable<PostEntity> returnedPosts = _db.Posts.FromSqlRaw(sqlQuery);



            if (geomarker != "")
            {
                Console.WriteLine();
                Console.WriteLine("lat: " + geomarker.Split(",")[0]);
                Console.WriteLine("lng: " + geomarker.Split(",")[1]);

                double lat = Double.Parse(geomarker.Split(",")[0], CultureInfo.InvariantCulture),
                       lng = Double.Parse(geomarker.Split(",")[1], CultureInfo.InvariantCulture);

                returnedPosts = returnedPosts.Where(post =>
                {
                    Console.WriteLine("pLat: " + post.PerformServiceInPlace.Replace("@", "").Split(",")[0]);
                    Console.WriteLine("pLng: " + post.PerformServiceInPlace.Replace("@", "").Split(",")[1]);

                    double pLat = Double.Parse(
                        post.PerformServiceInPlace.Replace("@", "").Split(",")[0],
                        CultureInfo.InvariantCulture
                    );
                    double pLng = Double.Parse(
                        post.PerformServiceInPlace.Replace("@", "").Split(",")[1],
                        CultureInfo.InvariantCulture
                    );

                    return Math.Sqrt((lat - pLat) * (lat - pLat) + (lng - pLng) * (lng - pLng)) <= 1;
                });
            }

            return returnedPosts;
        }
        public async Task<int> GetCountByFilter(string keywords, string geomarker)
        {
            await Task.Run(() => { });

            List<string> conditions = new List<string>();

            if (keywords != "")
            {
                conditions.Add("WHERE " + String.Join(" AND ", keywords.Split(" ").Select(e =>
                    $"(    LOWER([Title]) LIKE LOWER(\"%{e}%\")    OR    LOWER([Text]) LIKE LOWER(\"%{e}%\")    )")));
            }

            string sqlQuery = $"SELECT COUNT(*) AS Value FROM [Posts] {String.Join(" AND ", conditions)}";

            int result = _db.Set<ScalarReturn<int>>()
                .FromSqlRaw(sqlQuery)
                .AsEnumerable()
                .First().Value;



            //if (geomarker != "")
            //{
            //    double lat = Double.Parse(geomarker.Split(",")[0]),
            //           lng = Double.Parse(geomarker.Split(",")[1]);

            //    result -= returnedPosts.Sum(post =>
            //    {
            //        double pLat = Double.Parse(
            //            post.PerformServiceInPlace.Substring(0, post.PerformServiceInPlace.IndexOf(",") - 1)
            //        );
            //        double pLng = Double.Parse(
            //            post.PerformServiceInPlace.Substring(post.PerformServiceInPlace.IndexOf(",") + 1, post.PerformServiceInPlace.LastIndexOf(","))
            //        );

            //        return Math.Sqrt((lat - pLat) * (lat - pLat) + (lng - pLng) * (lng - pLng)) <= 1;
            //    });
            //}

            return result;
        }
    }
}
