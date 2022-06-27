using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
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

        public async Task<JwtApiModel> Login(string username, string password)
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

            string inputHashPass = HashPassword(password);
            string userHashPass = await _UoW.UserRepository.GetHashPasswordById(user.Id);
            if (userHashPass != inputHashPass)
            {
                throw new NotFoundAppException($"uncorrect password");
            }

            ClaimsIdentity claims = GetIdentity(user.UserName, user.Role);
            string jwtString = JwtTokenizer.GetEncodedJWT(claims, AuthOptions.Lifetime);

            return new JwtApiModel(jwtString);
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
                LoginInfo = new LoginInfoEntity()
                {
                    HashPassword = HashPassword(password),
                },
            };
            user = await _UoW.UserRepository.Add(user);
            await _UoW.SaveChanges();

            return UserMapping.ToUserAM(user);
        }

        public async Task ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(oldPassword))
            {
                throw new ArgumentException($"{nameof(oldPassword)} not specified");
            }
            if (string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentException($"{nameof(newPassword)} not specified");
            }

            UserEntity user = await _UoW.UserRepository.GetByName(username);
            if (user is null)
            {
                throw new NotFoundAppException($"user not found");
            }

            string inputHashPass = HashPassword(oldPassword);
            string userHashPass = await _UoW.UserRepository.GetHashPasswordById(user.Id);
            if (userHashPass != inputHashPass)
            {
                throw new NotFoundAppException($"uncorrect old password");
            }

            user.LoginInfo.HashPassword = HashPassword(newPassword);
            await _UoW.UserRepository.Update(user);
            await _UoW.SaveChanges();
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

        protected ClaimsIdentity GetIdentity(string username, string role)
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
