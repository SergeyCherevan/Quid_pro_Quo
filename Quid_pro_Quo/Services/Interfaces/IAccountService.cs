using System.Threading.Tasks;

using Quid_pro_Quo.WebApiModels;

namespace Quid_pro_Quo.Services.Interfaces
{
    public interface IAccountService
    {
        Task<string> Login(string username, string password);
        Task<UserApiModel> Registration(string username, string password);
    }
}
