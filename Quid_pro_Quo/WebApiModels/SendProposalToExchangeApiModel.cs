namespace Quid_pro_Quo.WebApiModels
{
    public class SendProposalToExchangeApiModel
    {
        public int RequestingPostId { get; set; }
        public int RequestedPostId { get; set; }
        public int DateNumberOfRequestedPost { get; set; }
        public int DateNumberOfRequestingPost { get; set; }
        public string Text { get; set; }
    }
}
