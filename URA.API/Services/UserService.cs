using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Transactions;
using URA.API.Domain.Dtos.Requests;
using URA.API.Domain.Models;
using URA.API.Domain.Services;

namespace URA.API.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserProfilesService _userProfilesService;
        public UserService(UserManager<User> userManager, IUserProfilesService userProfilesService)
        {
            _userManager = userManager;
            _userProfilesService = userProfilesService;
        }

        public async Task<IdentityResult> SignUpAsync(UserSignUpDto userSignUp)
        {
            IdentityResult isCreated;

            //open a transaction 
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))       
            {
                var existingUser = await _userManager.FindByEmailAsync(userSignUp.Email);

                //verifying if the email already exists
                if (existingUser != null)
                {
                    //TODO create a new exception UserAlreadyExistsException
                }

                var newUser = userSignUp.AsUser();
                isCreated = await _userManager.CreateAsync(newUser, newUser.PasswordHash);

                var newUserProfile = userSignUp.AsUserProfile(newUser.Id);
                newUserProfile = await _userProfilesService.CreateAsync(newUserProfile);

                scope.Complete();
            }

            return isCreated;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            return existingUser;
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            var isPasswordChecked = await _userManager.CheckPasswordAsync(user, password);

            return isPasswordChecked;
        }
    }
}
