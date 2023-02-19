using System.ComponentModel.DataAnnotations;

namespace ShoelessJoeAPI.DataAccess.DataModels
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(255)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        [Required]
        [MaxLength(10)]
        public string PhoneNumb { get; set; }
        public bool IsAdmin { get; set; } = false;

        public List<Shoe> SoldToShoes { get; set; }
        public List<Manufacter> Manufacters { get; set; }
    }
}
