using Quid_pro_Quo.Database.Ralational;

namespace Quid_pro_Quo.WebApiModels
{
    public class ComplaintApiModel
    {
        public ComplaintEnum ComplaintType { get; set; }
        public int ComplaintedContentId { get; set; }
        public string Text { get; set; }
        public string AuthorName { get; set; }
    }
}
