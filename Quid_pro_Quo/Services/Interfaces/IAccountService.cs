using System.Threading.Tasks;

using Quid_pro_Quo.WebApiModels;

namespace Quid_pro_Quo.Services.Interfaces
{
    public interface IAccountService
    {
        Task<JwtApiModel> Login(string username, string password);
        Task<UserApiModel> Registration(string username, string password);
        Task ChangePassword(string username, string oldPassword, string newPassword);
    }
}
