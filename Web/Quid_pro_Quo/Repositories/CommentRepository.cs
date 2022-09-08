using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

using Quid_pro_Quo.Database.NoSql;
using Quid_pro_Quo.Repositories.Interfaces;

namespace Quid_pro_Quo.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        protected IQuidProQuoMongoDbContext _db;
        public CommentRepository(IQuidProQuoMongoDbContext db)
        {
            _db = db;
        }

        public async Task<CommentLineEntity> GetById(int commentedContentId, CommentEnum commentType)
            => (await _db.CommentLines.FindAsync(
                e => e.CommentedContentId == commentedContentId && e.CommentType == commentType
            )).FirstOrDefault();

        public async Task<IQueryable<CommentLineEntity>> GetByPredicate(Expression<Func<CommentLineEntity, bool>> predicate)
            => (IQueryable<CommentLineEntity>)await _db.CommentLines.FindAsync(
                predicate ?? (e => true)
            );

        public async Task<CommentLineEntity> Add(int commentedContentId, CommentEnum commentType, MessageEntity comment)
        {
            CommentLineEntity commentLine;

            if ((commentLine = await GetById(commentedContentId, commentType)) == null)
            {
                commentLine = new CommentLineEntity()
                {
                    CommentedContentId = commentedContentId,
                    CommentType = commentType,
                    CommentsList = new List<MessageEntity>() { comment }
                };

                await _db.CommentLines.InsertOneAsync(commentLine);
            }
            else
            {
                commentLine.CommentsList.Add(comment);
            }

            return commentLine;
        }

        public async Task<CommentLineEntity> Delete(int commentedContentId, CommentEnum commentType)
        {
            var commentLine = await GetById(commentedContentId, commentType);

            _db.CommentLines.DeleteOne(
                e => e.CommentedContentId == commentedContentId && e.CommentType == commentType
            );

            return commentLine;
        }

        public async Task<CommentLineEntity> Delete(int commentedContentId, CommentEnum commentType, int idComment)
        {
            var commentLine = await GetById(commentedContentId, commentType);
            var comment = commentLine.CommentsList.FirstOrDefault(e => e.Id == idComment);

            commentLine.CommentsList.Remove(comment);

            return commentLine;
        }
    }
}
