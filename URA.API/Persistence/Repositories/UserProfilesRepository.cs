using Microsoft.EntityFrameworkCore;
using URA.API.Domain.Models;
using URA.API.Persistence.Contexts;

namespace URA.API.Persistence.Repositories
{
    public class UserProfilesRepository : BaseRepository<UserProfile>
    {
        public UserProfilesRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
        protected override DbSet<UserProfile> Collection => _db.UserProfiles;
    }
}
