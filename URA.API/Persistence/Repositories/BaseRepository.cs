using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using URA.API.Domain.Models;
using URA.API.Persistence.Contexts;

namespace URA.API.Persistence.Repositories
{
    public abstract class BaseRepository<T>: IBaseRepository<T> where T : class, IBaseEntity
    {
        protected readonly AppDbContext _db;

        protected abstract DbSet<T> Collection { get;}

        public BaseRepository(AppDbContext dbContext)
        {
            _db = dbContext;
        }

        public IEnumerable<T> GetAll()
        {
            return this.Collection.AsEnumerable<T>();
        }

        public T GetById(long id)
        {
            return Collection.Where(entity => entity.Id == id).FirstOrDefault(null);
        }

        public T Create(T entity)
        {
            return this.Collection.Add(entity).Entity;
        }

        public IEnumerable<T> GetByFilter(Func<T, bool> onFilter)
        {
            return Collection.Where(onFilter).AsEnumerable();
        }
    }
}
