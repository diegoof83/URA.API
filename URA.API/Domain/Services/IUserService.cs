using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URA.API.Domain.Models;
using URA.API.Domain.Models.Requests;

namespace URA.API.Domain.Services
{
    public interface IUserService
    {
        public Task<IdentityResult> Register(UserSignUpRequest userSignUp);

        public Task<User> GetUserByEmail(string email);
    }
}
