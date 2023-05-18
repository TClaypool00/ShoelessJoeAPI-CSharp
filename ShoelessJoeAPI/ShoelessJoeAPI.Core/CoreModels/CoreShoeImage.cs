using Microsoft.AspNetCore.Http;
using System.Drawing;

namespace ShoelessJoeAPI.Core.CoreModels
{
    public class CoreShoeImage
    {
        public int ShoeImageId { get; set; }

        public IFormFile RightShoeImage1 { get; set; }

        public IFormFile RightShoeImage2 { get; set; }

        public IFormFile LeftShoeImage1 { get; set; }

        public IFormFile LeftShoeImage2 { get; set; }

        public string RightShoeImage1Path { get; set; }

        public string RightShoeImage2Path { get; set; }

        public string LeftShoeImage1Path { get; set; }

        public string LeftShoeImage2Path { get; set; }

        public int ShoeId { get; set; }
        public CoreShoe Shoe { get; set; }
    }
}
