using System.ComponentModel.DataAnnotations;

namespace ShoelessJoeAPI.App.ApiModels
{
    public class ApiUser : LoginModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "First name is requried")]
        [MaxLength(255, ErrorMessage = "First name has a max length of 255 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(ErrorMessage = "Last name has a max length of 255 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Must be a valid phone number")]
        public string PhoneNumb { get; set; }

        public bool IsAdmin { get; set; }
    }
}
