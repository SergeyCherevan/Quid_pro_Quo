using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Quid_pro_Quo.Database.Ralational;
using Quid_pro_Quo.Database.NoSql;
using Quid_pro_Quo.Repositories.Interfaces;

namespace Quid_pro_Quo.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public IPostRepository PostRepository { get; }
        public IExchangeOfServicesRepository ExchangeOfServicesRepository { get; }
        public IComplaintRepository ComplaintRepository { get; }
        public IMessagingRepository MessagingRepository { get; }
        public ICommentRepository CommentRepository { get; }

        protected QuidProQuoRelationalDbContext _relationalDb { get; set; }
        protected IQuidProQuoMongoDbContext _noSqlDb { get; set; }

        public UnitOfWork(QuidProQuoRelationalDbContext relationalDb, IQuidProQuoMongoDbContext noSqlDb)
        {
            _relationalDb = relationalDb;
            _noSqlDb = noSqlDb;

            UserRepository = new UserRepository(relationalDb);
            PostRepository = new PostRepository(relationalDb);
            ExchangeOfServicesRepository = new ExchangeOfServicesRepository(relationalDb);
            ComplaintRepository = new ComplaintRepository(relationalDb);

            MessagingRepository = new MessagingRepository(_noSqlDb, UserRepository);
            CommentRepository = new CommentRepository(_noSqlDb);
        }

        public async Task<int> SaveChanges() => await _relationalDb.SaveChangesAsync();
    }
}
