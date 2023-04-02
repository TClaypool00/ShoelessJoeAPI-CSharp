using System.Drawing;

namespace ShoelessJoeAPI.Core.CoreModels
{
    public class CoreShoeImage
    {
        public int ShoeImageId { get; set; }

        public string RightShoeImage1 { get; set; }

        public string RightShoeImage2 { get; set; }

        public string LeftShoeImage1 { get; set; }

        public string LeftShoeImage2 { get; set; }

        public int ShoeId { get; set; }
        public CoreShoe Shoe { get; set; }
    }
}
