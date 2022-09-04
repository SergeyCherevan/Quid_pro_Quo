using System;

using Quid_pro_Quo.Database.Ralational;

namespace Quid_pro_Quo.WebApiModels
{
    public class ExchangeOfServicesApiModel
    {
        public int Id { get; set; }
        public int RequestingPostId { get; set; }
        public int RequestedPostId { get; set; }
        public int DateNumberOfRequestingPost { get; set; }
        public int DateNumberOfRequestedPost { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }

        public StatusEnum ProposalStatus { get; set; }
        public StatusEnum DoneStatus1 { get; set; }
        public StatusEnum DoneStatus2 { get; set; }
        public bool NotViewed { get; set; }
    }
}
