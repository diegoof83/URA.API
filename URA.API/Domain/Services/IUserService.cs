using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URA.API.Domain.Filters;
using URA.API.Domain.Models;

namespace URA.API.Domain.Services
{
    public interface IUserService
    {
        public IEnumerable<User> GetAll();
        public IEnumerable<User> GetByFilter(UserFilter userFilter);
    }
}
