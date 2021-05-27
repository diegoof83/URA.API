using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URA.API.Persistence.Contexts;

namespace URA.API.Persistence.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly AppDbContext _db;

        public BaseRepository(AppDbContext dbContext)
        {
            _db = dbContext;
        }
    }
}
