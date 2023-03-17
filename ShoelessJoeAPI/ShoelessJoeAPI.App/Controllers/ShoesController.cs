using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoelessJoeAPI.App.ApiModels;
using ShoelessJoeAPI.App.ApiModels.PostModels;
using ShoelessJoeAPI.Core.Interfaces;

namespace ShoelessJoeAPI.App.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ShoesController : ControllerHelper
    {
        private readonly IShoeService _service;
        private readonly IUserService _userService;
        private readonly IManufacterService _manufacturerService;
        private readonly IModelService _modelService;
        private readonly string _location;

        public ShoesController(IShoeService service, IUserService userService, IManufacterService manufacterService, IModelService modelService)
        {
            _service = service;
            _userService = userService;
            _manufacturerService = manufacterService;
            _modelService = modelService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ApiShoeModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetShoesAsync([FromQuery] int? ownerId = null, int? soldToId = null, DateTime? datePosted = null, bool? isSold = null, int? index = null)
        {
            ExtractToken();

            try
            {
                if (ownerId is not null && ownerId != UserId && !IsAdmin)
                {
                    return Unauthorized();
                }

                var apiShoes = new List<ApiShoeModel>();

                var coreShoes = await _service.GetShoesAsync(ownerId, soldToId, datePosted, isSold, index);

                if (coreShoes.Count > 0)
                {
                    for (int i = 0; i < coreShoes.Count; i++)
                    {
                        apiShoes.Add(ApiMapper.MapShoe(coreShoes[i]));
                    }

                    return Ok(apiShoes);
                }
                else
                {
                    return NotFound("No shoes found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ErrorMessage);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiManufacter), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetShoeByIdAsync(int id)
        {
            ExtractToken();

            try
            {
                if (await _service.ShoeExistsById(id))
                {
                    if (await _service.ShoeIsOwnedByUser(id, UserId) || IsAdmin)
                    {
                        var shoe = await _service.GetShoesAsync(id);

                        return Ok(ApiMapper.MapShoe(shoe));
                    }
                    else
                    {
                        return Unauthorized(UnAuthMessage);
                    }
                }
                else
                {
                    return NotFound(ShoeNotFoundMessage(id));
                }
            } catch (Exception ex)
            {
                return StatusCode(500, ErrorMessage);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PostShoeAsync([FromBody] PostShoeModel shoe)
        {
            ExtractToken();

            try
            {                
                if (ModelState.IsValid)
                {
                    if (await _modelService.ModelExistsAsync(shoe.ModelId, UserId))
                    {
                        if (!shoe.BothSizesAreNull())
                        {
                            await _service.AddShoeAsync(ApiMapper.MapShoe(shoe));

                            return Ok("Shoe has been added");
                        }
                        else
                        {
                            return BadRequest(BothSizesAreNullMessage());
                        }
                    }
                    else
                    {
                        return Unauthorized(UnAuthMessage);
                    }
                }
                else
                {
                    return BadRequest();
                }
            } catch (Exception ex)
            {
                return StatusCode(500, ErrorMessage);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PutShoeAsync(int id, [FromBody] PostShoeModel shoe)
        {
            ExtractToken();

            try
            {
                if (await _service.ShoeExistsById(id))
                {
                    if (await _service.ShoeIsOwnedByUser(id, UserId) || IsAdmin)
                    {
                        if (ModelState.IsValid)
                        {
                            if (!shoe.BothSizesAreNull())
                            {
                                var coreShoe = ApiMapper.MapShoe(shoe, id);

                                coreShoe = await _service.UpdateShoeAsync(coreShoe, id);

                                return Ok("Shoe has been updated!");
                            }
                            else
                            {
                                return BadRequest(BothSizesAreNullMessage());
                            }
                        } else
                        {
                            return BadRequest();
                        }
                    }
                    else
                    {
                        return Unauthorized(UnAuthMessage);
                    }
                }
                else
                {
                    return NotFound(ShoeNotFoundMessage(id));
                }
            } catch (Exception ex)
            {
                return StatusCode(500, ErrorMessage);
            }
        }

        [HttpPut("Sell/{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ShoeSoldAsync(int id, [FromBody] UserIdModel soldToUserId)
        {
            ExtractToken();

            try
            {
                if (await _service.ShoeExistsById(id))
                {
                    if (!_service.ShoeIsAlreadySold(id))
                    {
                        if (UserId != soldToUserId.UserId)
                        {
                            if (ModelState.IsValid)
                            {
                                await _service.SellShoeAsync(id, soldToUserId.UserId);

                                return Ok("Shoe has been sold!");
                            }
                            else 
                            { 
                                return BadRequest(); 
                            }
                        }
                        else
                        {
                            return BadRequest(CannotByYourOwnShoe());
                        }
                    }
                    else
                    {
                        return BadRequest("This is show is already sold");
                    }
                }
                else
                {
                    return NotFound(ShoeNotFoundMessage(id));
                }
            } catch (Exception ex)
            {
                return StatusCode(500, ErrorMessage);
            }
        }

        public static string ShoeNotFoundMessage(int id)
        {
            return $"A shoe with an id of {id} does not exists";
        }

        protected string BothSizesAreNullMessage()
        {
            return "Both left shoe size and right shoe size cannot be empty";
        }

        protected string CannotByYourOwnShoe()
        {
            return "You cannot buy your own shoe(s).";
        }
    }
}