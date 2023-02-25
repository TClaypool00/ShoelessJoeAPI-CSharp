using Microsoft.EntityFrameworkCore;
using ShoelessJoeAPI.Core.CoreModels;
using ShoelessJoeAPI.Core.CoreModels.PartialModels;
using ShoelessJoeAPI.Core.Interfaces;
using ShoelessJoeAPI.DataAccess.DataModels;

namespace ShoelessJoeAPI.DataAccess.Services
{
    public class ManufacterService : IManufacterService
    {
        private readonly ShoelessJoeContext _context;

        public ManufacterService(ShoelessJoeContext context)
        {
            _context = context;
        }

        public async Task<CoreManufacter> AddManufacterAsync(CoreManufacter manufacter)
        {
            var dataManufacter = Mapper.MapManufacter(manufacter);

            await _context.Manufacters.AddAsync(dataManufacter);

            await SaveAsync();

            manufacter.ManufacterId = dataManufacter.ManufacterId;

            return manufacter;
        }

        public async Task DeleteManufacterAsync(int id)
        {
            var dataManufacter = await GetDataManufacter(id);

            _context.Manufacters.Remove(dataManufacter);

            await SaveAsync();
        }

        public async Task<CoreManufacter> GetGetManufacterAsync(int id)
        {
            return Mapper.MapManufacter(await GetDataManufacter(id));
        }

        public async Task<List<CoreManufacter>> GetManufactersAsync(int? userId = null, int? index = null)
        {
            index ??= 1;

            List<Manufacter> manufacters;
            var coreManufacters = new List<CoreManufacter>();

            if (userId is null)
            {
                manufacters = await _context.Manufacters.Select(m => new Manufacter
                {
                    ManufacterId = m.ManufacterId,
                    ManufacterName = m.ManufacterName,
                    User = new User
                    {
                        UserId = m.User.UserId,
                        FirstName = m.User.FirstName,
                        LastName = m.User.LastName
                    }
                })
                .Take(10)
                .Skip((int)index * 10)                
                .ToListAsync();
            } 
            else
            {
                manufacters = await _context.Manufacters.Select(m => new Manufacter
                {
                    ManufacterId = m.ManufacterId,
                    ManufacterName = m.ManufacterName,
                    User = new User
                    {
                        UserId = m.User.UserId,
                        FirstName = m.User.FirstName,
                        LastName = m.User.LastName
                    }
                })
                .Take(10)
                .Skip((int)index * 10)
                .Where(m => m.ManufacterId == userId)
                .ToListAsync();
            }

            if (manufacters.Count > 0)
            {
                for (int i = 0; i < manufacters.Count; i++)
                {
                    coreManufacters.Add(Mapper.MapManufacter(manufacters[i]));
                }
            }

            return coreManufacters;
        }

        public Task<bool> ManufacterExistsById(int id)
        {
            return _context.Manufacters.AnyAsync(m => m.ManufacterId == id);
        }

        public async Task<CoreManufacter> UpdateManufacter(CoreManufacter manufacter, int id)
        {
            var currentManufacter = GetDataManufacter(id);
            var newManufacter = Mapper.MapManufacter(manufacter);

            _context.Entry(currentManufacter).CurrentValues.SetValues(newManufacter);

            await SaveAsync();

            return manufacter;
        }

        private async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }


        private Task<Manufacter> GetDataManufacter(int id)
        {
            return _context.Manufacters
                .Select(m => new Manufacter
                {
                    ManufacterId = m.ManufacterId,
                    ManufacterName = m.ManufacterName,
                    User = new User
                    {
                        UserId = m.User.UserId,
                        FirstName = m.User.FirstName,
                        LastName = m.User.LastName
                    }
                })
                .FirstOrDefaultAsync(m => m.ManufacterId == id);
        }

        public Task<bool> ManufacterExistByName(string name, int userId)
        {
            return _context.Manufacters.AnyAsync(m => m.ManufacterName == name && m.UserId == userId);
        }

        public async Task<List<CoreManufacterDropDown>> GetCoreManufacterDropDown(int userId, int? index = null)
        {
            index ??= 1;

            var coreManufacters = new List<CoreManufacterDropDown>();

            var manufacters = await _context.Manufacters.Select(m => new Manufacter
            {
                ManufacterId = m.ManufacterId,
                ManufacterName = m.ManufacterName,
                UserId = m.UserId
            })
            .Where(u => u.UserId == userId)
            .Take(10)
            .Skip((int)index * 10)
            .ToListAsync();

            for (int i = 0; i < manufacters.Count; i++)
            {
                coreManufacters.Add(Mapper.MapManufacterDropDown(manufacters[i]));
            }

            return coreManufacters;
        }

        public Task<bool> ManufacterExistsByUserId(int id, int userId)
        {
            return _context.Manufacters.AnyAsync(m => m.ManufacterId == id && m.UserId == userId);
        }
    }
}
