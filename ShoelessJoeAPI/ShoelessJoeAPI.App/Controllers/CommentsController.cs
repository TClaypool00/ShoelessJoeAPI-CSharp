using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoelessJoeAPI.App.ApiModels;
using ShoelessJoeAPI.App.ApiModels.PostModels;
using ShoelessJoeAPI.App.ApiModels.UpdateModels;
using ShoelessJoeAPI.Core.Interfaces;

namespace ShoelessJoeAPI.App.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerHelper
    {
        private readonly ICommentService _service;
        private readonly IUserService _userService;
        private readonly IPotentialBuyService _potentialBuyService;
        private readonly IShoeService _shoeService;

        public CommentsController(ICommentService service, IUserService userService, IPotentialBuyService potentialBuyService, IShoeService shoeService) : base("Comments")
        {
            _service = service;
            _userService = userService;
            _potentialBuyService = potentialBuyService;
            _shoeService = shoeService;

        }

        [HttpGet]
        public async Task<ActionResult> GetCommentsAsync(int? potentialBuyId = null, int? shoeId = null, int? userId = null, int? index = null)
        {
            ExtractToken();

            try
            {
                if (!IsAdmin)
                {
                    if (userId != UserId)
                    {
                        return Unauthorized(UnAuthMessage);
                    }

                    if (potentialBuyId is not null && !await _potentialBuyService.UserHasAccessToPotentialBuy(UserId, (int)potentialBuyId))
                    {
                        return Unauthorized(UnAuthMessage);
                    }

                    if (shoeId is not null && !await _shoeService.ShoeIsOwnedByUserAsync((int)shoeId, UserId))
                    {
                        return Unauthorized(UnAuthMessage);
                    }
                }

                var comments = await _service.GetCommentsAsync(potentialBuyId, shoeId, userId, index);

                if (comments.Count == 0)
                {
                    return NotFound("No comments found");
                }

                var apiComments = new List<ApiCommentModel>();

                for (int i = 0; i < comments.Count; i++)
                {
                    apiComments.Add(ApiMapper.MapComment(comments[i]));
                }

                return Ok(apiComments);

            } catch (Exception ex)
            {
                return InternalError(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCommentByIdAsync(int id)
        {
            ExtractToken();

            try
            {
                if (!await _service.CommentExistsByIdAsync(id))
                {
                    return NotFound(CommentNotfoundMessage(id));
                }

                if (!await _service.CommentOwnedByUserAsync(id, UserId) && !IsAdmin)
                {
                    return Unauthorized(UnAuthMessage);
                }

                var comment = await _service.GetCommentAsync(id);

                return Ok(ApiMapper.MapComment(comment));


            } catch (Exception ex)
            {
                return InternalError(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostCommentAsync(PostCommentModel model)
        {
            ExtractToken();

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(DisplaysModelStateErrors());
                }

                if (!await _userService.UserExistsByIdAsync(model.UserId))
                {
                    return NotFound(UsersController.UserNotFoundMessage(model.UserId));
                }

                if (!await _potentialBuyService.PotentialBuyExistsByIdAsync(model.PotentialBuyId))
                {
                    return NotFound(PotentialBuysController.PotentialBuyNotFoundMessage(model.PotentialBuyId));
                }

                if (!await _potentialBuyService.UserHasAccessToPotentialBuy(model.UserId, model.PotentialBuyId) && !IsAdmin)
                {
                    return Unauthorized(UnAuthMessage);
                }

                if (!UserIdDoesMatch(model.UserId))
                {
                    return Unauthorized(UnAuthMessage);
                }

                if (await _potentialBuyService.IsShoeSoldByPotentialBuyId(model.PotentialBuyId))
                {
                    return BadRequest(ShoesController.ShoeIsAlreadySold());
                }

                var comment = ApiMapper.MapComment(model);
                comment = await _service.AddCommentAsync(comment);

                return Ok(ApiMapper.MapComment(comment));
            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutCommentAsync(int id, [FromBody] UpdateCommentModel model)
        {
            ExtractToken();

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(DisplaysModelStateErrors());
                }

                if (await _potentialBuyService.IsShoeSoldByCommentId(id, UserId))
                {
                    return BadRequest(ShoesController.ShoeIsAlreadySold());
                }

                if (!await _service.CommentExistsByIdAsync(id))
                {
                    return NotFound(CommentNotfoundMessage(id));
                }

                if (!await _service.CommentOwnedByUserAsync(id, UserId) && !IsAdmin)
                {
                    return Unauthorized(UnAuthMessage);
                }

                var comment = ApiMapper.MapComment(model, id);
                comment = await _service.UpdateCommentAsync(comment);

                return Ok(ApiMapper.MapComment(comment));


            } catch (Exception ex)
            {
                return InternalError(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCommentAsync(int id)
        {
            ExtractToken();

            try
            {
                if (!await _service.CommentExistsByIdAsync(id))
                {
                    return NotFound(CommentNotfoundMessage(id));
                }

                if (!await _service.CommentOwnedByUserAsync(id, UserId) && !IsAdmin)
                {
                    return Unauthorized(UnAuthMessage);
                }

                if (await _potentialBuyService.IsShoeSoldByCommentId(id, UserId))
                {
                    return BadRequest(ShoesController.ShoeIsAlreadySold());
                }

                await _service.DeleteCommentAsync(id);

                return Ok("Comment has been deleted");
            } catch (Exception ex)
            {
                return InternalError(ex);
            }
        }

        public static string CommentNotfoundMessage(int id)
        {
            return $"Comment with an id of {id} does not exist";
        }
    }
}
