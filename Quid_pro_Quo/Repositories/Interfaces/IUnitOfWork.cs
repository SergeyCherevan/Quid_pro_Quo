using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Quid_pro_Quo.Repositories;

namespace Quid_pro_Quo.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IPostRepository PostRepository { get; }
        IExchangeOfServicesRepository ExchangeOfServicesRepository { get; }
        IComplaintRepository ComplaintRepository { get; }
        IMessagingRepository MessagingRepository { get; }
        ICommentRepository CommentRepository { get; }

        Task<int> SaveChanges();
    }
}
