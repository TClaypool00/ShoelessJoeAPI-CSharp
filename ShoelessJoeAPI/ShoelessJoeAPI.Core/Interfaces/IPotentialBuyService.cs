using ShoelessJoeAPI.Core.CoreModels;

namespace ShoelessJoeAPI.Core.Interfaces
{
    public interface IPotentialBuyService
    {
        public Task<List<CorePotentialBuy>> GetPotentialBuysAsync(int? userId = null, int? shoeId = null, bool? isSold = null, DateTime? dateSold = null, int? index = null);

        public Task<CorePotentialBuy> GetPotentialBuyByIdAsync(int id);

        public Task<CorePotentialBuy> AddPotentialBuyAsync(CorePotentialBuy potentialBuy);

        public Task<bool> PotentialBuyExistsByIdAsync(int id);

        public Task<bool> PotentialBuyExistsByUserIdAsync(int userId, int shoeId);

        public Task<bool> UserHasAccessToPotentialBuy(int userId, int id);

        public Task SellShoeAsync(int id, int userId);

        public bool IsShoeSoldAsync(int shoeId, int userId);

        public Task<bool> IsShoeSoldByCommentId(int commentId, int userId);

        public Task<bool> IsShoeSoldByPotentialBuyId(int id);

    }
}
