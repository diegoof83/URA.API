using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using URA.API.Domain.Models;
using URA.API.Domain.Dtos.Requests;

namespace URA.API.Domain.Services
{
    public interface IUserService
    {
        public Task<IdentityResult> SignUpAsync(UserSignUpDto userSignUp);

        public Task<User> GetUserByEmailAsync(string email);

        public Task<bool> CheckPasswordAsync(User user, string password);
    }
}
