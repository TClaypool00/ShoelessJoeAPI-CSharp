using ShoelessJoeAPI.App.ApiModels.UpdateModels;
using System.ComponentModel.DataAnnotations;

namespace ShoelessJoeAPI.App.ApiModels.PostModels
{
    public class PostCommentModel : UpdateCommentModel
    {
        [Required(ErrorMessage = "User Id is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Potential Buy Id is required")]
        public int PotentialBuyId { get; set; }
    }
}
