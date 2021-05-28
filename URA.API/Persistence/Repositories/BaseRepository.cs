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
            return Collection.AsEnumerable<T>();
        }

        public T GetById(long id)
        {
            return Collection.FirstOrDefault(entity => entity.Id == id);
        }
        
        public IEnumerable<T> GetByFilter(Func<T, bool> onFilter)
        {
            return Collection.Where(onFilter).AsEnumerable();
        }

        public T Create(T entity)
        {
            entity = Collection.Add(entity).Entity;
            _db.SaveChanges();

            return entity;
        }

        public T Update(T entity)
        {
            entity = Collection.Update(entity).Entity;
            _db.SaveChanges();

            return entity;
        }

        public void Delete(T entity)
        {
            Collection.Remove(entity);
            _db.SaveChanges();
        }
    }
}
