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
                IsSold = shoe.IsSold,
                ModelId = shoe.ModelId,
                //SoldToId = shoe.SoldToId
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
                DatePosted = shoe.DatePosted,
                IsSold = shoe.IsSold,
            };

            if (shoe.Model is not null)
            {
                coreShoe.Model = MapModel(shoe.Model);
            }

            return coreShoe;
        }
    }
}
