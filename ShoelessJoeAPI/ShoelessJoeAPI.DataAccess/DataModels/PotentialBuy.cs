using System.ComponentModel.DataAnnotations;

namespace ShoelessJoeAPI.DataAccess.DataModels
{
    public class PotentialBuy
    {
        [Key]
        public int PotentialBuyId { get; set; }

        [Required]
        public int ShoeId { get; set; }
        public Shoe Shoe { get; set; }

        [Required]
        public int PotentialBuyerUserId { get; set; }
        public User PotentialBuyer { get; set; }

        [Required]
        public bool IsSold { get; set; }

        public DateTime? DateSold { get; set; } = null;

        public List<Comment> Comments { get; set; }
    }
}
