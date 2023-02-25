using ShoelessJoeAPI.Core.CoreModels;
using ShoelessJoeAPI.Core.CoreModels.PartialModels;

namespace ShoelessJoeAPI.Core.Interfaces
{
    public interface IManufacterService
    {
        public Task<List<CoreManufacter>> GetManufactersAsync(int? userId = null, int? index = null);

        public Task<List<CoreManufacterDropDown>> GetCoreManufacterDropDown(int userId, int? index = null);

        public Task<CoreManufacter> GetGetManufacterAsync(int id);

        public Task<CoreManufacter> AddManufacterAsync(CoreManufacter manufacter);

        public Task<CoreManufacter> UpdateManufacter(CoreManufacter manufacter, int id);

        public Task DeleteManufacterAsync(int id);

        public Task<bool> ManufacterExistsById(int id);

        public Task<bool> ManufacterExistByName(string name, int userId);

        public Task<bool> ManufacterExistsByUserId(int id, int userId);
    }
}
