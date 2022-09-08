using System.Collections.Generic;

namespace Quid_pro_Quo.WebApiModels
{
    public class PostsPageApiModel
    {
        public IEnumerable<PostGetApiModel> Posts { get; set; }
        public int PostsCount { get; set; }
    }
}
