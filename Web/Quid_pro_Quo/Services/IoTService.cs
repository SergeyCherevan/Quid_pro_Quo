﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

using Quid_pro_Quo.Authentification;
using Quid_pro_Quo.Database.Ralational;
using Quid_pro_Quo.DTOs;
using Quid_pro_Quo.Exceptions;
using Quid_pro_Quo.AttachingRequests;
using Quid_pro_Quo.Mappings;
using Quid_pro_Quo.Repositories;
using Quid_pro_Quo.Repositories.Interfaces;
using Quid_pro_Quo.Services.Interfaces;
using Quid_pro_Quo.WebApiModels;
using Quid_pro_Quo.SignalRHubs;

namespace Quid_pro_Quo.Services
{
    public class IoTService : IIoTService
    {
        protected IUnitOfWork _UoW { get; set; }
        protected IAttachingRequestsQueue _attachingRequestsQueue { get; set; }
        protected IServiceProvider _serviceProvider { get; set; }
        protected IHubContext<IoTHub> _iotContext { get; set; }
        public IoTService(
            IUnitOfWork uow,
            IAttachingRequestsQueue attachingRequestsQueue,
            IServiceProvider serviceProvider,
            IHubContext<IoTHub> iotContext)
        {
            _UoW = uow;
            _attachingRequestsQueue = attachingRequestsQueue;
            _serviceProvider = serviceProvider;
            _iotContext = iotContext;
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

            UserEntity owner;
            if (IoT.OwnerId != null)
            {
                owner = await _UoW.UserRepository.GetById((int)IoT.OwnerId);
            }
            else
            {
                owner = null;
            }

            ClaimsIdentity claims = GetIdentity(iotCode, owner?.UserName ?? "");
            string jwtString = JwtTokenizer.GetEncodedJWT(claims, AuthOptions.Lifetime);

            return new JwtApiModel(jwtString);
        }

        public async Task<int> GetUserIoTCode(string ownerName)
        {
            if (string.IsNullOrEmpty(ownerName))
            {
                throw new ArgumentException($"{nameof(ownerName)} not specified");
            }

            IoTEntity IoT = await _UoW.IoTRepository.GetByOwnerName(ownerName);
            if (IoT is null)
            {
                throw new NotFoundAppException($"IoT not found");
            }

            return IoT.IoTCode;
        }

        public async Task AddRequestToAttach(string ownerName, int iotCode)
        {
            await Task.Run(() => { });

            _attachingRequestsQueue.Add((ownerName, iotCode));

            await AttachingRequestDeleteScheduler.Start(_serviceProvider, iotCode);
        }

        public async Task DeleteRequestToAttach(int iotCode)
        {
            await Task.Run(() => _attachingRequestsQueue.DeleteByCode(iotCode));
        }

        public async Task<JwtApiModel> AttachToUser(int iotCode)
        {
            IoTEntity IoT = await _UoW.IoTRepository.GetByCode(iotCode);
            if (IoT is null)
            {
                throw new NotFoundAppException($"IoT not found");
            }

            var request = _attachingRequestsQueue.GetByCode(iotCode);
            UserEntity owner = await _UoW.UserRepository.GetByName(request.OwnerName);

            IoT.OwnerId = owner.Id;
            await _UoW.IoTRepository.Update(IoT);

            await DeleteRequestToAttach(iotCode);

            await _UoW.SaveChanges();

            await _iotContext.Clients.User(owner.UserName).SendAsync("IoTisAttached");

            ClaimsIdentity claims = GetIdentity(iotCode, owner.UserName);
            string jwtString = JwtTokenizer.GetEncodedJWT(claims, AuthOptions.Lifetime);

            return new JwtApiModel(jwtString);
        }

        public async Task DetachIoTFromUser(string ownerName)
        {
            IoTEntity IoT = await _UoW.IoTRepository.GetByOwnerName(ownerName);
            if (IoT is null)
            {
                throw new NotFoundAppException($"IoT not found");
            }

            IoT.OwnerId = null;
            await _UoW.IoTRepository.Update(IoT);

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
