using System.ComponentModel.DataAnnotations;

namespace ShoelessJoeAPI.App.ApiModels.UpdateModels
{
    public class UpdateCommentModel
    {
        [Required(ErrorMessage = "Comment Text is required")]
        [MaxLength(255, ErrorMessage = "Comment Text has max length of 255 characters")]
        public string CommentText { get; set; }
    }
}
