using System.ComponentModel.DataAnnotations;
using URA.API.Domain.Models;

namespace URA.API.Domain.Dtos.Requests
{
    public class UserSignUpDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }

        public User AsUser()
        {
            return new User
            {
                Email = this.Email,
                UserName = this.Email,
                PasswordHash = this.Password
            };
        }

        public User AsUser(string id)
        {
            var user = AsUser();
            user.Id = id;

            return user;
        }

        public UserProfile AsUserProfile(string userId)
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
