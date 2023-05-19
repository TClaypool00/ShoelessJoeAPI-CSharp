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
                if (manufactures.Count == 0)
                {
                    return NotFound("No manufacters found");
                }

                var apiManufacers = new List<ApiManufacter>();

                for (int i = 0; i < manufactures.Count; i++)
                {
                    apiManufacers.Add(ApiMapper.MapManufacter(manufactures[i]));
                }

                return Ok(apiManufacers);
            } catch (Exception e)
            {
                return InternalError(e);
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
                if (!await _service.ManufacterExistsById(id))
                {
                    return NotFound(ManufacterNotFoundMessage(id));
                }

                var coreManufacter = await _service.GetGetManufacterAsync(id);

                return Ok(ApiMapper.MapManufacter(coreManufacter));
            }
            catch (Exception e)
            {
                return InternalError(e);
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

                if (dropDowns.Count == 0)
                {
                    return NotFound("No manufacters found");
                }

                var apiDropDowns = new List<ApiManufacterDropDown>();

                for (int i = 0; i < dropDowns.Count; i++)
                {
                    apiDropDowns.Add(ApiMapper.MapManufacter(dropDowns[i]));
                }

                return Ok(apiDropDowns);
            }
            catch (Exception e)
            {
                return InternalError(e);
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
                if (!ModelState.IsValid)
                {
                    return BadRequest(DisplaysModelStateErrors());
                }

                if (!await _userService.UserExistsByIdAsync(model.UserId))
                {
                    return BadRequest(UsersController.UserNotFoundMessage(model.UserId));
                }

                if (await _service.ManufacterExistByName(model.ManufacterName, model.UserId))
                {
                    return BadRequest($"A manufacter with the {model.ManufacterName} already exists");
                }

                var coreManufacter = ApiMapper.MapManufacter(model);
                coreManufacter = await _service.AddManufacterAsync(coreManufacter);

                return Ok(ApiMapper.MapManufacterDropDown(coreManufacter));
            }
            catch (Exception e)
            {
                return InternalError(e);
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
                if (!ModelState.IsValid)
                {
                    return BadRequest(DisplaysModelStateErrors());
                }

                if (!await _service.ManufacterExistsByUserId(id, manufacter.UserId))
                {
                    return NotFound(ManufacterNotFoundMessage(id));
                }

                if (await _service.ManufacterExistByName(manufacter.ManufacterName, manufacter.UserId))
                {
                    return BadRequest(ManufacterNameExists(manufacter.ManufacterName));
                }

                var coreManufacter = ApiMapper.MapManufacter(manufacter);

                coreManufacter = await _service.UpdateManufacter(coreManufacter, id);

                return Ok(ApiMapper.MapManufacter(coreManufacter));
            }
            catch (Exception e)
            {
                return InternalError(e);
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
                if (!await _service.ManufacterExistsById(id))
                {
                    return NotFound(ManufacterNotFoundMessage(id));
                }

                await _service.DeleteManufacterAsync(id);

                return Ok("Manufacter has been deleted");
            }
            catch (Exception e)
            {
                return InternalError(e);
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
