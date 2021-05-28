using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using URA.API.Domain.Filters;
using URA.API.Domain.Models;
using URA.API.Domain.Services;
using URA.API.Persistence.Repositories;

namespace URA.API.Services
{
    public class UsersService : IUsersService
    {
        private IBaseRepository<User> _repository;

        public UsersService(IBaseRepository<User> repository)
        {
            _repository = repository;
        }

        public IEnumerable<User> GetAll()
        {
            return _repository.GetAll();
        }

        public User GetById(long id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<User> GetByFilter(UserFilter filter)
        {
            Expression<Func<User, bool>> isEmailMatched = entity => entity.Email.Contains(filter.Email);
            Expression<Func<User, bool>> isNameMatched = entity => entity.FirstName.Contains(filter.Name);

            Expression orExpr = Expression.Or(isEmailMatched, isNameMatched);

            return Filter(orExpr);
        }       

        private IEnumerable<User> Filter(Expression expression)
        {
            return _repository.GetByFilter(Expression.Lambda<Func<User,bool>>(expression).Compile());
        }

        public User Create(User entity)
        {
            return _repository.Create(entity);
        }

        public User Update(User entity)
        {
            return _repository.Update(entity);
        }

        public void Delete(User entity)
        {
            _repository.Delete(entity);
        }
    }
}
