using System.ComponentModel.DataAnnotations;

namespace ShoelessJoeAPI.App.ApiModels.PostModels
{
    public class PostRefreshTokenModel
    {
        [Required(ErrorMessage ="Token is required")]
        public string Token { get; set; }

        [Required(ErrorMessage = "Refresh token is required")]
        public string RefreshToken { get; set; }
    }
}
