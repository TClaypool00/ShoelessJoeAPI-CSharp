﻿using ShoelessJoeAPI.Core.CoreModels;

namespace ShoelessJoeAPI.Core.Interfaces
{
    public interface IShoeService
    {
        public Task<List<CoreShoe>> GetShoesAsync(int? ownerId = null, int? soldToId = null, DateTime? datePosted = null, bool? isSold = null, int? index = null);

        public Task<CoreShoe> GetShoesAsync(int id);

        public Task AddShoeAsync(CoreShoe shoe);

        public Task SellShoeAsync(int id, int soldToUserId);

        public Task<CoreShoe> UpdateShoeAsync(CoreShoe shoe, int id);

        public Task<bool> ShoeExistsById(int id);

        public Task<bool> ShoeIsOwnedByUser(int id, int owner);

        public bool ShoeIsAlreadySold(int id);
    }
}
