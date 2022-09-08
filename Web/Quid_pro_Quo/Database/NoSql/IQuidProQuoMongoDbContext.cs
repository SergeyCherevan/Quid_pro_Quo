using MongoDB.Driver;

using Microsoft.Extensions.Configuration;

namespace Quid_pro_Quo.Database.NoSql
{
    public interface IQuidProQuoMongoDbContext
    {
        public IMongoDatabase Database { get; }

        public IMongoCollection<MessagingEntity> Messagings { get; }
        public IMongoCollection<CommentLineEntity> CommentLines { get; }

        public void CreateIndexes();
    }
}
