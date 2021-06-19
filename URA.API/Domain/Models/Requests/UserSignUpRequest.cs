using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace URA.API.Domain.Models.Requests
{
    public class UserSignUpRequest
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }

        public User ToUser()
        {
            return new User
            {
                Email = this.Email,
                UserName = this.Email,
                PasswordHash = this.Password
            };
        }

        public User ToUser(string id)
        {
            var user = ToUser();
            user.Id = id;

            return user;
        }

        public UserProfile ToUserProfile(string userId)
        {
            return new UserProfile
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                UserId = userId
            };
        }
    }
}
