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

        public int ModelId { get; set; }
        public Model Model { get; set; }

        public ShoeImage ShoeImage { get; set; }

        public List<PotentialBuy> PotentialBuys { get; set; }
    }
}
