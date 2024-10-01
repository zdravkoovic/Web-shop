using Infrastructure.SQL.Database.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebShop.src;
using WebShop.src.Infrastructure.SQL.Database.Model;

namespace Infrastructure.SQL.Database
{
    public class Context(IConfiguration configuration) : IdentityDbContext<ApiUser>
    {
        public IConfiguration Configuration { get; set; } = configuration;

        public DbSet<ApiUser> Accounts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ProductImage> Images { get; set; }
        public DbSet<AvatarImage> Avatars { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
          =>  options.UseNpgsql(Configuration.GetConnectionString("WebShopDatabase"));
        //   .AddInterceptors(new DbCIntercept(new LoggerFactory().CreateLogger<DbCIntercept>()))
        //   .EnableSensitiveDataLogging()
        //   .UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddConsole(); }));
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChatMessage>()
            .HasIndex(c => c.Timestamp);

            modelBuilder.Entity<ChatMessage>()
            .HasIndex(c => c.SenderId);

            modelBuilder.Entity<ChatMessage>()
            .HasIndex(c => c.RecipientId);

            modelBuilder.Entity<ChatMessage>()
            .HasOne<ApiUser>()
            .WithMany()
            .HasForeignKey(c => c.SenderId)
            .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<ChatMessage>()
            .HasOne<ApiUser>()
            .WithMany()
            .HasForeignKey(c => c.RecipientId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
            .HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
            .HasOne(e => e.Product)
            .WithMany(e => e.Comments)
            .HasForeignKey(c => c.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AvatarImage>()
            .HasOne<ApiUser>()
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}