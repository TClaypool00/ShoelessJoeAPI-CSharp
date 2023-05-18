using ShoelessJoeAPI.App.ApiModels.PostModels;
using ShoelessJoeAPI.Core.CoreModels;

namespace ShoelessJoeAPI.App.ApiModels.PartialModels
{
    public class ApiMultiPotentialBuyModel : PostPotentialBuyModel
    {
        public int PotentialBuyId { get; set; }

        public string ShoeImage { get; set; }

        public string ShoeSize { get; set; }

        public string UserName { get; set; }

        public string Brand { get; set; }



        public ApiMultiPotentialBuyModel(CorePotentialBuy potentialBuy)
        {
            PotentialBuyId = potentialBuy.PotentialBuyId;
            Brand = $"{potentialBuy.Shoe.Model.Manufacter.ManufacterName} {potentialBuy.Shoe.Model.ModelName}";
            UserId = potentialBuy.Shoe.Model.Manufacter.User.UserId;
            ShoeId = potentialBuy.Shoe.ShoeId;
            UserName = $"{potentialBuy.Shoe.Model.Manufacter.User.FirstName} {potentialBuy.Shoe.Model.Manufacter.User.LastName}";

            if (potentialBuy.Shoe.ShoeImage.RightShoeImage1 is not null || potentialBuy.Shoe.ShoeImage.RightShoeImage2 is not null)
            {
                ShoeSize = $"{potentialBuy.Shoe.RightSize} (R)";

                if (potentialBuy.Shoe.ShoeImage.RightShoeImage1 is not null)
                {
                    ShoeImage = potentialBuy.Shoe.ShoeImage.RightShoeImage1.FileName;
                    return;
                }

                if (potentialBuy.Shoe.ShoeImage.RightShoeImage2 is not null)
                {
                    ShoeImage = potentialBuy.Shoe.ShoeImage.RightShoeImage2.FileName;
                    return;
                }
            }

            if (potentialBuy.Shoe.ShoeImage.LeftShoeImage1 is not null || potentialBuy.Shoe.ShoeImage.LeftShoeImage2 is not null)
            {
                ShoeSize = $"{potentialBuy.Shoe.LeftSize} (L)";

                if (potentialBuy.Shoe.ShoeImage.LeftShoeImage1 is not null)
                {
                    ShoeImage = potentialBuy.Shoe.ShoeImage.LeftShoeImage1.FileName;
                    return;
                }

                if (potentialBuy.Shoe.ShoeImage.LeftShoeImage2 is not null)
                {
                    ShoeImage = potentialBuy.Shoe.ShoeImage.LeftShoeImage2.FileName;
                    return;
                }
            }
        }
    }
}
