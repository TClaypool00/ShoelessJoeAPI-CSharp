using ShoelessJoeAPI.Core.CoreModels;

namespace ShoelessJoeAPI.App.ApiModels.MultiModel
{
    public class MultiShoeModel
    {
        public MultiShoeModel()
        {
            
        }

        public MultiShoeModel(CoreShoeImage image)
        {
            if (image.LeftShoeImage1Path != "")
            {
                PicturePath = image.LeftShoeImage1Path;
                return;
            }

            if (PicturePath == "" && image.LeftShoeImage2Path != "")
            {
                PicturePath += image.LeftShoeImage2Path;
                return;
            }

            if (PicturePath == "" && image.RightShoeImage1Path != "")
            {
                PicturePath = image.RightShoeImage2Path;
                return;
            }

            if (PicturePath == "" && image.RightShoeImage2Path != "")
            {
                PicturePath = image.RightShoeImage2Path;
                return;
            }
        }

        public int ShoeId { get; set; }

        public bool BothShoes { get; set; }

        public string ModelName { get; set; }

        public string ManufacterName { get; set; }

        public string PicturePath { get; set; }

        public string DatePosted { get; set; }


        public string OwnerFirstName { get; set; }

        public string OwnerLastName { get; set; }
    }
}
