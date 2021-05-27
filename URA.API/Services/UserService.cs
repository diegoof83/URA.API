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
    public class UserService : IUserService
    {
        private IBaseRepository<User> _repository;

        public UserService(IBaseRepository<User> repository)
        {
            _repository = repository;
        }

        public IEnumerable<User> GetAll()
        {
            return _repository.GetAll();
        }

        public IEnumerable<User> GetByFilter(UserFilter userFilter)
        {
            Expression<Func<User, bool>> isEmailMatched = entity => entity.Email.Contains(userFilter.Email);
            Expression<Func<User, bool>> isNameMatched = entity => entity.Name.Contains(userFilter.Name);

            Expression orExpr = Expression.Or(isEmailMatched, isNameMatched);

            return Filter(orExpr);
        }

        private IEnumerable<User> Filter(Expression expression)
        {
            return _repository.GetByFilter(Expression.Lambda<Func<User,bool>>(expression).Compile());
        }
    }
}
