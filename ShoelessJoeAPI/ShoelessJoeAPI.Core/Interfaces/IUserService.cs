using ShoelessJoeAPI.Core.CoreModels;

namespace ShoelessJoeAPI.Core.Interfaces
{
    public interface IUserService
    {
        public Task<List<CoreUser>> GetUsersAsync();

        public Task<CoreUser> GetUserByIdAsync(int id);

        public Task<CoreUser> GetUserByEmailAsync(string email);

        public Task AddUserAsync(CoreUser user);

        public Task UpdateUserAsync(CoreUser user, int id);

        public Task<bool> UserExistsByIdAsync(int id);

        public Task<bool> UserExistsByEmailAsync(string email, int? userId = null);

        public Task<bool> UserExistsByPhoneNumbAsync(string phoneNumb, int? userId = null);        
    }
}
