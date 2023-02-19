using System.ComponentModel.DataAnnotations;

namespace ShoelessJoeAPI.DataAccess.DataModels
{
    public class Model
    {
        [Key]
        public int ModelId { get; set; }

        [Required]
        [MaxLength(255)]
        public string ModelName { get; set; }

        public int ManufacterId { get; set; }
        public Manufacter Manufacter { get; set; }

        public List<Shoe> Shoes { get; set; }
    }
}
