using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URA.API.Domain.Models;
using URA.API.Persistence.Contexts;

namespace URA.API.TESTS
{
    public class DbForTestsInitializer
    {
        public static void Initialize(AppDbContext dbContext)
        {
            dbContext.Users.AddRange(GetSeedingUsers());
            dbContext.SaveChanges();
        }

        public static void Reinitialize(AppDbContext dbContext)
        {
            dbContext.Users.RemoveRange(dbContext.Users);
            Initialize(dbContext);
        }

        /// <summary>
        /// Create a new list of Users with 3 new Users objects
        /// </summary>
        /// <returns>A list of Users as Enumerable</returns>
        public static List<User> GetSeedingUsers()
        {
            var users = new List<User>();

            for (int i = 1; i <= 3; i++)
            {
                users.Add(CreateNewUser(i));
            }

            return users;
        }

        /// <summary>
        /// Create a new User object
        /// </summary>
        /// <param name="id">The User's identification</param>
        /// <returns></returns>
        public static User CreateNewUser(long id)
        {
            return new()
            {
                Id = id,
                Email = "userTest" + id + "@test.com",
                FirstName = "User" + id,
                LastName = "Test" + id,
                Password = "password" + id
            };
        }
    }
}
