using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using URA.API.Domain.Models;

namespace URA.API.Persistence.Repositories
{
    public interface IBaseRepository<T> where T : class, IBaseEntity
    {
        public IEnumerable<T> GetAll();

        public T GetById(string id);        

        public IEnumerable<T> GetByFilter(Func<T, bool> onFilter);

        public Task<T> CreateAsync(T entity);

        public T Update(T entity);

        public void Delete(T entity);
    }
}
