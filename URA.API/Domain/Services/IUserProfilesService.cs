using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URA.API.Domain.Filters;
using URA.API.Domain.Models;

namespace URA.API.Domain.Services
{
    public interface IUserProfilesService
    {
        public IEnumerable<UserProfile> GetAll();
        public IEnumerable<UserProfile> GetByFilter(UserFilter filter);
        public UserProfile GetById(string id);
        public UserProfile Create(UserProfile entity);
        public UserProfile Update(UserProfile entity);
        public void Delete(UserProfile entity);
    }
}
