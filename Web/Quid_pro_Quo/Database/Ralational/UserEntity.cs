using System.ComponentModel.DataAnnotations.Schema;

namespace Quid_pro_Quo.Database.Ralational
{
    [Table("Users")]
    public class UserEntity : BaseEntity<int>
    {
        public string UserName { get; set; }
        public string AvatarFileName { get; set; }
        public string Biographi { get; set; }
        public string Role { get; set; }

        [ForeignKey("Id")]
        public virtual LoginInfoEntity LoginInfo { get; set; }
    }
}
