using Microsoft.EntityFrameworkCore;
using URA.API.Domain.Models;
using URA.API.Persistence.Contexts;

namespace URA.API.Persistence.Repositories
{
    public class UsersRepository : BaseRepository<User>
    {
        public UsersRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
        protected override DbSet<User> Collection => _db.Users;
    }
}
