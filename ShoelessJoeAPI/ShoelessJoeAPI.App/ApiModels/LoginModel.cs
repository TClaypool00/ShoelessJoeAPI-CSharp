using System.ComponentModel.DataAnnotations;

namespace ShoelessJoeAPI.App.ApiModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is required")]
        [MaxLength(255, ErrorMessage = "Eamil has a max length of 255 characters")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Must be a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(.{8,20}|[^0-9]*|[^A-Z])$", ErrorMessage = "Password do not meet our requirements")]
        public string Password { get; set; }
    }
}
