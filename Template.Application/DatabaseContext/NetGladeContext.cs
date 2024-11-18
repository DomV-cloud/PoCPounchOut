using Microsoft.EntityFrameworkCore;
using Template.Domain.Entities;

namespace Template.Application.DatabaseContext
{
    public class TemplateContext : DbContext
    {
        public TemplateContext(DbContextOptions<TemplateContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => new { oi.OrderId, oi.ItemId });

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Item)
                .WithMany(i => i.OrderItems)
                .HasForeignKey(oi => oi.ItemId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>().HasData(
               new User 
               { 
                   Id = Guid.NewGuid(), 
                   SecreteID = "123456789",
                   FirstName = "John", 
                   LastName = "Doe",
                   Email = "random@email",
                   Password = "ŠelDědečekNaKopeček" 
               }
             );

            // Seed pro entitu Item
            modelBuilder.Entity<Item>().HasData(
                new Item
                {
                    Id = 1,
                    ItemName = "Laptop",
                    NumberOfItems = 10,
                    Price = 1500.00m
                },
                new Item
                {
                    Id = 2,
                    ItemName = "Smartphone",
                    NumberOfItems = 20,
                    Price = 800.00m
                },
                new Item
                {
                    Id = 3,
                    ItemName = "Headphones",
                    NumberOfItems = 50,
                    Price = 150.00m
                }
            );
        }
    }
}
