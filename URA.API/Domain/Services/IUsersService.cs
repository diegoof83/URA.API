using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URA.API.Domain.Filters;
using URA.API.Domain.Models;

namespace URA.API.Domain.Services
{
    public interface IUsersService
    {
        public IEnumerable<User> GetAll();
        public IEnumerable<User> GetByFilter(UserFilter filter);
        public User GetById(long id);
        public User Create(User entity);
        public User Update(User entity);
        public void Delete(User entity);
    }
}
