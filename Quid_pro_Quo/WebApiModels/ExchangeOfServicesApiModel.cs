using System;

namespace Quid_pro_Quo.WebApiModels
{
    public class ExchangeOfServicesApiModel
    {
        public int RequestingPostId { get; set; }
        public int RequestedPostId { get; set; }
        public int DateNumberOfRequestedPost { get; set; }
        public int DateNumberOfRequestingPost { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
    }
}
