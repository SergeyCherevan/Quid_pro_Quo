using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Quid_pro_Quo.Database.Ralational;
using Quid_pro_Quo.Mappings;
using Quid_pro_Quo.Repositories.Interfaces;
using Quid_pro_Quo.Services.Interfaces;
using Quid_pro_Quo.WebApiModels;

namespace Quid_pro_Quo.Services
{
    public class ExchangeOfServicesService : IExchangeOfServicesService
    {
        protected IUnitOfWork _UoW { get; set; }
        public ExchangeOfServicesService(IUnitOfWork uow)
        {
            _UoW = uow;
        }

        public async Task<IEnumerable<ExchangeOfServicesApiModel>> GetBySender(string senderName, StatusEnum proposalStatus)
        {
            UserEntity sender = await _UoW.UserRepository.GetByName(senderName);

            return (await _UoW.ExchangeOfServicesRepository.GetBySender(sender.Id, proposalStatus))
                .Select(e => e.ToExchangeOfServicesApiModel());
        }

        public async Task<IEnumerable<ExchangeOfServicesApiModel>> GetByDestination
            (string destinationName, StatusEnum proposalStatus)
        {
            UserEntity destination = await _UoW.UserRepository.GetByName(destinationName);

            return (await _UoW.ExchangeOfServicesRepository.GetByDestination(destination.Id, proposalStatus))
                .Select(e => e.ToExchangeOfServicesApiModel());
        }

        public async Task<IEnumerable<ExchangeOfServicesApiModel>> GetConfirmed(string userName, StatusEnum doneStatus)
        {
            UserEntity user = await _UoW.UserRepository.GetByName(userName);

            return (await _UoW.ExchangeOfServicesRepository.GetConfirmed(user.Id, doneStatus))
                .Select(e => e.ToExchangeOfServicesApiModel());
        }



        public async Task<ExchangeOfServicesApiModel> SendProposal(SendProposalToExchangeApiModel model)
        {
            ExchangeOfServicesEntity entity
                = await _UoW.ExchangeOfServicesRepository.Add(model.ToExchangeOfServicesEntity(DateTime.Now));
            ExchangeOfServicesApiModel returnedModel = entity.ToExchangeOfServicesApiModel();

            await _UoW.SaveChanges();

            return returnedModel;
        }

        public async Task<ExchangeOfServicesApiModel> ConfirmProposal(int id)
        {
            ExchangeOfServicesEntity entity = await _UoW.ExchangeOfServicesRepository.GetById(id);
            entity.ProposalStatus = StatusEnum.Yes;
            entity = await _UoW.ExchangeOfServicesRepository.Update(entity);
            await _UoW.SaveChanges();

            ExchangeOfServicesApiModel returnedModel = entity.ToExchangeOfServicesApiModel();

            return returnedModel;
        }

        public async Task<ExchangeOfServicesApiModel> CancelProposal(int id)
        {
            ExchangeOfServicesEntity entity = await _UoW.ExchangeOfServicesRepository.GetById(id);
            entity.ProposalStatus = StatusEnum.No;
            entity = await _UoW.ExchangeOfServicesRepository.Update(entity);
            await _UoW.SaveChanges();

            ExchangeOfServicesApiModel returnedModel = entity.ToExchangeOfServicesApiModel();

            return returnedModel;
        }

        public async Task<ExchangeOfServicesApiModel> CancelExchange(int exchangeId, int postId)
        {
            ExchangeOfServicesEntity entity = await _UoW.ExchangeOfServicesRepository.GetById(exchangeId);

            if (entity.RequestingPostId == postId)
            {
                entity.DoneStatus1 = StatusEnum.No;
            }
            if (entity.RequestedPostId == postId)
            {
                entity.DoneStatus2 = StatusEnum.No;
            }

            entity = await _UoW.ExchangeOfServicesRepository.Update(entity);
            await _UoW.SaveChanges();

            ExchangeOfServicesApiModel returnedModel = entity.ToExchangeOfServicesApiModel();

            return returnedModel;
        }
    }
}
