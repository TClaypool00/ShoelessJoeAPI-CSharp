using Microsoft.EntityFrameworkCore;
using ShoelessJoeAPI.Core.CoreModels;
using ShoelessJoeAPI.Core.Interfaces;
using ShoelessJoeAPI.DataAccess.DataModels;

namespace ShoelessJoeAPI.DataAccess.Services
{
    public class ShoeService : ServiceHelper, IShoeService
    {
        private readonly ShoelessJoeContext _context;

        public ShoeService(ShoelessJoeContext context)
        {
            _context = context;
        }

        public async Task AddShoeAsync(CoreShoe shoe)
        {
            var dataShoe = Mapper.MapShoe(shoe);

            await _context.Shoes.AddAsync(dataShoe);

            await SaveAsync();
        }

        public async Task<List<CoreShoe>> GetShoesAsync(int? ownerId = null, int? soldToId = null, DateTime? datePosted = null, bool? isSold = null, int? index = null)
        {
            ConfigureIndex(index);

            var shoes = new List<Shoe>();
            var coreShoes = new List<CoreShoe>();

            if (ownerId is null && soldToId is null && datePosted is null && isSold is null)
            {
                shoes = await FindDataShoe()
                .Take(10)
                .Skip(_index)
                .ToListAsync();
            }
            else
            {
                if (ownerId is not null)
                {
                    shoes = await FindDataShoe()
                    .Take(10)
                    .Skip(_index)
                    .Where(a => a.Model.Manufacter.User.UserId == ownerId)
                    .ToListAsync();
                }

                if (soldToId is not null)
                {
                    shoes = shoes.Where(a => a.Model.Manufacter.UserId == soldToId).ToList();
                }

                if (datePosted is not null)
                {
                    shoes = shoes.Where(b => b.DatePosted ==  datePosted).ToList();
                }

                if (isSold is not null)
                {
                    shoes = shoes.Where(c => c.IsSold == isSold).ToList();
                }

                if (shoes.Count > 0)
                {
                    for (int i = 0; i < shoes.Count; i++)
                    {
                        coreShoes.Add(Mapper.MapShoe(shoes[i]));
                    }
                }                
            }

            return coreShoes;
        }

        public async Task<CoreShoe> GetShoesAsync(int id)
        {
            var dataShoe = await FindDataShoe().FirstOrDefaultAsync(s => s.ShoeId == id);

            return Mapper.MapShoe(dataShoe);
        }

        public Task<bool> ShoeExistsById(int id)
        {
            return _context.Shoes.AnyAsync(s => s.ShoeId == id);
        }

        public Task<bool> ShoeIsOwnedByUser(int id, int ownerId)
        {
            return _context.Shoes.AnyAsync(s => s.ShoeId == id && s.Model.Manufacter.UserId == ownerId);
        }

        public async Task<CoreShoe> UpdateShoeAsync(CoreShoe shoe, int id)
        {
            var dataShoe = Mapper.MapShoe(shoe);

            _context.Shoes.Update(dataShoe);

            await SaveAsync();

            return shoe;
        }

        public async Task SellShoeAsync(int id, int soldToUserId)
        {
            var dataShoe = await _context.Shoes.FindAsync(id);
            dataShoe.IsSold = true;
            dataShoe.SoldToUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == soldToUserId);

            _context.Shoes.Update(dataShoe);

            await SaveAsync();
        }

        public bool ShoeIsAlreadySold(int id)
        {
            return _context.Shoes
                .FirstOrDefaultAsync(a => a.ShoeId == id).Result.IsSold;
        }

        private async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        private IQueryable<Shoe> FindDataShoe()
        {
            return _context.Shoes
                .Select(a => new Shoe
                {
                    ShoeId = a.ShoeId,
                    LeftSize = a.LeftSize,
                    RightSize = a.RightSize,
                    IsSold = a.IsSold,
                    DatePosted = a.DatePosted,
                    Model = new Model
                    {
                        ModelId = a.ModelId,
                        ModelName = a.Model.ModelName,
                        Manufacter = new Manufacter
                        {
                            ManufacterId = a.Model.ManufacterId,
                            ManufacterName = a.Model.Manufacter.ManufacterName,
                            User = new User
                            {
                                UserId = a.Model.Manufacter.UserId,
                                FirstName = a.Model.Manufacter.User.FirstName,
                                LastName = a.Model.Manufacter.User.LastName
                            }
                        }
                    }
                });
        }        
    }
}
