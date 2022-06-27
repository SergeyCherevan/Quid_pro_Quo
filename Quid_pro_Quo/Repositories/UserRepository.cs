using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Quid_pro_Quo.Repositories.Interfaces;
using Quid_pro_Quo.Database.Ralational;

namespace Quid_pro_Quo.Repositories
{
    public class UserRepository : BaseRelationalRepository<UserEntity, int>, IUserRepository
    {
        protected BaseRelationalRepository<LoginInfoEntity, int> _loginInfoRepository;
        public UserRepository(QuidProQuoRelationalDbContext db) : base(db)
        {
            _loginInfoRepository = new BaseRelationalRepository<LoginInfoEntity, int>(db);
        }



        public async Task<UserEntity> GetByName(string username)
            => await _db.Users.Include(u => u.LoginInfo).FirstOrDefaultAsync(u => u.UserName == username);
        public async Task<string> GetHashPasswordById(int id)
            => (await _loginInfoRepository.GetById(id)).HashPassword;
        public async Task<IEnumerable<UserEntity>> GetByFilter(string keywords, int pageNumber, int pageSize)
        {
            await Task.Run(() => { });

            string condition =
                keywords != "" ?
                    "WHERE " + String.Join(" AND ", keywords.Split(" ").Select(e =>
                    $"(    LOWER([UserName]) LIKE LOWER(\"%{e}%\")    OR    LOWER([Biographi]) LIKE LOWER(\"%{e}%\")    )"))
                :
                    "";

            string sqlQuery = $"SELECT * FROM [Users] {condition} LIMIT {pageSize} OFFSET {pageNumber * pageSize}";

            return _db.Users.FromSqlRaw(sqlQuery);
        }
        public async Task<int> GetCountByFilter(string keywords)
        {
            await Task.Run(() => { });

            string condition =
                keywords != "" ?
                    "WHERE " + String.Join(" AND ", keywords.Split(" ").Select(e =>
                    $"(    LOWER([UserName]) LIKE LOWER(\"%{e}%\")    OR    LOWER([Biographi]) LIKE LOWER(\"%{e}%\")    )"))
                :
                    "";

            string sqlQuery = $"SELECT * FROM [Users] {condition}";

            return _db.Set<ScalarReturn<int>>()
                .FromSqlRaw(sqlQuery)
                .AsEnumerable()
                .First().Value;
        }

        public async new Task<UserEntity> Add(UserEntity user)
        {
            user = (await _db.Users.AddAsync(user)).Entity;
            user.LoginInfo.Id = user.Id;
            await _db.LoginInfos.AddAsync(user.LoginInfo);

            return user;
        }
    }
}
