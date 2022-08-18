using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quid_pro_Quo.Database.Ralational
{
    [Table("ExchangesOfServices")]
    public class ExchangeOfServicesEntity : BaseEntity<int>
    {
        public int RequestingPostId { get; set; }
        public int RequestedPostId { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }

        [ForeignKey("RequestingPostId")]
        public virtual UserEntity RequestingPost { get; set; }
        [ForeignKey("RequestedPostId")]
        public virtual UserEntity RequestedPost { get; set; }
    }
}
