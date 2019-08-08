using Owlvey.Falcon.Authority.Infra.Data.SqlServer.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Owvley.Falcon.Authority.Domain.Models;
using TheRoostt.Authority.Domain.Models;

namespace Owlvey.Falcon.Authority.Infra.Data.SqlServer.Contexts
{
    public class FalconAuthDbContext : IdentityDbContext<User>
    {
        public FalconAuthDbContext(DbContextOptions<FalconAuthDbContext> options) :
           base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerMember> Members { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
               .HasMany(a => a.Preferences)
               .WithOne(b => b.User)
               .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<CustomerMember>().HasKey(x => new { x.CustomerMemberId });
            
            modelBuilder.Entity<CustomerMember>()
               .HasOne(pt => pt.Customer)
               .WithMany(p => p.Members)
               .OnDelete(DeleteBehavior.Restrict)
               .HasForeignKey(pt => pt.CustomerId);
            
            modelBuilder.Entity<CustomerMember>()
               .HasOne(pt => pt.User)
               .WithMany(p => p.Customers)
               .HasForeignKey(pt => pt.UserId);

            modelBuilder.RemovePluralizingTableNameConvention();
            base.OnModelCreating(modelBuilder);
        }
    }
}
