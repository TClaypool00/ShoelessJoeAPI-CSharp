using ShoelessJoeAPI.Core.CoreModels;
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

        public static CoreUser MapUser(User user)
        {
            return new CoreUser
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumb = user.PhoneNumb,
                IsAdmin = user.IsAdmin
            };
        }
    }
}
