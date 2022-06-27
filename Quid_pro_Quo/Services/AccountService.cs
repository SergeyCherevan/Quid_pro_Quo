using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

using Quid_pro_Quo.Authentification;
using Quid_pro_Quo.Database.Ralational;
using Quid_pro_Quo.Exceptions;
using Quid_pro_Quo.Mappings;
using Quid_pro_Quo.Repositories.Interfaces;
using Quid_pro_Quo.Services.Interfaces;
using Quid_pro_Quo.WebApiModels;

namespace Quid_pro_Quo.Services
{
    public class AccountService : IAccountService
    {
        protected IUnitOfWork _UoW { get; set; }
        public AccountService(IUnitOfWork uow)
        {
            _UoW = uow;
        }

        public async Task<string> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException($"{nameof(username)} not specified");
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException($"{nameof(password)} not specified");
            }

            UserEntity user = await _UoW.UserRepository.GetByName(username);
            if (user is null)
            {
                throw new NotFoundAppException($"user not found");
            }

            string userHashPass = await _UoW.UserRepository.GetHashPasswordById(user.Id);
            if (userHashPass != HashPassword(password))
            {
                throw new NotFoundAppException($"uncorrect password");
            }

            ClaimsIdentity claims = GetIdentity(user.UserName, user.Role);
            string jwtString = JwtTokenizer.GetEncodedJWT(claims, AuthOptions.Lifetime);

            return jwtString;
        }
        public async Task<UserApiModel> Registration(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException($"{nameof(username)} not specified");
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException($"{nameof(password)} not specified");
            }

            UserEntity user = await _UoW.UserRepository.GetByName(username);
            if (user is not null)
            {
                throw new AlreadyExistAppException($"user already exist");
            }

            user = new UserEntity()
            {
                UserName = username,
                AvatarFileName = null,
                Biographi = "",
                Role = "User",
            };
            user = await _UoW.UserRepository.Add(user);

            return UserMapping.ToUserAM(user);
        }

        protected static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        private ClaimsIdentity GetIdentity(string username, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, username),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}
