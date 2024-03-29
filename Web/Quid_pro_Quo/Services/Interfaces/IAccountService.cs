﻿using System.Threading.Tasks;
using Quid_pro_Quo.DTOs;
using Quid_pro_Quo.WebApiModels;

namespace Quid_pro_Quo.Services.Interfaces
{
    public interface IAccountService
    {
        Task<JwtApiModel> Login(string userName, string password);
        Task<UserApiModel> Registration(string userName, string password);
        Task ChangePassword(string userName, string oldPassword, string newPassword);
        Task<UserApiModel> Edit(AccountFormDTO account);
    }
}
