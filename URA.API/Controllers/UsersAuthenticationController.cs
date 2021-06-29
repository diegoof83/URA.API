using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using URA.API.Config;
using URA.API.Domain.Dtos.Requests;
using URA.API.Domain.Dtos.Responses;
using URA.API.Domain.Models;
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
        public async Task<IActionResult> Register(UserSignUpDto userSignUp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var isRegistered = _userService.SignUpAsync(userSignUp);

                    if (isRegistered.Result.Succeeded)
                    {
                        var user = await _userService.GetUserByEmailAsync(userSignUp.Email);
                        var jwtToken = GenerateJwtToken(user);

                        return new CreatedResult($"/users/{isRegistered.Id}", new UserSignUpResponseDto()
                        {
                            Success = true,
                            Token = jwtToken
                        });
                    }
                    else
                    {
                        throw new Exception(string.Join("\n", isRegistered.Result.Errors.Select(x => x.Description).ToList()));
                    }
                }
                throw new Exception("Invalid attributes");
            }
            catch (Exception ex)
            {
                return ValidationProblem(ex.Message);
            }
        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(UserLoginDto userLogin)//TODO Re-factory of this method taking the logic to its service
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userService.GetUserByEmailAsync(userLogin.Email);

                //verifying if user already exists
                if (existingUser is null)
                {
                    return BadRequest(new UserLoginResponseDto()
                    {
                        Erros = new List<string>()
                        {
                            "Invalid login request."
                        },
                        Success = false
                    });
                }

                var isPasswordValid = await _userService.CheckPasswordAsync(existingUser, userLogin.Password);

                if (!isPasswordValid)
                {
                    return BadRequest(new UserLoginResponseDto()
                    {
                        Erros = new List<string>()
                        {
                            "Ivalid login request."
                        },
                        Success = false
                    });
                }

                var jwtToken = GenerateJwtToken(existingUser);

                return Ok(new UserLoginResponseDto()
                {
                    Success = true,
                    Token = jwtToken
                });
            }

            return BadRequest(new UserLoginResponseDto()
            {
                Erros = new List<string>()
                {
                    "Ivalid information(s)."
                },
                Success = false
            });
        }

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
