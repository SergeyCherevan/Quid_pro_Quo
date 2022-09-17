using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

using Quid_pro_Quo.Authentification;
using Quid_pro_Quo.Database.Ralational;
using Quid_pro_Quo.DTOs;
using Quid_pro_Quo.Exceptions;
using Quid_pro_Quo.Mappings;
using Quid_pro_Quo.Repositories;
using Quid_pro_Quo.Repositories.Interfaces;
using Quid_pro_Quo.Services.Interfaces;
using Quid_pro_Quo.WebApiModels;

namespace Quid_pro_Quo.Services
{
    public class IoTService : IIoTService
    {
        protected IUnitOfWork _UoW { get; set; }
        public IoTService(IUnitOfWork uow)
        {
            _UoW = uow;
        }

        public async Task<JwtApiModel> Login(int iotCode, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException($"{nameof(password)} not specified");
            }

            IoTEntity IoT = await _UoW.IoTRepository.GetByCode(iotCode);
            if (IoT is null)
            {
                throw new NotFoundAppException($"IoT not found");
            }

            string inputHashPass = HashPassword(password);
            if (IoT.HashPassword != inputHashPass)
            {
                throw new NotFoundAppException($"uncorrect password");
            }

            UserEntity owner = await _UoW.UserRepository.GetById(IoT.OwnerId);

            ClaimsIdentity claims = GetIdentity(iotCode, owner.UserName);
            string jwtString = JwtTokenizer.GetEncodedJWT(claims, AuthOptions.Lifetime);

            return new JwtApiModel(jwtString);
        }

        public Task<JwtApiModel> AttachToUser(int iotCode)
        {
            throw new NotImplementedException();
        }

        

        protected static string HashPassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(password);
            var hashPass = sha.ComputeHash(asByteArray);

            return Convert.ToBase64String(hashPass);
        }

        protected ClaimsIdentity GetIdentity(int iotCode, string ownerName)
        {
            var claims = new List<Claim>
            {
                new Claim("IoTCode", iotCode + ""),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "ArduinoUnit"),
                new Claim("OwnerName", ownerName)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}
