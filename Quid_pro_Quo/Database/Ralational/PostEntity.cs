using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quid_pro_Quo.Database.Ralational
{
    [Table("Posts")]
    public class PostEntity : BaseEntity<int>
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string ImageFileNames { get; set; }
        public int AuthorId { get; set; }
        public DateTime PostedAt { get; set; }
        public bool IsActual { get; set; }

        public string PerformServiceOnDatesList { get; set; }

        // In Format: @39.6192195,-71.7422487,6.99z
        public string PerformServiceInPlace { get; set; }



        [ForeignKey("AuthorId")]
        public virtual UserEntity Author { get; set; }
    }
}
