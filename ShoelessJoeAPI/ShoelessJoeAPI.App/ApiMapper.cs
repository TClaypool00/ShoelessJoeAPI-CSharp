using ShoelessJoeAPI.App.ApiModels;
using ShoelessJoeAPI.Core.CoreModels;
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
    }
}
