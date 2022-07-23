using System.Threading.Tasks;

using Quid_pro_Quo.DTOs;
using Quid_pro_Quo.WebApiModels;

namespace Quid_pro_Quo.Services.Interfaces
{
    public interface IPostService
    {
        Task<PostGetApiModel> Publish(PostFormDTO post);
        Task<PostGetApiModel> GetById(int id);
        Task<PostsPageApiModel> GetByFilter(GetPostByFilterApiModel model);
    }
}
