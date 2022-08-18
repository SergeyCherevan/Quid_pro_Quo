using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quid_pro_Quo.Database.Ralational
{
    [Table("ExchangesOfServices")]
    public class ExchangeOfServicesEntity : BaseEntity<int>
    {
        public int RequestingPostId { get; set; }
        public int RequestedPostId { get; set; }
        public int DateNumberOfRequestedPost { get; set; }
        public int DateNumberOfRequestingPost { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }

        public StatusEnum ProposalStatus { get; set; }
        public StatusEnum DoneStatus1 { get; set; }
        public StatusEnum DoneStatus2 { get; set; }
        public bool NotViewed { get; set; }

        [ForeignKey("RequestingPostId")]
        public virtual PostEntity RequestingPost { get; set; }
        [ForeignKey("RequestedPostId")]
        public virtual PostEntity RequestedPost { get; set; }
    }

    public enum StatusEnum
    {
        NoInfo = 0,
        Yes = 1,
        No = 2,
    }
}
