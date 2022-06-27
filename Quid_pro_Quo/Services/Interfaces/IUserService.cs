using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Quid_pro_Quo.WebApiModels;

namespace Quid_pro_Quo.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserApiModel> GetUserByName(string username);
        Task<UsersPageApiModel> GetUsersByFilter(string keywords, int pageNumber, int pageSize);
    }
}
