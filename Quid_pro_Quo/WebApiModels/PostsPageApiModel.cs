using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quid_pro_Quo.WebApiModels
{
    public class PostsPageApiModel
    {
        public IEnumerable<PostGetApiModel> Posts { get; set; }
        public int PostsCount { get; set; }
    }
}
