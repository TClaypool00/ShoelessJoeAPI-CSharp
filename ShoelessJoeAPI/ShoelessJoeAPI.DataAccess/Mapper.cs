using ShoelessJoeAPI.Core.CoreModels;
using ShoelessJoeAPI.Core.CoreModels.PartialModels;
using ShoelessJoeAPI.DataAccess.DataModels;

namespace ShoelessJoeAPI.DataAccess
{
    public class Mapper
    {
        public static User MapUser(CoreUser user)
        {
            return new User
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                PhoneNumb = user.PhoneNumb,
                IsAdmin = user.IsAdmin
            };
        }

        public static CoreUser MapUser(User user, bool showPassword = false)
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

        public static Manufacter MapManufacter(CoreManufacter manufacter)
        {
            var dataManufacter = new Manufacter
            {
                ManufacterName = manufacter.ManufacterName,
                UserId = manufacter.UserId,                
            };

            if (manufacter.ManufacterId > 0)
            {
                dataManufacter.ManufacterId = manufacter.ManufacterId;
            }

            return dataManufacter;
        }

        public static CoreManufacter MapManufacter(Manufacter manufacter, CoreUser user = null)
        {
            var coreManufacter = new CoreManufacter
            {
                ManufacterId = manufacter.ManufacterId,
                ManufacterName = manufacter.ManufacterName                
            };

            if (user is not null)
            {
                coreManufacter.User = user;
            }
            else if (manufacter.User is not null)
            {
                coreManufacter.User = MapUser(manufacter.User);
            }

            return coreManufacter;
        }

        public static CoreManufacterDropDown MapManufacterDropDown(Manufacter manufacter)
        {
            return new CoreManufacterDropDown
            {
                ManufacterId = manufacter.ManufacterId,
                ManufacterName = manufacter.ManufacterName
            };
        }

        public static Model MapModel(CoreModel model)
        {
            var dataModel = new Model
            {
                ModelName = model.ModelName,
                ManufacterId = model.ManufacterId
            };

            if (dataModel.ModelId != 0)
            {
                dataModel.ModelId = model.ModelId;
            }

            return dataModel;
        }

        public static CoreModel MapModel(Model model, out CoreManufacter manufacter, out CoreUser user)
        {
            manufacter = null;
            user = null;

            var coreModel = new CoreModel
            {
                ModelId = model.ModelId,
                ModelName = model.ModelName               
            };

            if (manufacter is not null)
            {
                coreModel.Manufacter = manufacter;
            }
            else
            {
                coreModel.Manufacter = MapManufacter(model.Manufacter);

                if (user is null)
                {
                    user = coreModel.Manufacter.User;
                }
                else
                {
                    coreModel.Manufacter.User = user;
                }
            }

            return coreModel;
        }

        public static CoreModel MapModel(Model model, CoreManufacter manufacter = null)
        {
            var coreModel = new CoreModel
            {
                ModelId = model.ModelId,
                ModelName = model.ModelName
            };

            if (model.Manufacter is not null)
            {
                coreModel.Manufacter = MapManufacter(model.Manufacter);
            } else if (manufacter != null)
            {
                coreModel.Manufacter = manufacter;
            }

            return coreModel;
        }

        public static CoreModelDropDown MapModelDropDown(Model model)
        {
            return new CoreModelDropDown
            {
                ModelId = model.ModelId,
                ModelName = model.ModelName
            };
        }

        public static Shoe MapShoe(CoreShoe shoe)
        {
            var dataShoe = new Shoe
            {
                LeftSize = shoe.LeftSize,
                RightSize = shoe.RightSize,
                DatePosted = shoe.DatePosted,
                ModelId = shoe.ModelId
            };

            if (shoe.ShoeId != 0)
            {
                dataShoe.ShoeId = shoe.ShoeId;
            }

            return dataShoe;
        }

        public static CoreShoe MapShoe(Shoe shoe)
        {
            var coreShoe = new CoreShoe
            {
                ShoeId = shoe.ShoeId,
                LeftSize = shoe.LeftSize,
                RightSize = shoe.RightSize,
                DatePosted = shoe.DatePosted
            };

            if (shoe.Model is not null)
            {
                coreShoe.Model = MapModel(shoe.Model);
            }

            if (shoe.ShoeImage is not null)
            {
                coreShoe.ShoeImage = MapShoeImage(shoe.ShoeImage);
            }

            return coreShoe;
        }

        public static PotentialBuy MapPotentialBuy(CorePotentialBuy potentialBuy)
        {
            var dataPotentialBuy = new PotentialBuy
            {
                ShoeId = potentialBuy.ShoeId,
                PotentialBuyerUserId = potentialBuy.PotentialBuyerUserId,
                IsSold = potentialBuy.IsSold,
                DateSold = potentialBuy.DateSoldDate
            };

            if (potentialBuy.PotentialBuyId != 0)
            {
                dataPotentialBuy.PotentialBuyId = potentialBuy.PotentialBuyId;
            }

            return dataPotentialBuy;
        }

        public static CorePotentialBuy MapPotentialBuy(PotentialBuy potentialBuy)
        {
            var corePotentialBuy = new CorePotentialBuy
            {
                PotentialBuyId = potentialBuy.PotentialBuyId,
                DateSoldDate = potentialBuy.DateSold,
                IsSold = potentialBuy.IsSold,
            };

            if (potentialBuy.PotentialBuyer is not null)
            {
                corePotentialBuy.PotentialBuyer = MapUser(potentialBuy.PotentialBuyer);
            }

            if (potentialBuy.Shoe is not null)
            {
                corePotentialBuy.Shoe = MapShoe(potentialBuy.Shoe);
            }

            return corePotentialBuy;
        }

        public static CorePotentialBuy MapPotentialBuy(PotentialBuy potentialBuy, out CoreUser potentialBuyer, out Shoe shoe)
        {
            potentialBuyer = null;
            shoe = null;

            var corePotentialBuy = new CorePotentialBuy
            {
                PotentialBuyId = potentialBuy.PotentialBuyId,
                DateSoldDate = potentialBuy.DateSold,
                IsSold = potentialBuy.IsSold,
            };

            if (potentialBuy.PotentialBuyer is not null)
            {
                corePotentialBuy.PotentialBuyer = MapUser(potentialBuy.PotentialBuyer);
            }

            if (potentialBuy.Shoe is not null)
            {
                corePotentialBuy.Shoe = MapShoe(potentialBuy.Shoe);
            }

            return corePotentialBuy;
        }

        public static CoreShoeImage MapShoeImage(ShoeImage shoeImage)
        {
            return new CoreShoeImage
            {
                ShoeImageId = shoeImage.ShoeImageId,
                LeftShoeImage1Path = shoeImage.LeftShoeImage1,
                LeftShoeImage2Path = shoeImage.LeftShoeImage2,
                RightShoeImage1Path = shoeImage.RightShoeImage1,
                RightShoeImage2Path= shoeImage.RightShoeImage2
            };
        }

        public static ShoeImage MapShoeImage(CoreShoeImage shoeImage)
        {
            var dataShoeImage = new ShoeImage
            {
                LeftShoeImage1 = shoeImage.LeftShoeImage1Path,
                LeftShoeImage2 = shoeImage.LeftShoeImage2Path,
                RightShoeImage1 = shoeImage.RightShoeImage1Path,
                RightShoeImage2 = shoeImage.RightShoeImage2Path
            };

            if (shoeImage.ShoeImageId != 0)
            {
                dataShoeImage.ShoeImageId = shoeImage.ShoeImageId;
            }
            return dataShoeImage;
        }

        public static CoreComment MapComment(Comment comment)
        {
            var coreComment = new CoreComment
            {
                CommentId = comment.CommentId,
                CommentText = comment.CommentText,
                DatePosted = comment.DatePosted,
            };

            if (comment.User != null)
            {
                coreComment.User = MapUser(comment.User);
            }

            return coreComment;
        }

        public static Comment MapComment(CoreComment comment)
        {
            var dataComment = new Comment
            {
                CommentText = comment.CommentText,
                DatePosted = comment.DatePosted,
                PotentialBuyId = comment.PotentialBuyId,
                UserId = comment.UserId
            };

            if (comment.CommentId != 0)
            {
                dataComment.CommentId = comment.CommentId;
            }

            return dataComment;
        }
    }
}
