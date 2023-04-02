using System.ComponentModel.DataAnnotations;

namespace ShoelessJoeAPI.DataAccess.DataModels
{
    public class ShoeImage
    {
        [Key]
        public int ShoeImageId { get; set; }

        [MaxLength(255)]
        public string RightShoeImage1 { get; set; }

        [MaxLength(255)]
        public string RightShoeImage2 { get; set; }

        [MaxLength(255)]
        public string LeftShoeImage1 { get; set; }

        [MaxLength(255)]
        public string LeftShoeImage2 { get; set;  }

        public int ShoeId { get; set; }
        public Shoe Shoe { get; set; }
    }
}
