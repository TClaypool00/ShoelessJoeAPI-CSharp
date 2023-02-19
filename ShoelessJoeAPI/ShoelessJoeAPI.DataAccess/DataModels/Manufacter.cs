using System.ComponentModel.DataAnnotations;

namespace ShoelessJoeAPI.DataAccess.DataModels
{
    public class Manufacter
    {
        [Key]
        public int ManufacterId { get; set; }

        [Required]
        [MaxLength(255)]
        public string ManufacterName { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public List<Model> Models { get; set; }
    }
}
