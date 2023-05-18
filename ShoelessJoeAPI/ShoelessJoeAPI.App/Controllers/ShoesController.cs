using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoelessJoeAPI.App.ApiModels;
using ShoelessJoeAPI.App.ApiModels.PostModels;
using ShoelessJoeAPI.Core.CoreModels;
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
        private readonly IModelService _modelService;
        private readonly IPotentialBuyService _potentialBuyService;

        public ShoesController(IShoeService service, IUserService userService, IModelService modelService, IPotentialBuyService potentialBuyService) : base("Shoes")
        {
            _service = service;
            _userService = userService;
            _modelService = modelService;
            _potentialBuyService = potentialBuyService;

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
                return InternalError(ex);
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
                    if (await _service.ShoeIsOwnedByUserAsync(id, UserId) || IsAdmin)
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
                return InternalError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PostShoeAsync([FromForm] PostShoeModel shoe)
        {
            ExtractToken();

            try
            {
                if (shoe.ValidateLeftShoe())
                {
                    ModelState.AddModelError("RightSize", "Your must add pictures");
                }

                if (shoe.ValidateRightShoe())
                {
                    ModelState.AddModelError("LeftSize", "You must add pictures");
                }

                if (ModelState.IsValid)
                {
                    if (await _modelService.ModelExistsAsync(shoe.ModelId, UserId))
                    {
                        if (!shoe.BothSizesAreNull())
                        {
                            var coreShoe = ApiMapper.MapShoe(shoe);
                            coreShoe.Model = new CoreModel
                            {
                                Manufacter = new CoreManufacter
                                {
                                    UserId = UserId
                                }
                            };

                            coreShoe.ShoeImage = ApiMapper.MapShoeImage(shoe);

                            await _service.AddShoeAsync(coreShoe);

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
                    return BadRequest(DisplaysModelStateErrors());
                }
            } catch (Exception ex)
            {
                return InternalError(ex);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PutShoeAsync(int id, [FromForm] PostShoeModel shoe)
        {
            ExtractToken();

            try
            {
                if (ModelState.IsValid)
                {
                    if (await _service.ShoeExistsById(id))
                    {
                        if (await _service.ShoeIsOwnedByUserAsync(id, UserId) || IsAdmin)
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
                } else
                {
                    return BadRequest(DisplaysModelStateErrors());
                }
                
            } catch (Exception ex)
            {
                return InternalError(ex);
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
                if (!ModelState.IsValid)
                {
                    return BadRequest(DisplaysModelStateErrors());
                }

                if (!await _userService.UserExistsByIdAsync(soldToUserId.UserId))
                {
                    return NotFound(UsersController.UserNotFoundMessage(soldToUserId.UserId));
                }

                if (!await _service.ShoeExistsById(id))
                {
                    return BadRequest(ShoeNotFoundMessage(id));
                }

                if (_potentialBuyService.IsShoeSoldAsync(id, soldToUserId.UserId))
                {
                    return BadRequest(ShoeIsAlreadySold());
                }

                if (soldToUserId.UserId == UserId)
                {
                    return BadRequest(CannotByYourOwnShoe());
                }

                if (!await _service.ShoeIsOwnedByUserAsync(id, UserId))
                {
                    return Unauthorized(UnAuthMessage);
                }


                await _potentialBuyService.SellShoeAsync(id, soldToUserId.UserId);

                return Ok("Shoe has been sold!");
            }
            catch (Exception ex)
            {
                return InternalError(ex);
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

        public static string CannotByYourOwnShoe()
        {
            return "You cannot buy your own shoe(s).";
        }

        public static string ShoeIsAlreadySold()
        {
            return "Shoe has already been sold";
        }
    }
}