using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Quid_pro_Quo.Database.Ralational;
using Quid_pro_Quo.WebApiModels;

namespace Quid_pro_Quo.Mappings
{
    public static class ExchangeOfServicesMapping
    {
        public static ExchangeOfServicesEntity ToExchangeOfServicesEntity
            (this SendProposalToExchangeApiModel model, DateTime time)
            => new ExchangeOfServicesEntity()
            {
                Id = 0,

                RequestingPostId = model.RequestingPostId,
                RequestedPostId = model.RequestedPostId,
                DateNumberOfRequestingPost = model.DateNumberOfRequestingPost,
                DateNumberOfRequestedPost = model.DateNumberOfRequestedPost,

                Text = model.Text,
                Time = time,

                ProposalStatus = StatusEnum.NoInfo,
                DoneStatus1 = StatusEnum.NoInfo,
                DoneStatus2 = StatusEnum.NoInfo,
                NotViewed = true,
            };

        public static ExchangeOfServicesApiModel ToExchangeOfServicesApiModel
            (this ExchangeOfServicesEntity entity)
            => new ExchangeOfServicesApiModel()
            {
                Id = 0,

                RequestingPostId = entity.RequestingPostId,
                RequestedPostId = entity.RequestedPostId,
                DateNumberOfRequestingPost = entity.DateNumberOfRequestingPost,
                DateNumberOfRequestedPost = entity.DateNumberOfRequestedPost,

                Text = entity.Text,
                Time = entity.Time,

                ProposalStatus = entity.ProposalStatus,
                DoneStatus1 = entity.DoneStatus1,
                DoneStatus2 = entity.DoneStatus2,
                NotViewed = entity.NotViewed,
            };
    }
}
