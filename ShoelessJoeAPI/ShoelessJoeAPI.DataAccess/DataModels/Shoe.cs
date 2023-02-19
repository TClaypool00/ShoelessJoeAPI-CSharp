using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoelessJoeAPI.DataAccess.DataModels
{
    public class Shoe
    {
        [Key]
        public int ShoeId { get; set; }

        [Range(1, 20)]
        public double? RightSize { get; set; } = null;

        [Range(1, 20)]
        public double? LeftSize { get; set; } = null;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DatePosted { get; set; }

        [Required]
        public bool IsSold { get; set; } = false;

        public int ModelId { get; set; }
        public Model Model { get; set; }

        public int? SoldToId { get; set; } = null;
        public User SoldToUser { get; set; }
    }
}
