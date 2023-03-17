using System.ComponentModel.DataAnnotations;

namespace ShoelessJoeAPI.App.ApiModels.PostModels
{
    public class UserIdModel
    {
        [Required]
        public int UserId { get; set; }
    }
}
