using System.ComponentModel.DataAnnotations;

namespace ShoelessJoeAPI.App.ApiModels.PostModels
{
    public class PostShoeModel
    {
        public double? LeftSize { get; set; }

        public double? RightSize { get; set;}

        [Required(ErrorMessage = "Model Id is required")]
        public int ModelId { get; set; }

        public bool BothSizesAreNull()
        {
            return (LeftSize is null && RightSize is null) || (LeftSize == 0 && RightSize == 0);
        }
    }
}
