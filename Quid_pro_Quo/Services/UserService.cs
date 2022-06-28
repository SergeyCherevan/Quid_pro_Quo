using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Quid_pro_Quo.Database.Ralational;
using Quid_pro_Quo.Exceptions;
using Quid_pro_Quo.Mappings;
using Quid_pro_Quo.Repositories.Interfaces;
using Quid_pro_Quo.Services.Interfaces;
using Quid_pro_Quo.WebApiModels;

namespace Quid_pro_Quo.Services
{
    public class UserService : IUserService
    {
        protected IUnitOfWork _UoW { get; set; }
        public UserService(IUnitOfWork uow)
        {
            _UoW = uow;
        }

        public async Task<UserApiModel> GetUserByName(string username)
        {
            UserEntity user = await _UoW.UserRepository.GetByName(username);
            if (user is null)
            {
                throw new NotFoundAppException($"user not found");
            }

            return user.ToUserApiModel();
        }

        public async Task<UsersPageApiModel> GetUsersByFilter(string keywords, int pageNumber, int pageSize)
            => new UsersPageApiModel()
                {
                    Users = (await _UoW.UserRepository.GetByFilter(keywords, pageNumber, pageSize))
                                .Select(userEntity => userEntity.ToUserApiModel()),
                    UsersCount = await _UoW.UserRepository.GetCountByFilter(keywords),
                };
    }
}
