using ShoelessJoeAPI.Core.CoreModels;

namespace ShoelessJoeAPI.Core.Interfaces
{
    public interface ICommentService
    {
        public Task<List<CoreComment>> GetCommentsAsync(int? potentialBuyId = null, int? shoeId = null, int? userId = null, int? index = null);

        public Task<CoreComment> GetCommentAsync(int id);

        public Task<CoreComment> AddCommentAsync(CoreComment comment);

        public Task<CoreComment> UpdateCommentAsync(CoreComment comment);

        public Task<bool> CommentExistsByIdAsync(int id);

        public Task<bool> CommentOwnedByUserAsync(int id, int userId);

        public Task DeleteCommentAsync(int id);
    }
}
