using ShoelessJoeAPI.App.ApiModels;
using ShoelessJoeAPI.App.ApiModels.PartialModels;
using ShoelessJoeAPI.App.ApiModels.PostModels;
using ShoelessJoeAPI.App.ApiModels.UpdateModels;
using ShoelessJoeAPI.Core.CoreModels;
using ShoelessJoeAPI.Core.CoreModels.PartialModels;

namespace ShoelessJoeAPI.App
{
    public class ApiMapper
    {
        public static ApiUser MapUser(CoreUser user)
        {
            return new ApiUser
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumb = user.PhoneNumb,
                IsAdmin = user.IsAdmin
            };
        }

        public static CoreUser MapUser(RegisterModel user)
        {
            return new CoreUser
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumb = user.PhoneNumb,
                IsAdmin = false,
                Password = user.Password
            };
        }

        public static CoreUser MapUser(ApiUser user, bool showPassword = false)
        {
            return new CoreUser
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumb = user.PhoneNumb,
                IsAdmin = user.IsAdmin,
                Password = showPassword ? user.Password : ""
            };
        }

        public static AuthUser MapUser(CoreUser user, string token)
        {
            return new AuthUser
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumb = user.PhoneNumb,
                IsAdmin = user.IsAdmin,
                Token = token
            };
        }

        public static ApiManufacter MapManufacter(CoreManufacter manufacter)
        {
            return new ApiManufacter
            {
                ManufacterId = manufacter.ManufacterId,
                ManufacterName = manufacter.ManufacterName,
                UserId = manufacter.User.UserId,
                FirstName = manufacter.User.FirstName,
                LastName = manufacter.User.LastName,
            };
        }

        public static CoreManufacter MapManufacter(PostManufacter manufacter)
        {
            return new CoreManufacter
            {
                ManufacterName = manufacter.ManufacterName,
                UserId = manufacter.UserId
            };
        }

        public static ApiManufacterDropDown MapManufacterDropDown(CoreManufacter manufacter)
        {
            return new ApiManufacterDropDown
            {
                ManufacterId = manufacter.ManufacterId,
                ManufacterName = manufacter.ManufacterName
            };
        }

        public static ApiManufacterDropDown MapManufacter(CoreManufacterDropDown dropDown)
        {
            return new ApiManufacterDropDown
            {
                ManufacterId = dropDown.ManufacterId,
                ManufacterName = dropDown.ManufacterName
            };
        }

        public static ApiModel MapModel(CoreModel model)
        {
            return new ApiModel
            {
                ModelId = model.ModelId,
                ModelName = model.ModelName,
                ManufacterId = model.Manufacter.ManufacterId,
                ManufacterName = model.Manufacter.ManufacterName,
                UserId = model.Manufacter.User.UserId,
                FirstName = model.Manufacter.User.FirstName,
                LastName = model.Manufacter.User.LastName
            };
        }

        public static CoreModel MapModel(PostModel model, int id = 0)
        {
            var coreModel = new CoreModel
            {
                ModelName = model.ModelName,
                ManufacterId = model.ManufacterId
            };

            if (id != 0)
            {
                coreModel.ModelId = id;
            }

            return coreModel;
        }

        public static ApiModelDropDown MapModelDropDown(CoreModel model)
        {
            return new ApiModelDropDown
            {
                ModelId = model.ModelId,
                ModelName = model.ModelName
            };
        }

        public static ApiShoeModel MapShoe(CoreShoe shoe)
        {
            var apiShoe = new ApiShoeModel
            {
                ShoeId = shoe.ShoeId,
                LeftSize = shoe.LeftSize,
                RightSize = shoe.RightSize,
                //IsSold = shoe.IsSold,
                UserId = shoe.Model.Manufacter.User.UserId,
                FirstName = shoe.Model.Manufacter.User.FirstName,
                LastName = shoe.Model.Manufacter.User.LastName,
                ManufacterId = shoe.Model.Manufacter.ManufacterId,
                MaufacterName = shoe.Model.Manufacter.ManufacterName,
                ModelId = shoe.Model.ModelId,
                ModelName = shoe.Model.ModelName
            };

            return apiShoe;
        }

        public static CoreShoe MapShoe(PostShoeModel shoe, int id = 0)
        {
            var coreShoe = new CoreShoe
            {
                LeftSize = shoe.LeftSize,
                RightSize = shoe.RightSize,
                ModelId = shoe.ModelId
            };

            if (id != 0)
            {
                coreShoe.ShoeId = id;
            }

            return coreShoe;
        }

        public static CoreShoeImage MapShoeImage(PostShoeModel shoeModel)
        {
            return new CoreShoeImage
            {
                LeftShoeImage1 = shoeModel.LeftImage1,
                LeftShoeImage2 = shoeModel.LeftImage2,
                RightShoeImage1 = shoeModel.RightImage2,
                RightShoeImage2 = shoeModel.RightImage2
            };
        }

        public static ApiPotentialBuyModel MapPotentialBuy(CorePotentialBuy potentialBuy)
        {
            return new ApiPotentialBuyModel
            {
                PotentialBuyId = potentialBuy.PotentialBuyId,
                IsSold = potentialBuy.IsSold,
                ShoeImageId = potentialBuy.Shoe.ShoeImage.ShoeImageId,
                LeftShoeImage1 = potentialBuy.Shoe.ShoeImage.LeftShoeImage1Path,
                LeftShoeImage2 = potentialBuy.Shoe.ShoeImage.LeftShoeImage2Path,
                RightShoeImage1 = potentialBuy.Shoe.ShoeImage.RightShoeImage1Path,
                RightShoeImage2 = potentialBuy.Shoe.ShoeImage.RightShoeImage2Path,
                ShoeId = potentialBuy.Shoe.ShoeId,
                LeftSize = potentialBuy.Shoe.LeftSize,
                RightSize = potentialBuy.Shoe.RightSize,
                ModelId = potentialBuy.Shoe.Model.ModelId,
                ModelName = potentialBuy.Shoe.Model.ModelName,
                ManufacterId = potentialBuy.Shoe.Model.Manufacter.ManufacterId,
                ManufacterName = potentialBuy.Shoe.Model.Manufacter.ManufacterName,
                BuyerId = potentialBuy.PotentialBuyer.UserId,
                BuyerFirstName = potentialBuy.PotentialBuyer.FirstName,
                BuyerLastName = potentialBuy.PotentialBuyer.LastName,
                OwnerId = potentialBuy.Shoe.Model.Manufacter.User.UserId,
                OwnerFirstName = potentialBuy.Shoe.Model.Manufacter.User.FirstName,
                OwnerLastName = potentialBuy.Shoe.Model.Manufacter.User.LastName
            };
        }

        public static CorePotentialBuy MapPotentialBuy(PostPotentialBuyModel potentialBuy)
        {
            return new CorePotentialBuy
            {
                DateSoldDate = null,
                IsSold = false,
                PotentialBuyerUserId = potentialBuy.UserId,
                ShoeId = potentialBuy.ShoeId
            };
        }


        public static ApiCommentModel MapComment(CoreComment comment)
        {
            return new ApiCommentModel
            {
                CommentId = comment.CommentId,
                CommentText = comment.CommentText,
                Date = comment.DatePosted.ToString(),
                UserId = comment.User.UserId,
                FirstName = comment.User.FirstName,
                LastName = comment.User.LastName,
            };
        }

        public static CoreComment MapComment(PostCommentModel comment)
        {
            return new CoreComment
            {
                CommentText = comment.CommentText,
                UserId = comment.UserId,
                PotentialBuyId = comment.PotentialBuyId
            };
        }

        public static CoreComment MapComment(UpdateCommentModel comment, int id)
        {
            return new CoreComment
            {
                CommentId = id,
                CommentText = comment.CommentText
            };
        }
    }
}
