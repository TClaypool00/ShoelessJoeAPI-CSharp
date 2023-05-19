using Microsoft.EntityFrameworkCore;
using ShoelessJoeAPI.Core.CoreModels;
using ShoelessJoeAPI.Core.Interfaces;
using ShoelessJoeAPI.DataAccess.DataModels;

namespace ShoelessJoeAPI.DataAccess.Services
{
    public class PotentialBuyService : ServiceHelper, IPotentialBuyService
    {
        private readonly ShoelessJoeContext _context;
        private List<PotentialBuy> _potentialBuys;

        public PotentialBuyService(ShoelessJoeContext context)
        {
            _context = context;
        }

        public async Task<CorePotentialBuy> AddPotentialBuyAsync(CorePotentialBuy potentialBuy)
        {
            var dataPotentialBuy = Mapper.MapPotentialBuy(potentialBuy);

            await _context.PotentialBuys.AddAsync(dataPotentialBuy);

            await SaveAsync();

            potentialBuy.PotentialBuyId = dataPotentialBuy.PotentialBuyId;

            if (potentialBuy.PotentialBuyerUserId == 0)
            {
                throw new ArgumentException("Potential Buy could not be added");
            }

            dataPotentialBuy = await GetDataPotentialBuy()
                .FirstOrDefaultAsync(s => s.PotentialBuyId == potentialBuy.PotentialBuyId);

            potentialBuy = Mapper.MapPotentialBuy(dataPotentialBuy);

            return potentialBuy;
        }

        public async Task<CorePotentialBuy> GetPotentialBuyByIdAsync(int id)
        {
            var dataPotentialBuy = await GetDataPotentialBuy().FirstOrDefaultAsync(s => s.PotentialBuyId == id);

            var corePotentialBuy = Mapper.MapPotentialBuy(dataPotentialBuy);

            if ((await _context.Comments.AnyAsync(a => a.PotentialBuyId == id) && (corePotentialBuy.Comments is null)))
            {
                corePotentialBuy.Comments = new List<CoreComment>();
            }

            return corePotentialBuy;
        }

        public async Task<List<CorePotentialBuy>> GetPotentialBuysAsync(int? userId = null, int? shoeId = null, bool? isSold = null, DateTime? dateSold = null, int? index = null)
        {
            ConfigureIndex(index);
            _potentialBuys = null;

            var corePotentialBuys = new List<CorePotentialBuy>();

            if (userId is null && shoeId is not null && isSold is not null && dateSold is null)
            {
                _potentialBuys = await GetDataPotentialBuys()
                    .ToListAsync();
            } else
            {
                if (userId is not null)
                {
                    _potentialBuys = await GetDataPotentialBuys()
                        .Where(m => m.Shoe.Model.Manufacter.User.UserId == userId)
                        .ToListAsync();
                }

                if (shoeId is not null)
                {
                    if (ListIsEmpty())
                    {
                        _potentialBuys = await GetDataPotentialBuys()
                            .Where(m => m.Shoe.Model.Manufacter.User.UserId == userId)
                            .ToListAsync();
                    }
                    else
                    {
                        _potentialBuys = _potentialBuys.Where(m => m.Shoe.Model.Manufacter.User.UserId == userId).ToList();
                    }
                }

                if (isSold is not null)
                {
                    if (ListIsEmpty())
                    {
                        _potentialBuys = await GetDataPotentialBuys()
                            .Where(m => m.IsSold == isSold)
                            .ToListAsync();
                    }
                    else
                    {
                        _potentialBuys = _potentialBuys.Where(m => m.IsSold == isSold).ToList();
                    }
                }

                if (dateSold is null && isSold is not null && (bool)!isSold)
                {
                    if (ListIsEmpty())
                    {
                        _potentialBuys = await GetDataPotentialBuys()
                        .Where(m => m.DateSold == null && m.IsSold == false)
                        .ToListAsync();
                    }
                    else
                    {
                        _potentialBuys = _potentialBuys
                        .Where(m => m.DateSold == null && m.IsSold == false)
                        .ToList();
                    }
                } else if (dateSold is not null)
                {
                    if (ListIsEmpty())
                    {
                        _potentialBuys = await GetDataPotentialBuys()
                        .Where(m => m.DateSold == dateSold)
                        .ToListAsync();
                    }
                    else
                    {
                        _potentialBuys = _potentialBuys
                        .Where(m => m.DateSold == dateSold)
                        .ToList();
                    }
                }
            }

            if (!ListIsEmpty() && _potentialBuys.Count > 0)
            {
                for (int i = 0; i < _potentialBuys.Count; i++)
                {
                    corePotentialBuys.Add(Mapper.MapPotentialBuy(_potentialBuys[i]));
                }
            }

            return corePotentialBuys;
        }

        public bool IsShoeSoldAsync(int shoeId, int userId)
        {
            return _context.PotentialBuys.FirstOrDefaultAsync(s => s.ShoeId == shoeId && s.PotentialBuyerUserId == userId).Result.IsSold;
        }

        public Task<bool> PotentialBuyExistsByIdAsync(int id)
        {
            return _context.PotentialBuys.AnyAsync(s => s.PotentialBuyId == id);
        }

        public Task<bool> UserHasAccessToPotentialBuy(int userId, int id)
        {
            return _context.PotentialBuys.AnyAsync(s => s.PotentialBuyId == id && (s.Shoe.Model.Manufacter.UserId == userId || s.PotentialBuyer.UserId == userId));
        }

        private async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        private IQueryable<PotentialBuy> GetDataPotentialBuy()
        {
            return _context.PotentialBuys
                .Select(s => new PotentialBuy
                {
                    PotentialBuyId = s.PotentialBuyId,
                    IsSold = s.IsSold,
                    DateSold = s.DateSold == null ? DateTime.Now : s.DateSold.Value,
                    PotentialBuyer = new User
                    {
                        UserId = s.PotentialBuyer.UserId,
                        FirstName = s.PotentialBuyer.FirstName,
                        LastName = s.PotentialBuyer.LastName

                    },
                    Shoe = new Shoe
                    {
                        ShoeId = s.Shoe.ShoeId,
                        RightSize = s.Shoe.RightSize,
                        LeftSize = s.Shoe.LeftSize,
                        DatePosted = s.Shoe.DatePosted,
                        Model = new Model
                        {
                            ModelId = s.Shoe.Model.ModelId,
                            ModelName = s.Shoe.Model.ModelName,
                            Manufacter = new Manufacter
                            {
                                ManufacterId = s.Shoe.Model.Manufacter.ManufacterId,
                                ManufacterName = s.Shoe.Model.Manufacter.ManufacterName,
                                User = new User
                                {
                                    UserId = s.Shoe.Model.Manufacter.User.UserId,
                                    FirstName = s.Shoe.Model.Manufacter.User.FirstName,
                                    LastName = s.Shoe.Model.Manufacter.User.LastName
                                }
                            }
                        },
                        ShoeImage = new ShoeImage
                        {
                            ShoeImageId = s.Shoe.ShoeImage.ShoeImageId,
                            LeftShoeImage1 = s.Shoe.ShoeImage.LeftShoeImage1 ?? "",
                            LeftShoeImage2 = s.Shoe.ShoeImage.LeftShoeImage2 ?? "",
                            RightShoeImage1 = s.Shoe.ShoeImage.RightShoeImage1 ?? "",
                            RightShoeImage2 = s.Shoe.ShoeImage.RightShoeImage2 ?? ""
                        }
                    }

                });
        }

        private IQueryable<PotentialBuy> GetDataPotentialBuys()
        {
            return GetDataPotentialBuy()
                .Take(10)
                .Skip(_index);
        }

        private bool ListIsEmpty()
        {
            return _potentialBuys is null;
        }

        public Task<bool> PotentialBuyExistsByUserIdAsync(int userId, int shoeId)
        {
            return _context.PotentialBuys.AnyAsync(s => s.Shoe.ShoeId == shoeId && s.PotentialBuyer.UserId == userId);
        }

        public async Task SellShoeAsync(int id, int userId)
        {
            var dataShoe = await _context.PotentialBuys.FirstOrDefaultAsync(s => s.ShoeId == id && s.PotentialBuyerUserId == userId);
            dataShoe.IsSold = true;
            dataShoe.DateSold = DateTime.Now;

            _context.PotentialBuys.Update(dataShoe);

            await SaveAsync();
        }

        public async Task<bool> IsShoeSoldByCommentId(int commentId, int userId)
        {
            int potentialBuyId = (await _context.Comments.FirstOrDefaultAsync(c => c.CommentId == commentId && c.UserId == userId)).PotentialBuyId;

            return (await _context.PotentialBuys.FirstOrDefaultAsync(p => p.PotentialBuyId == potentialBuyId)).IsSold;
        }

        public async Task<bool> IsShoeSoldByPotentialBuyId(int id)
        {
            return (await _context.PotentialBuys.FirstOrDefaultAsync(p => p.PotentialBuyId == id)).IsSold;
        }

        public async Task<bool> DeletePotentialBuyById(int id)
        {
            try
            {
                _context.Comments.RemoveRange(await _context.Comments.Where(a => a.PotentialBuyId == id).ToListAsync());
                _context.PotentialBuys.Remove(await _context.PotentialBuys.FindAsync(id));

                await SaveAsync();

                return true;
            } catch (Exception)
            {
                throw;
            }
        }
    }
}
