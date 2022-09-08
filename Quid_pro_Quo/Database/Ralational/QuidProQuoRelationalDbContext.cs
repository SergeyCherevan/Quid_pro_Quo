using Microsoft.EntityFrameworkCore;

namespace Quid_pro_Quo.Database.Ralational
{
    public class QuidProQuoRelationalDbContext : DbContext
    {
        public QuidProQuoRelationalDbContext(DbContextOptions<QuidProQuoRelationalDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<LoginInfoEntity> LoginInfos { get; set; }
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<ExchangeOfServicesEntity> ExchangesOfServicess { get; set; }
        public DbSet<ComplaintEntity> Complaints { get; set; }
        public DbSet<IoTEntity> IoTs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ScalarReturn<int>>().HasNoKey().ToView(null);
        }
    }
}
