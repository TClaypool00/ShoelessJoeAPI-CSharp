using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ShoelessJoeAPI.Core.CoreModels;
using ShoelessJoeAPI.Core.Interfaces;
using ShoelessJoeAPI.DataAccess.DataModels;

namespace ShoelessJoeAPI.DataAccess.Services
{
    public class ShoeService : ServiceHelper, IShoeService
    {
        private readonly ShoelessJoeContext _context;
        private string _filePath;
        private string _picturePath;
        private FileStream _stream;

        public ShoeService(ShoelessJoeContext context)
        {
            _context = context;
        }

        public async Task AddShoeAsync(CoreShoe shoe)
        {
            var dataShoe = Mapper.MapShoe(shoe);

            await _context.Shoes.AddAsync(dataShoe);

            await SaveAsync();

            if (dataShoe.ShoeId == 0)
            {
                throw new ArgumentException(_idErrorMessage);
            }

            _filePath = SecretConfig.PicturePath + $"User{shoe.Model.Manufacter.UserId}";

            CreateDirectory();

            _filePath += $"\\Shoe{dataShoe.ShoeId}";

            CreateDirectory();

            _filePath += "\\";


            try
            {
                if (shoe.ShoeImage.LeftShoeImage1 is not null)
                {
                    _picturePath = GenerateFileName(shoe.ShoeImage.LeftShoeImage1, ShoePicturesTypes.LeftShoe1);
                    using (_stream = new FileStream(CombineFileName(), FileMode.Create))
                    {
                        await shoe.ShoeImage.LeftShoeImage1.CopyToAsync(_stream);
                    }

                    shoe.ShoeImage.LeftShoeImage1Path = _picturePath ;
                }

                if (shoe.ShoeImage.LeftShoeImage2 is not null)
                {
                    _picturePath = GenerateFileName(shoe.ShoeImage.LeftShoeImage2, ShoePicturesTypes.LeftShoe2);
                    using (_stream = new FileStream(CombineFileName(), FileMode.Create))
                    {
                        await shoe.ShoeImage.LeftShoeImage2.CopyToAsync(_stream);
                    }

                    shoe.ShoeImage.LeftShoeImage2Path = _picturePath ;
                }

                if (shoe.ShoeImage.RightShoeImage1 is not null)
                {
                    _picturePath = GenerateFileName(shoe.ShoeImage.RightShoeImage1, ShoePicturesTypes.RightShoe1);
                    using (_stream = new FileStream(CombineFileName(), FileMode.Create))
                    {
                        await shoe.ShoeImage.RightShoeImage1.CopyToAsync(_stream);
                    }

                    shoe.ShoeImage.RightShoeImage1Path = _picturePath;
                }

                if (shoe.ShoeImage.RightShoeImage2 is not null)
                {
                    _picturePath = GenerateFileName(shoe.ShoeImage.RightShoeImage2, ShoePicturesTypes.RightShoe2);
                    using (_stream = new FileStream(CombineFileName(), FileMode.Create))
                    {
                        await shoe.ShoeImage.RightShoeImage2.CopyToAsync(_stream);
                    }
                    
                    shoe.ShoeImage.RightShoeImage2Path= _picturePath;
                }

            } catch (Exception)
            {
                _context.Shoes.Remove(dataShoe);
                await SaveAsync();
                throw;
            }

            var dataShoeImage = Mapper.MapShoeImage(shoe.ShoeImage);
            dataShoeImage.Shoe = dataShoe;

            await _context.ShoeImages.AddAsync(dataShoeImage);

            try
            {
                await SaveAsync();
            } catch (Exception)
            {
                _context.Shoes.Remove(dataShoe);
                await SaveAsync();
                throw;
            }
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

        public Task<bool> ShoeIsOwnedByUserAsync(int id, int ownerId)
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

        private void CreateDirectory()
        {
            if (!Directory.Exists(_filePath))
            {
                Directory.CreateDirectory(_filePath);
            }
        }

        private static string GenerateFileName(IFormFile file, ShoePicturesTypes type)
        {
            return Path.GetFileNameWithoutExtension(file.FileName) + "-" + Guid.NewGuid().ToString() + "-" + type.ToString() + Path.GetExtension(file.FileName);
        }

        private string CombineFileName()
        {
            return _filePath + _picturePath;
        }

        public enum ShoePicturesTypes
        {
            LeftShoe1,
            LeftShoe2,
            RightShoe1,
            RightShoe2
        }
    }
}
