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
    public class ModelsController : ControllerHelper
    {
        private readonly IModelService _service;
        private readonly IManufacterService _manufacterService;
        private readonly IUserService _userService;

        public ModelsController(IModelService service, IManufacterService manufacterService, IUserService userService) : base("Models")
        {
            _service = service;
            _manufacterService = manufacterService;
            _userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ApiModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetModelsAsync([FromQuery] int? userId = null, int? index = null)
        {
            try
            {
                var coreModels = await _service.GetModelsAsync(userId, index);

                if (coreModels.Count > 0)
                {
                    var apiModels = new List<ApiModel>();
                    for (int i = 0; i < coreModels.Count; i++)
                    {
                        apiModels.Add(ApiMapper.MapModel(coreModels[i]));
                    }

                    return Ok(apiModels);
                } else
                {
                    return NotFound("No models found");
                }
            } catch (Exception e)
            {
                return InternalError(e);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiManufacter), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetModelByIdAsync(int id)
        {
            try
            {
                if (await _service.ModelExistsAsync(id))
                {
                    var coreModel = await _service.GetModelAsync(id);

                    return Ok(ApiMapper.MapModel(coreModel));
                }
                else
                {
                    return NotFound(ModelNotFoundMessage(id));
                }
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiManufacterDropDown), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PostModelAsync([FromBody] PostModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _userService.UserExistsByIdAsync(model.UserId))
                    {
                        if (await _manufacterService.ManufacterExistsByUserId(model.ManufacterId, model.UserId))
                        {
                            if (!await _service.ModelNameExistsAsync(model.ModelName, model.UserId))
                            {
                                var coreModel = ApiMapper.MapModel(model);
                                coreModel = await _service.AddModelAsync(coreModel);

                                return Ok(ApiMapper.MapModelDropDown(coreModel));
                            }
                            else
                            {
                                return BadRequest(ModelNameExistMessage(model.ModelName));
                            }
                        }
                        else
                        {
                            return Unauthorized(ManufacturesController.ManufacterNotAccessNessage());
                        }
                    }
                    else
                    {
                        return NotFound(UsersController.UserNotFoundMessage(model.UserId));
                    }
                }
                else
                {
                    return BadRequest();
                }

            } catch (Exception e)
            {
                return InternalError(e);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiManufacterDropDown), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateModelAsync(int id, [FromBody] PostModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _manufacterService.ManufacterExistsByUserId(id, model.UserId))
                    {
                        if (await _service.ModelNameExistsAsync(model.ModelName, model.UserId, id))
                        {
                            var coreModel = ApiMapper.MapModel(model, id);

                            coreModel = await _service.UpdateModelAsync(coreModel, id);

                            return Ok(ApiMapper.MapModelDropDown(coreModel));
                        }
                        else
                        {
                            return BadRequest(ModelNameExistMessage(model.ModelName));
                        }
                    }
                    else
                    {
                        return Unauthorized(ManufacturesController.ManufacterNotAccessNessage());
                    }
                }
                else
                {
                    return BadRequest();
                }
            } catch (Exception e)
            {
                return InternalError(e);
            }
        }

        public static string ModelNotFoundMessage(int id)
        {
            return $"A model with an id of {id} cannot be found";
        }

        public static string ModelNameExistMessage(string name)
        {
            return $"A model with the name of {name} already exists";
        }
    }
}
