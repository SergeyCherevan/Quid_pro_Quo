﻿using System.Collections.Generic;
using System.Threading.Tasks;

using Quid_pro_Quo.Database.Ralational;

namespace Quid_pro_Quo.Repositories.Interfaces
{
    public interface IPostRepository : IBaseRepository<PostEntity, int>
    {
        Task<IEnumerable<PostEntity>> GetByAuthorId(int id);
        Task<IEnumerable<PostEntity>> GetByFilter(string keywords, int pageNumber, int pageSize);
    }
}
