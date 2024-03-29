﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using ShoelessJoeAPI.App.ApiModels;
using ShoelessJoeAPI.App.ApiModels.PostModels;
using ShoelessJoeAPI.Core.CoreModels;
using ShoelessJoeAPI.Core.Interfaces;
using ShoelessJoeAPI.DataAccess.DataModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ShoelessJoeAPI.App.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerHelper
    {
        private readonly IUserService _service;
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenService _refreshTokenService;

        public UsersController(IUserService service, IConfiguration configuration, IRefreshTokenService refreshTokenService) : base("Users")
        {
            _service = service;
            _configuration = configuration;
            _refreshTokenService = refreshTokenService;
        }

        /// <summary>
        /// Retrieves all the users in the database and converts them to type ApiUser
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<ApiUser>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> GetAllUsers()
        {
            try
            {
                var coreUsers = await _service.GetUsersAsync();

                if (coreUsers.Count == 0)
                {
                    return NotFound("No users found.");
                }

                var apiUsers = new List<ApiUser>();

                for (int i = 0; i < coreUsers.Count; i++)
                {
                    apiUsers.Add(ApiMapper.MapUser(coreUsers[i]));
                }

                return Ok(apiUsers);
            } catch (Exception e)
            {
                return InternalError(e);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetUserAsync(int id)
        {
            try
            {
                if (!await _service.UserExistsByIdAsync(id))
                {
                    return NotFound(UserNotFoundMessage(id));
                }

                var coreUser = await _service.GetUserByIdAsync(id);

                return Ok(ApiMapper.MapUser(coreUser));
            } catch (Exception e)
            {
                return InternalError(e);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PostUserAsync([FromBody] RegisterModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(DisplaysModelStateErrors());
                }

                if (await _service.UserExistsByEmailAsync(model.Email))
                {
                    return BadRequest(EmailAlreadyExistsMessage(model.Email));
                }

                if (await _service.UserExistsByPhoneNumbAsync(model.PhoneNumb))
                {
                    return BadRequest(PhoneNumbExistsMessage(model.PhoneNumb));
                }

                var coreUser = ApiMapper.MapUser(model);
                coreUser.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

                await _service.AddUserAsync(coreUser);

                return Ok("User has been registered");

            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiUser), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PutUserAsync(int id, [FromBody] ApiUser model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(DisplaysModelStateErrors());
                }

                if (!await _service.UserExistsByIdAsync(model.UserId))
                {
                    return NotFound(UserNotFoundMessage(id));
                }

                if (await _service.UserExistsByEmailAsync(model.Email, id))
                {
                    return BadRequest(EmailAlreadyExistsMessage(model.Email));
                }

                if (await _service.UserExistsByPhoneNumbAsync(model.PhoneNumb, id))
                {
                    return BadRequest(PhoneNumbExistsMessage(model.PhoneNumb));
                }

                var coreUser = ApiMapper.MapUser(model);

                await _service.UpdateUserAsync(coreUser, id);

                return Ok(ApiMapper.MapUser(coreUser));
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(typeof(ApiUser), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> LoginAsync([FromBody] LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(DisplaysModelStateErrors());
                }

                if (!await _service.UserExistsByEmailAsync(model.Email))
                {
                    return BadRequest("Incorrect email address");
                }

                var coreUser = await _service.GetUserByEmailAsync(model.Email);

                if (!BCrypt.Net.BCrypt.Verify(model.Password, coreUser.Password))
                {
                    return BadRequest("Incorrect password");
                }

                var claims = GetClaims(coreUser);

                string token = CreateToken(claims);
                string refreshToken = CreateRefreshToken();

                await _refreshTokenService.AddRefreshTokenAsync(refreshToken, DateTime.Now.AddDays(7), coreUser.UserId);


                return Ok(ApiMapper.MapUser(coreUser, token, refreshToken));
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }
        [HttpPost("RefeshToken")]
        public async Task<ActionResult> PostRefreshToken(PostRefreshTokenModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(DisplaysModelStateErrors());
                }

                if (!await _refreshTokenService.RefreshTokenExists(model.RefreshToken))
                {
                    return NotFound("Refresh token not found");
                }

                var refreshToken = await _refreshTokenService.GetRefreshTokn(model.RefreshToken);

                if (refreshToken.DateExpired < DateTime.Now)
                {
                    return BadRequest("Refresh token has expired");
                }

                var currentUser = GetPrincipalFromExpiredToken(model.Token);
                int userId = int.Parse(currentUser.Claims.FirstOrDefault(x => x.Type.Equals("UserId")).Value);

                if (userId != refreshToken.UserId)
                {
                    return Unauthorized(UnAuthMessage);
                }

                return Ok(CreateToken(currentUser.Claims));

            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }

        public static string UserNotFoundMessage(int id)
        {
            return $"A user with an id of {id} cannot be found";
        }

        private static string EmailAlreadyExistsMessage(string email)
        {
            return $"A user with an email address of {email} already exists";
        }

        private static string PhoneNumbExistsMessage(string phoneNumb)
        {
            return $"A user with a phone number of {phoneNumb} already exists";
        }

        private string CreateToken(IEnumerable<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]));

            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescripton = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = signIn
            };

            var token = tokenHandler.CreateToken(tokenDescripton);

            return tokenHandler.WriteToken(token);
        }

        private static string CreateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private List<Claim> GetClaims(CoreUser coreUser)
        {
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                new Claim("UserId", coreUser.UserId.ToString()),
                new Claim("Email", coreUser.Email),
                new Claim("PhoneNumber", coreUser.PhoneNumb),
                new Claim("FirstName", coreUser.FirstName),
                new Claim("LastName", coreUser.LastName),
                new Claim("IsAdmin", coreUser.IsAdmin.ToString())
            };
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:Key"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
