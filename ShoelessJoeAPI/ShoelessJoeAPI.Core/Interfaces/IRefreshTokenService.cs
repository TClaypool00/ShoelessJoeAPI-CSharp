using ShoelessJoeAPI.Core.CoreModels;

namespace ShoelessJoeAPI.Core.Interfaces
{
    public interface IRefreshTokenService
    {
        public Task AddRefreshTokenAsync(string refreshToken, DateTime dateExpired, int userId);

        public Task<CoreRefreshToken> GetRefreshTokn(string refreshToken);

        public Task<bool> RefreshTokenExists(string refreshToken);
    }
}
