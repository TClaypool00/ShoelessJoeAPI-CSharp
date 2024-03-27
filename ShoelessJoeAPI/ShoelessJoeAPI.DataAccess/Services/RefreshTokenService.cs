using Microsoft.EntityFrameworkCore;
using ShoelessJoeAPI.Core.CoreModels;
using ShoelessJoeAPI.Core.Interfaces;
using ShoelessJoeAPI.DataAccess.DataModels;

namespace ShoelessJoeAPI.DataAccess.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly ShoelessJoeContext _context;

        public RefreshTokenService(ShoelessJoeContext context)
        {
            _context = context;
        }

        public async Task AddRefreshTokenAsync(string refreshToken, DateTime dateExpired, int userId)
        {
            var dataRefreshToken = new RefreshToken(refreshToken, dateExpired, userId);

            await _context.AddAsync(dataRefreshToken);

            await _context.SaveChangesAsync();
        }

        public async Task<CoreRefreshToken> GetRefreshTokn(string refreshToken)
        {
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(r => r.Token == refreshToken);

            return Mapper.MapRefreshToken(token);
        }

        public Task<bool> RefreshTokenExists(string refreshToken)
        {
            return _context.RefreshTokens.AnyAsync(r => r.Token == refreshToken);
        }
    }
}
