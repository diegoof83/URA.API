using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URA.API.Domain.Models;
using URA.API.Persistence.Contexts;

namespace URA.API.Persistence.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
        protected override DbSet<User> Collection => _db.Users;
    }
}
