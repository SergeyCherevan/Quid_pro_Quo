using System.Threading.Tasks;

using Quid_pro_Quo.Database.Ralational;
using Quid_pro_Quo.DTOs;
using Quid_pro_Quo.Repositories.Interfaces;
using Quid_pro_Quo.WebApiModels;

namespace Quid_pro_Quo.Mappings
{
    public static class UserMapping
    {
        public static UserApiModel ToUserApiModel(this UserEntity entity)
            => new UserApiModel()
                {
                    Id = entity.Id,
                    UserName = entity.UserName,
                    AvatarFileName = entity.AvatarFileName,
                    Biographi = entity.Biographi,
                    Role = entity.Role,
                };

        public static AccountFormDTO ToAccountFormDTO(this AccountFormApiModel model, string oldUserName)
            => new AccountFormDTO()
                {
                    OldUserName = oldUserName,
                    NewUserName = model.UserName,
                    AvatarFile = model.AvatarFile,
                    Biographi = model.Biographi,
                };
    }
}
