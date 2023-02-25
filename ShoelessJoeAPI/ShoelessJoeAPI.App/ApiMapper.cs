using ShoelessJoeAPI.App.ApiModels;
using ShoelessJoeAPI.App.ApiModels.PartialModels;
using ShoelessJoeAPI.App.ApiModels.PostModels;
using ShoelessJoeAPI.Core.CoreModels;
using ShoelessJoeAPI.Core.CoreModels.PartialModels;
using ShoelessJoeAPI.DataAccess.DataModels;

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
    }
}
