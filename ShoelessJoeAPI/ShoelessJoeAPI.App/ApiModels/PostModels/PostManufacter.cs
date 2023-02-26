using System.ComponentModel.DataAnnotations;

namespace ShoelessJoeAPI.App.ApiModels.PostModels
{
    public class PostManufacter
    {
        [Required(ErrorMessage = "Manufacter name is required")]    
        [MaxLength(255, ErrorMessage = "Manufacter name has max length of 255")]
        public string ManufacterName { get; set; }

        [Required(ErrorMessage = "User Id is required")]
        public int UserId { get; set; }
    }
}
