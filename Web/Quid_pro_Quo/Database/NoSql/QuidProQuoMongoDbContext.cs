using MongoDB.Driver;

namespace Quid_pro_Quo.Database.NoSql
{
    public class QuidProQuoMongoDbContext : IQuidProQuoMongoDbContext
    {
        string _connectionString;
        public IMongoDatabase Database { get; private set; }

        public IMongoCollection<MessagingEntity> Messagings
        {
            get => Database.GetCollection<MessagingEntity>("messagings");
        }
        public IMongoCollection<CommentLineEntity> CommentLines
        {
            get => Database.GetCollection<CommentLineEntity>("commentLines");
        }

        public QuidProQuoMongoDbContext(string connectionString, string databaseName)
        {
            _connectionString = connectionString;
            Database = new MongoClient(_connectionString).GetDatabase(databaseName);
        }

        public async void CreateIndexes()
        {
            await Messagings.Indexes
                .CreateOneAsync(new CreateIndexModel<MessagingEntity>(Builders<MessagingEntity>.IndexKeys.Ascending(_ => _.User1Id)))
                .ConfigureAwait(false);

            await Messagings.Indexes
                .CreateOneAsync(new CreateIndexModel<MessagingEntity>(Builders<MessagingEntity>.IndexKeys.Ascending(_ => _.User2Id)))
                .ConfigureAwait(false);

            await CommentLines.Indexes
                .CreateOneAsync(new CreateIndexModel<CommentLineEntity>(Builders<CommentLineEntity>.IndexKeys.Ascending(_ => _.CommentedContentId)))
                .ConfigureAwait(false);

            await CommentLines.Indexes
                .CreateOneAsync(new CreateIndexModel<CommentLineEntity>(Builders<CommentLineEntity>.IndexKeys.Ascending(_ => _.CommentType)))
                .ConfigureAwait(false);
        }
    }
}
