﻿using System.Threading.Tasks;
using Quid_pro_Quo.DTOs;
using Quid_pro_Quo.WebApiModels;

namespace Quid_pro_Quo.Services.Interfaces
{
    public interface IIoTService
    {
        Task<JwtApiModel> Login(int iotCode, string password);
        Task<int> GetUserIoTCode(string ownerName);
        Task AddRequestToAttach(string ownerName, int iotCode);
        Task DeleteRequestToAttach(int iotCode);
        Task<JwtApiModel> AttachToUser(int iotCode);
        Task DetachIoTFromUser(string ownerName);
    }
}
