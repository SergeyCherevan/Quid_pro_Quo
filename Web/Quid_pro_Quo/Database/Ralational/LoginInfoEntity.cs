using System.ComponentModel.DataAnnotations.Schema;

namespace Quid_pro_Quo.Database.Ralational
{
    [Table("LoginInfos")]
    public class LoginInfoEntity : BaseEntity<int>
    {
        public string HashPassword { get; set; }
    }
}
