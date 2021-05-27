using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using URA.API.Domain.Models;

namespace URA.API.Persistence.Repositories
{
    public interface IBaseRepository<T> where T : class, IBaseEntity
    {
        public IEnumerable<T> GetAll();

        public T GetById(long id);

        public T Create(T entity);

        public IEnumerable<T> GetByFilter(Func<T, bool> onFilter);
    }
}
