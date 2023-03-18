using System.ComponentModel.DataAnnotations;

namespace ShoelessJoeAPI.DataAccess.DataModels
{
    public class ShoeImage
    {
        [Key]
        public int ShoeImageId { get; set; }

        public byte[] RightShoeImage1 { get; set; }

        public byte[] RightShoeImage2 { get; set; }

        public byte[] LeftShoeImage1 { get; set; }

        public byte[] LeftShoeImage2 { get; set;  }

        public int ShoeId { get; set; }
        public Shoe Shoe { get; set; }
    }
}
