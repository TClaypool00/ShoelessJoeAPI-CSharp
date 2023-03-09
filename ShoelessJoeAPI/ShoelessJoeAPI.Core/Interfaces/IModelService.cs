using ShoelessJoeAPI.Core.CoreModels;
using ShoelessJoeAPI.Core.CoreModels.PartialModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoelessJoeAPI.Core.Interfaces
{
    public interface IModelService
    {
        public Task<List<CoreModel>> GetModelsAsync(int? userId = null, int? index = null);

        public Task<List<CoreModelDropDown>> GetModelDropDown(int userId, int? index = null);

        public Task<CoreModel> GetModelAsync(int id);

        public Task<CoreModel> AddModelAsync(CoreModel model);

        public Task<CoreModel> UpdateModelAsync(CoreModel model, int id);

        public Task DeleteModelAsync(int id);

        public Task<bool> ModelExistsAsync(int id, int? userId = null);

        public Task<bool> ModelNameExistsAsync(string name, int userId, int? id =  null);
    }
}
