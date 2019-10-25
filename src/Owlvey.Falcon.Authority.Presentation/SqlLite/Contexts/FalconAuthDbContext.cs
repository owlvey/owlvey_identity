using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Owvley.Falcon.Authority.Domain.Models;

namespace Owlvey.Falcon.Authority.Infra.Data.Sqlite.Contexts
{
    public class FalconAuthDbContext : IdentityDbContext<User>
    {
        public FalconAuthDbContext(DbContextOptions<FalconAuthDbContext> options) :
           base(options)
        {

        }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
