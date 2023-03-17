using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoelessJoeAPI.App.ApiModels;
using ShoelessJoeAPI.App.ApiModels.PartialModels;
using ShoelessJoeAPI.App.ApiModels.PostModels;
using ShoelessJoeAPI.Core.Interfaces;

namespace ShoelessJoeAPI.App.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ManufacturesController : ControllerHelper
    {
        private readonly IManufacterService _service;
        private readonly IUserService _userService;

        public ManufacturesController(IManufacterService service, IUserService userService) : base("Maufacters")
        {
            _service = service;
            _userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ApiManufacter>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetManufacturesAsync(int? userId = null, int? index = null)
        {
            try
            {
                var manufactures = await _service.GetManufactersAsync(userId, index);
                if (manufactures.Count > 0)
                {
                    var apiManufacers = new List<ApiManufacter>();

                    for (int i = 0; i < manufactures.Count; i++)
                    {
                        apiManufacers.Add(ApiMapper.MapManufacter(manufactures[i]));
                    }

                    return Ok(apiManufacers);
                } else
                {
                    return NotFound("No manufacters found");
                }
            } catch (Exception e)
            {
                return InternalError(e, _location);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiManufacter), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetManufacturAsync(int id)
        {
            try
            {
                if (await _service.ManufacterExistsById(id))
                {
                    var coreManufacter = await _service.GetGetManufacterAsync(id);

                    return Ok(ApiMapper.MapManufacter(coreManufacter));
                } else
                {
                    return NotFound(ManufacterNotFoundMessage(id));
                }
            }
            catch (Exception e)
            {
                return InternalError(e, _location);
            }
        }

        [HttpGet("dropdown")]
        public async Task<ActionResult> GetManufacterDropDown([FromQuery] int? userId, int? index)
        {
            try
            {
                if (userId is null)
                {
                    return BadRequest("UserId cannot be null");
                }

                var dropDowns = await _service.GetCoreManufacterDropDown((int)userId, index);

                if (dropDowns.Count > 0)
                {
                    var apiDropDowns = new List<ApiManufacterDropDown>();

                    for (int i = 0; i < dropDowns.Count; i++)
                    {
                        apiDropDowns.Add(ApiMapper.MapManufacter(dropDowns[i]));
                    }

                    return Ok(apiDropDowns);
                }
                else
                {
                    return NotFound("No manufacters found");
                }
            }
            catch (Exception e)
            {
                return InternalError(e, _location);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiManufacterDropDown), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PostManufacterAsync([FromBody] PostManufacter model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _userService.UserExistsByIdAsync(model.UserId))
                    {
                        if (!await _service.ManufacterExistByName(model.ManufacterName, model.UserId))
                        {
                            var coreManufacter = ApiMapper.MapManufacter(model);
                            coreManufacter = await _service.AddManufacterAsync(coreManufacter);

                            return Ok(ApiMapper.MapManufacterDropDown(coreManufacter));
                        }
                        else
                        {
                            return BadRequest($"A manufacter with the {model.ManufacterName} already exists");
                        }
                    }
                    else
                    {
                        return BadRequest(UsersController.UserNotFoundMessage(model.UserId));
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                return InternalError(e, _location);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiManufacter), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PutManufacterAsync(int id, [FromBody] PostManufacter manufacter)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _service.ManufacterExistsByUserId(id, manufacter.UserId))
                    {
                        if (!await _service.ManufacterExistByName(manufacter.ManufacterName, manufacter.UserId))
                        {
                            var coreManufacter = ApiMapper.MapManufacter(manufacter);

                            coreManufacter = await _service.UpdateManufacter(coreManufacter, id);

                            return Ok(ApiMapper.MapManufacter(coreManufacter));
                        }
                        else
                        {
                            return BadRequest(ManufacterNameExists(manufacter.ManufacterName));
                        }
                    }
                    else
                    {
                        return NotFound(ManufacterNotFoundMessage(id));
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                return InternalError(e, _location);
            }
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteManufacterAsync(int id)
        {
            try
            {
                if (await _service.ManufacterExistsById(id))
                {
                    await _service.DeleteManufacterAsync(id);

                    return Ok("Manufacter has been deleted");
                }
                else
                {
                    return NotFound(ManufacterNotFoundMessage(id));
                }
            }
            catch (Exception e)
            {
                return InternalError(e, _location);
            }
        }

        public static string ManufacterNotFoundMessage(int id)
        {
            return $"A manufacter with an id of {id} cannot be found";
        }

        public static string ManufacterNameExists(string name)
        {
            return $"A manufacter with name {name} already exists";
        }

        public static string ManufacterNotAccessNessage()
        {
            return "You do not have access to this manufacter";
        }
    }
}
