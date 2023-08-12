using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Persistence.Contexts
{
    public class ECommerceDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public ECommerceDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Domain.Entities.File> Files { get; set; }
        public DbSet<ProductImageFile> ProductImageFiles { get; set; }
        public DbSet<InvoiceFile> InvoiceFiles { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<CompletedOrder> CompletedOrders { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Endpoint> Endpoints { get; set; }

        //Todo: 45.07

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var datas = ChangeTracker.Entries<BaseEntity>();

            foreach (var data in datas)
            {
                _ = data.State switch
                {
                    EntityState.Added => data.Entity.CreatedDate = DateTime.UtcNow,
                    EntityState.Modified => data.Entity.UpdatedDate = DateTime.UtcNow,
                    _ => DateTime.UtcNow
                };
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<AppUser>(x =>
                x.Property(p => p.Id)
                    .HasColumnType("uuid")
                    .HasDefaultValueSql("uuid_generate_v4()"));

            builder.Entity<AppRole>(x =>
                x.Property(p => p.Id)
                    .HasColumnType("uuid")
                    .HasDefaultValueSql("uuid_generate_v4()"));

            builder.Entity<Order>().HasKey(x => x.Id);

            builder.Entity<Order>()
                .HasOne(x => x.Basket)
                .WithOne(x => x.Order)
                .HasForeignKey<Order>(x => x.BasketId);

            builder.Entity<Order>()
                .HasIndex(x => x.OrderCode)
                .IsUnique();

            builder.Entity<Order>()
                .HasOne(x => x.CompletedOrder)
                .WithOne(c => c.Order)
                .HasForeignKey<CompletedOrder>(c => c.OrderId);

            base.OnModelCreating(builder);
        }
    }
}
