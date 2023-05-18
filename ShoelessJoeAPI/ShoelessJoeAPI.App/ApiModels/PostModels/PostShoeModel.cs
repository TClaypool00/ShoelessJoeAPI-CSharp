using System.ComponentModel.DataAnnotations;

namespace ShoelessJoeAPI.App.ApiModels.PostModels
{
    public class PostShoeModel
    {
        public double? LeftSize { get; set; }

        public double? RightSize { get; set;}

        [Required(ErrorMessage = "Model Id is required")]
        public int ModelId { get; set; }

        public IFormFile LeftImage1 { get; set; }

        public IFormFile LeftImage2 { get; set; }

        public IFormFile RightImage1 { get; set; }

        public IFormFile RightImage2 { get; set; }


        public bool BothSizesAreNull()
        {
            return (LeftSize is null && RightSize is null) || (LeftSize == 0 && RightSize == 0);
        }

        public bool ValidateLeftShoe()
        {
            return (LeftSize is null && (LeftImage1 is not null || LeftImage2 is not null)) || (LeftSize is not null && (LeftImage1 is null || LeftImage2 is null));
        }

        public bool ValidateRightShoe()
        {
            return (RightSize is null && (RightImage1 is not null || RightImage2 is not null)) || (RightSize is not null && (RightImage1 is null || RightImage2 is null));
        }
    }
}
