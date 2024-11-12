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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
               new User 
               { 
                   Id = Guid.NewGuid(), 
                   SecreteID = "550bce3e592023b2e7b015307f965133",
                   FirstName = "John", 
                   LastName = "Doe",
                   Email = "random@email",
                   Password = "ŠelDědečekNaKopeček" 
               }
             );
        }
    }
}
