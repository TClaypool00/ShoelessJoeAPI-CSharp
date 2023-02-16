using ShoelessJoeAPI.Core.CoreModels;

namespace ShoelessJoeAPI.Core.Interfaces
{
    public interface IUserService
    {
        public Task<List<CoreUser>> GetUsersAsync();

        public Task<CoreUser> GetUserByIdAsync(int id);

        public Task<CoreUser> GetUserByEmailAsync(string email);

        public Task<bool> AddUserAsync(CoreUser user);

        public Task<bool> UpdateUserAsync(CoreUser user, int id);

        public Task<bool> UserExistsByIdAsync(int id);

        public Task<bool> UserExistsByEmailAsync(string email);

        public Task<bool> UserExistsByPhoneNumbAsync(string phoneNumb);

        public Task SaveAsync();
    }
}
