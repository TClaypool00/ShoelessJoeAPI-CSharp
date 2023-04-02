﻿using Microsoft.AspNetCore.Authorization;
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
    public class PotentialBuysController : ControllerHelper
    {
        private readonly IPotentialBuyService _service;
        private readonly IUserService _userService;
        private readonly IShoeService _shoeService;

        public PotentialBuysController(IPotentialBuyService service, IUserService userService, IShoeService shoeService) : base("PotentialBuys")
        {
            _service = service;
            _userService = userService;
            _shoeService = shoeService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ApiMultiPotentialBuyModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> PotentialBuysAsync(int? userId = null, int? shoeId = null, bool? isSold = null, DateTime? dateSold = null, int? index = null)
        {
            ExtractToken();

            try
            {
                if ((userId is null) || (userId != UserId) && !IsAdmin)
                {
                    return Unauthorized(UnAuthMessage);
                }

                var corePotentialBuys = await _service.GetPotentialBuysAsync(userId, shoeId, isSold, dateSold, index);

                if (corePotentialBuys.Count > 0)
                {
                    var apiPotentialBuys = new List<ApiMultiPotentialBuyModel>();

                    for (int i = 0; i < corePotentialBuys.Count; i++)
                    {
                        apiPotentialBuys.Add(new ApiMultiPotentialBuyModel(corePotentialBuys[i]));
                    }

                    return Ok(apiPotentialBuys);
                }
                else
                {
                    return NotFound("No shoes found");
                }

            } catch (Exception ex)
            {
                return InternalError(ex, _location);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiPotentialBuyModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> PotentialBuyAsync(int id)
        {
            ExtractToken();

            try
            {
                if (await _service.PotentialBuyExistsByIdAsync(id))
                {
                    if (await _service.UserHasAccessToPotentialBuy(UserId, id) || IsAdmin)
                    {
                        var corePotentialBuy = await _service.GetPotentialBuyByIdAsync(id);

                        return Ok(ApiMapper.MapPotentialBuy(corePotentialBuy));
                    }
                    else
                    {
                        return Unauthorized(UnAuthMessage);
                    }
                } else {
                    return NotFound($"No reseource found with an id of {id}");
                }

            } catch (Exception ex)
            {
                return  InternalError(ex, _location);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiPotentialBuyModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> AddPotentialBuy([FromBody] PostPotentialBuyModel model)
        {
            ExtractToken();

            try
            {
                if (ModelState.IsValid)
                {
                    if (await _userService.UserExistsByIdAsync(model.UserId))
                    {
                        if (UserIdDoesMatch(model.UserId) || IsAdmin)
                        {
                            if (await _shoeService.ShoeExistsById(model.ShoeId))
                            {
                                if (!await _service.PotentialBuyExistsByUserIdAsync(model.UserId, model.ShoeId))
                                {
                                    var potentialBuy = ApiMapper.MapPotentialBuy(model);

                                    potentialBuy = await _service.AddPotentialBuyAsync(potentialBuy);

                                    return Ok(ApiMapper.MapPotentialBuy(potentialBuy));
                                }
                                else
                                {
                                    return BadRequest(BidAlreadyExists());
                                }
                            }
                            else
                            {
                                return BadRequest(ShoesController.ShoeNotFoundMessage(model.ShoeId));
                            }
                        }
                        else
                        {
                            return Unauthorized(UnAuthMessage);
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
            } catch (Exception ex)
            {
                return InternalError(ex, _location);
            }
        }

        [HttpPut("Sell/{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> SellShoeAsync(int id, [FromBody] UserIdModel model)
        {
            ExtractToken();

            try
            {
                if (!UserIdDoesMatch(model.UserId))
                {
                    if (await _userService.UserExistsByIdAsync(model.UserId))
                    {
                        if (!_service.IsShoeSoldAsync(id, model.UserId))
                        {
                            if (await _shoeService.ShoeExistsById(id))
                            {
                                if (await _shoeService.ShoeIsOwnedByUser(id, UserId))
                                {
                                    await _service.SellShoeAsync(id, model.UserId);

                                    return Ok("Shoe has been sold!");
                                }
                                else
                                {
                                    return Unauthorized(UnAuthMessage);
                                }
                            }
                            else
                            {
                                return NotFound(ShoesController.ShoeNotFoundMessage(id));
                            }
                        }
                        else
                        {
                            return BadRequest(ShoesController.ShoeIsAlreadySold());
                        }
                    }
                    else
                    {
                        return NotFound(UsersController.UserNotFoundMessage(model.UserId));
                    }
                }
                else
                {
                    return BadRequest(ShoesController.CannotByYourOwnShoe());
                }
            } catch (Exception ex)
            {
                return InternalError(ex, _location);
            }
        }

        public static string BidAlreadyExists()
        {
            return $"You have already placed a bid on this shoe";
        }
    }
}