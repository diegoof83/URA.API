using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using URA.API.Domain.Models;

namespace URA.API.Persistence.Contexts
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<UserProfile> UserProfiles
        {
            get { return Set<UserProfile>(); }
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //this.Database.EnsureDeleted();
            //this.Database.EnsureCreated();
        }
    }
}
