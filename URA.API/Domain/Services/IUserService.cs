using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using URA.API.Domain.Models;
using URA.API.Domain.Dtos.Requests;
using System.Transactions;
using URA.API.Config.Attributes;

namespace URA.API.Domain.Services
{
    public interface IUserService
    {
        [Transactional(TransactionScopeAsyncFlowOption.Enabled)]
        public Task<IdentityResult> SignUpAsync(UserSignUpDto userSignUp);

        public Task<User> GetUserByEmailAsync(string email);

        public Task<bool> CheckPasswordAsync(User user, string password);
    }
}
