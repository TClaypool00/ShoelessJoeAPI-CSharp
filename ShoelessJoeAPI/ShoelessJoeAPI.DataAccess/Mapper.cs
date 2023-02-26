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
    }
}
