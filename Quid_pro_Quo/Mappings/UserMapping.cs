using Quid_pro_Quo.Database.Ralational;
using Quid_pro_Quo.WebApiModels;

namespace Quid_pro_Quo.Mappings
{
    public class UserMapping
    {
        public static UserApiModel ToUserAM(UserEntity entity) => new UserApiModel()
        {
            Id = entity.Id,
            UserName = entity.UserName,
            AvatarFileName = entity.AvatarFileName,
            Biographi = entity.Biographi,
            Role = entity.Role,
        };
    }
}
