using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using URA.API.Domain.Filters;
using URA.API.Domain.Models;
using URA.API.Domain.Services;
using URA.API.Persistence.Repositories;

namespace URA.API.Services
{
    public class UserProfilesService : IUserProfilesService
    {
        private IBaseRepository<UserProfile> _repository;

        public UserProfilesService(IBaseRepository<UserProfile> repository)
        {
            _repository = repository;
        }

        public IEnumerable<UserProfile> GetAll()
        {
            return _repository.GetAll();
        }

        public UserProfile GetById(string id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<UserProfile> GetByFilter(UserFilter filter)
        {
            //Expression<Func<UserProfile, bool>> isEmailMatched = entity => entity.Email.Contains(filter.Email);
            Expression<Func<UserProfile, bool>> isNameMatched = entity => entity.FirstName.Contains(filter.Name);

            //Expression orExpr = Expression.Or(isEmailMatched, isNameMatched);

            //return Filter(orExpr);
            return Filter(isNameMatched);
        }

        private IEnumerable<UserProfile> Filter(Expression expression)
        {
            return _repository.GetByFilter(Expression.Lambda<Func<UserProfile,bool>>(expression).Compile());
        }

        public async Task<UserProfile> CreateAsync(UserProfile entity)
        {
            return await _repository.CreateAsync(entity);
        }

        public UserProfile Update(UserProfile entity)
        {
            return _repository.Update(entity);
        }

        public void Delete(UserProfile entity)
        {
            _repository.Delete(entity);
        }
    }
}
