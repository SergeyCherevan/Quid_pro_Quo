using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quid_pro_Quo.Database.Ralational
{
    [Table("IoTs")]
    [Index(nameof(OwnerId), IsUnique = true)]
    public class IoTEntity : BaseEntity<int>
    {
        public int IoTCode { get; set; }
        public int OwnerId { get; set; }
        public string HashPassword { get; set; }

        [ForeignKey("OwnerId")]
        public virtual UserEntity Owner { get; set; }
    }
}
