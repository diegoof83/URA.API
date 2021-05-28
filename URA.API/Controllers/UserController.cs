using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URA.API.Domain.Models;
using URA.API.Domain.Services;

namespace URA.API.Controllers
{
    [ApiController]
    [Route("api/ura/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService  userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IEnumerable<User> GetAll() 
        {
            var users = _userService.GetAll();

            return users;
        }

    }
}
