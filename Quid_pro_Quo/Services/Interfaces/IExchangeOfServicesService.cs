﻿using System.Collections.Generic;
using System.Threading.Tasks;

using Quid_pro_Quo.Database.Ralational;
using Quid_pro_Quo.WebApiModels;

namespace Quid_pro_Quo.Services.Interfaces
{
    public interface IExchangeOfServicesService
    {
        Task<IEnumerable<ExchangeOfServicesApiModel>> GetBySender(string senderName, StatusEnum proposalStatus);
        Task<IEnumerable<ExchangeOfServicesApiModel>> GetByDestination(string destinationName, StatusEnum proposalStatus);
        Task<IEnumerable<ExchangeOfServicesApiModel>> GetConfirmed(string userName, StatusEnum doneStatus);

        Task<ExchangeOfServicesApiModel> SendProposal(SendProposalToExchangeApiModel model);
    }
}
