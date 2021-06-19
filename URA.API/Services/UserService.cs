using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using URA.API.Domain.Models;
using URA.API.Domain.Models.Requests;
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
                
        public async Task<IdentityResult> Register(UserSignUpRequest userSignUp)
        {
            IdentityResult isCreated;

            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
            {
                var existingUser = await _userManager.FindByEmailAsync(userSignUp.Email);

                //verifying if the email already exists
                if (existingUser != null)
                {
                    //TODO create a new exception UserAlreadyExistsException
                }

                var newUser = userSignUp.ToUser();
                isCreated = await _userManager.CreateAsync(newUser, newUser.PasswordHash);

                var newUserProfile = userSignUp.ToUserProfile(newUser.Id);

                _userProfilesService.Create(newUserProfile);

                scope.Complete();
            }

            return isCreated;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var existingIdentityUser = await _userManager.FindByEmailAsync(email);

            return existingIdentityUser;
        }
    }
}
