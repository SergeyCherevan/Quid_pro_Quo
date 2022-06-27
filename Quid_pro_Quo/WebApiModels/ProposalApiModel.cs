using System;

namespace Quid_pro_Quo.WebApiModels
{
    public class ProposalApiModel
    {
        public int RequestingPost { get; set; }
        public int RequestedPost { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
    }
}
