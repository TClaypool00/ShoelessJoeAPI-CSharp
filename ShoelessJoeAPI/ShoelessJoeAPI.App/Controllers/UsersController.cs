using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShoelessJoeAPI.App.ApiModels;
using ShoelessJoeAPI.App.FileIO;
using ShoelessJoeAPI.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ShoelessJoeAPI.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerHelper
    {
        private readonly IUserService _service;
        private readonly IConfiguration _configuration;

        public UsersController(IUserService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }

        /// <summary>
        /// Retrieves all the users in the database and converts them to type ApiUser
        /// </summary>
        /// <returns></returns>

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(List<ApiUser>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> GetAllUsers()
        {
            try
            {                
                var coreUsers = await _service.GetUsersAsync();

                if (coreUsers.Count > 0)
                {
                    var apiUsers = new List<ApiUser>();

                    for (int i = 0; i < coreUsers.Count; i++)
                    {
                        apiUsers.Add(ApiMapper.MapUser(coreUsers[i]));
                    }

                    return Ok(apiUsers);
                } else
                {
                    return NotFound("No users found.");
                }
            } catch (Exception e)
            {
                FileWriter.WriteError(e, this);
                return StatusCode(500, ErrorMessage);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetUserAsync(int id)
        {
            try
            {
                if (await _service.UserExistsByIdAsync(id))
                {
                    var coreUser = await _service.GetUserByIdAsync(id);

                    return Ok(ApiMapper.MapUser(coreUser));
                }
                else
                {
                    return NotFound(UserNotFoundMessage(id));
                }
            } catch(Exception e)
            {
                FileWriter.WriteError(e, this);
                return StatusCode(500, ErrorMessage);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PostUserAsync([FromBody] RegisterModel model)
        {
            try
            {
                if (await _service.UserExistsByEmailAsync(model.Email))
                {
                    return BadRequest(EmailAlreadyExistsMessage(model.Email));
                }

                if (await _service.UserExistsByPhoneNumbAsync(model.PhoneNumb))
                {
                    return BadRequest(PhoneNumbExistsMessage(model.PhoneNumb));
                }                

                if (ModelState.IsValid)
                {
                    var coreUser = ApiMapper.MapUser(model);
                    coreUser.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

                    await _service.AddUserAsync(coreUser);

                    return Ok("User has been registered");
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception e)
            {
                FileWriter.WriteError(e, this);
                return StatusCode(500, ErrorMessage);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiUser), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PutUserAsync(int id, [FromBody] ApiUser model)
        {
            try
            {
                if (await _service.UserExistsByEmailAsync(model.Email, id))
                {
                    return BadRequest(EmailAlreadyExistsMessage(model.Email));
                }

                if (await _service.UserExistsByPhoneNumbAsync(model.PhoneNumb, id))
                {
                    return BadRequest(PhoneNumbExistsMessage(model.PhoneNumb));
                }

                if (await _service.UserExistsByIdAsync(model.UserId))
                {
                    if (ModelState.IsValid)
                    {
                        var coreUser = ApiMapper.MapUser(model);

                        await _service.UpdateUserAsync(coreUser, id);

                        return Ok(ApiMapper.MapUser(coreUser));
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return NotFound(UserNotFoundMessage(id));
                }
            }
            catch (Exception e)
            {
                FileWriter.WriteError(e, this);
                return StatusCode(500, ErrorMessage);
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
                if (ModelState.IsValid)
                {
                    if (await _service.UserExistsByEmailAsync(model.Email))
                    {
                        var coreUser = await _service.GetUserByEmailAsync(model.Email);

                        if (!BCrypt.Net.BCrypt.Verify(model.Password, coreUser.Password))
                        {
                            return BadRequest("Incorrect password");
                        }

                        var claims = new[]
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

                        var jwt = tokenHandler.WriteToken(token);

                        return Ok(ApiMapper.MapUser(coreUser, jwt));
                    }
                    else
                    {
                        return BadRequest("Incorrect email address");
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                FileWriter.WriteError(e, this);
                return StatusCode(500, ErrorMessage);
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
            
    }
}
