using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Quid_pro_Quo.Database.NoSql;

namespace Quid_pro_Quo.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<CommentLineEntity> GetById(int commentedContentId, CommentEnum commentType);

        Task<IQueryable<CommentLineEntity>> GetByPredicate(Expression<Func<CommentLineEntity, bool>> predicate);

        Task<CommentLineEntity> Add(int commentedContentId, CommentEnum commentType, MessageEntity message);

        Task<CommentLineEntity> Delete(int commentedContentId, CommentEnum commentType);
    }
}
