using Microsoft.EntityFrameworkCore;
using ShoelessJoeAPI.Core.CoreModels;
using ShoelessJoeAPI.Core.Interfaces;
using ShoelessJoeAPI.DataAccess.DataModels;

namespace ShoelessJoeAPI.DataAccess.Services
{
    public class UserService : IUserService
    {
        private readonly ShoelessJoeContext _context;

        public UserService(ShoelessJoeContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(CoreUser user)
        {
            await _context.Users.AddAsync(Mapper.MapUser(user));
            await SaveAsync();
        }

        public async Task<CoreUser> GetUserByEmailAsync(string email)
        {
            var dataUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            return Mapper.MapUser(dataUser, true);
        }

        public async Task<CoreUser> GetUserByIdAsync(int id)
        {
            var dataUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);

            return Mapper.MapUser(dataUser);
        }

        public async Task<List<CoreUser>> GetUsersAsync()
        {
            var dataUsers = await _context.Users.ToListAsync();
            var coreUsers = new List<CoreUser>();

            for (int i = 0; i < dataUsers.Count; i++)
            {
                coreUsers.Add(Mapper.MapUser(dataUsers[i]));
            }

            return coreUsers;

        }

        private async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(CoreUser user, int id)
        {
            var dataUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            var userToUpdate = Mapper.MapUser(user);

            userToUpdate.Password = dataUser.Password;
            userToUpdate.IsAdmin = dataUser.IsAdmin;

            _context.Entry(dataUser).CurrentValues.SetValues(userToUpdate);

            await SaveAsync();
        }

        public Task<bool> UserExistsByEmailAsync(string email, int? userId = null)
        {
            if (userId is null)
            {
                return _context.Users.AnyAsync(u => u.Email == email);
            }

            return _context.Users.AnyAsync(u => u.Email == email && u.UserId != userId);
        }

        public Task<bool> UserExistsByIdAsync(int id)
        {
            return _context.Users.AnyAsync(u => u.UserId == id);
        }

        public Task<bool> UserExistsByPhoneNumbAsync(string phoneNumb, int? userId = null)
        {
            if (userId is null)
            {
                return _context.Users.AnyAsync(u => u.PhoneNumb == phoneNumb);
            }

             return _context.Users.AnyAsync(u => u.PhoneNumb == phoneNumb && u.UserId != userId);
        }
    }
}
