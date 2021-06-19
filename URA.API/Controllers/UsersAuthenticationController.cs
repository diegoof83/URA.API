using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using URA.API.Config;
using URA.API.Domain.Models;
using URA.API.Domain.Models.Requests;
using URA.API.Domain.Models.Responses;
using URA.API.Domain.Services;

namespace URA.API.Controllers
{
    [ApiController]
    [Route("api/ura/[controller]")]
    public class UsersAuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtConfig _jwtConfig;

        public UsersAuthenticationController(
            IUserService userService,
            IOptionsMonitor<JwtConfig> optionsManager)
        {
            _userService = userService;
            _jwtConfig = optionsManager.CurrentValue;
        }

        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(UserSignUpRequest userSignUp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var isCreated = _userService.Register(userSignUp);

                    if (isCreated.Result.Succeeded)
                    {
                        var user = await _userService.GetUserByEmail(userSignUp.Email);
                        var jwtToken = GenerateJwtToken(user);

                        return new CreatedResult($"/users/{isCreated.Id}", new RegistrationResponse()
                        {
                            Success = true,
                            Token = jwtToken
                        });
                    }
                    else
                    {
                        throw new Exception(string.Join("\n", isCreated.Result.Errors.Select(x => x.Description).ToList()));
                    }
                }
                throw new Exception("Invalid attributes");
            }
            catch (Exception ex)
            {
                return ValidationProblem(ex.Message);
            }
        }

        //[HttpPost]
        //[Route("Login")]
        //public async Task<IActionResult> Loging(UserLoginRequest userLogin)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var existingIdentityUser = await _userManager.FindByEmailAsync(userLogin.Email);

        //        //verifying if user already exists
        //        if (existingIdentityUser == null)
        //        {
        //            return BadRequest(new RegistrationResponse()
        //            {
        //                Erros = new List<string>()
        //                {
        //                    "Invalid login request."
        //                },
        //                Success = false
        //            });
        //        }

        //        var isPasswordValid = await _userManager.CheckPasswordAsync(existingIdentityUser, userLogin.Password);

        //        if (!isPasswordValid)
        //        {
        //            return BadRequest(new RegistrationResponse()
        //            {
        //                Erros = new List<string>()
        //                {
        //                    "Ivalid login request."
        //                },
        //                Success = false
        //            });
        //        }

        //        var jwtToken = GenerateJwtToken(existingIdentityUser);

        //        return Ok(new RegistrationResponse()
        //        {
        //            Success = true,
        //            Token = jwtToken
        //        });
        //    }

        //    return BadRequest(new RegistrationResponse()
        //    {
        //        Erros = new List<string>()
        //        {
        //            "Ivalid information(s)."
        //        },
        //        Success = false
        //    });
        //}

        private string GenerateJwtToken(User user)
        {
            // Defines the jwt token which will be responsible of creating tokens
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            // Gets the secret from the app settings
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            // Defines our token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                // Adds claims which are properties that gives information about the token that belongs to the specific user
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id",user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(6),
                // Adds the encryption algorithm information which will be used to decrypt the token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}
