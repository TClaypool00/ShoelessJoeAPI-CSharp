using Microsoft.EntityFrameworkCore;
using ShoelessJoeAPI.Core.CoreModels;
using ShoelessJoeAPI.Core.CoreModels.PartialModels;
using ShoelessJoeAPI.Core.Interfaces;
using ShoelessJoeAPI.DataAccess.DataModels;

namespace ShoelessJoeAPI.DataAccess.Services
{
    public class ModelService : ServiceHelper, IModelService
    {
        private readonly ShoelessJoeContext _context;

        public ModelService(ShoelessJoeContext context)
        {
            _context = context;
        }

        public async Task<CoreModel> AddModelAsync(CoreModel model)
        {
            var dataModel = Mapper.MapModel(model);

            await _context.Models.AddAsync(dataModel);

            await SaveAsync();

            model.ModelId = dataModel.ModelId;

            return model;
        }

        public async Task DeleteModelAsync(int id)
        {
            var model = await _context.Models.FindAsync(id);

            _context.Models.Remove(model);

            await SaveAsync();
        }

        public async Task<CoreModel> GetModelAsync(int id)
        {
            var dataModel = await _context.Models
                .Select(m => new Model
                {
                    ModelId = m.ModelId,
                    ModelName = m.ModelName,
                    Manufacter = new Manufacter
                    {
                        ManufacterId = m.Manufacter.ManufacterId,
                        ManufacterName = m.Manufacter.ManufacterName,
                        User = new User
                        {
                            UserId = m.Manufacter.UserId,
                            FirstName = m.Manufacter.User.FirstName,
                            LastName = m.Manufacter.User.LastName
                        }
                    }
                })
                .FirstOrDefaultAsync(a => a.ModelId == id);

            return Mapper.MapModel(dataModel);
        }

        public async Task<List<CoreModelDropDown>> GetModelDropDown(int userId, int? index = null)
        {
            ConfigureIndex(index);
            var coreModels = new List<CoreModelDropDown>();

            var models = await _context.Models
                .Select(m => new Model
                {
                    ModelId = m.ModelId,
                    ModelName = m.ModelName
                })
                .Where(u => u.Manufacter.UserId == userId)
                .Take(10)
                .Skip(_index)
                .ToListAsync();

            if (models.Count > 0)
            {
                for (int i = 0; i < models.Count; i++)
                {
                    coreModels.Add(Mapper.MapModelDropDown(models[i]));
                }
            }

            return coreModels;
        }

        public async Task<List<CoreModel>> GetModelsAsync(int? userId = null, int? index = null)
        {
            ConfigureIndex(index);

            List<Model> models;
            var coreModels = new List<CoreModel>();

            if (userId != null)
            {
                models = await _context.Models
                    .Select(m => new Model
                    {
                        ModelId = m.ModelId,
                        ModelName = m.ModelName,
                        Manufacter = new Manufacter
                        {
                            ManufacterId=m.Manufacter.ManufacterId,
                            ManufacterName = m.Manufacter.ManufacterName,
                            User = new User
                            {
                                UserId = m.Manufacter.User.UserId,
                                FirstName = m.Manufacter.User.FirstName,
                                LastName = m.Manufacter.User.LastName
                            }
                        }
                    })
                    .Take(10)
                    .Skip(_index)
                    .Where(x => x.Manufacter.User.UserId == (int)userId)
                    .ToListAsync();
            }
            else
            {
                models = await _context.Models
                    .Select(m => new Model
                    {
                        ModelId = m.ModelId,
                        ModelName = m.ModelName,
                        Manufacter = new Manufacter
                        {
                            ManufacterId = m.Manufacter.ManufacterId,
                            ManufacterName = m.Manufacter.ManufacterName,
                            User = new User
                            {
                                UserId = m.Manufacter.User.UserId,
                                FirstName = m.Manufacter.User.FirstName,
                                LastName = m.Manufacter.User.LastName
                            }
                        }
                    })
                    .Take(10)
                    .Skip(_index)
                    .ToListAsync();
            }

            if (models.Count > 0)
            {
                CoreManufacter manufacter = null;
                CoreUser user = null;

                for (int i = 0; i < models.Count; i++)
                {
                    coreModels.Add(Mapper.MapModel(models[i], out manufacter, out user));
                }
            }

            return coreModels;
        }

        public Task<bool> ModelExistsAsync(int id, int? userId = null)
        {
            if(userId == null)
            {
                return _context.Models.AnyAsync(m => m.ModelId == id);
            }

            return _context.Models.AnyAsync(m => m.ModelId == id && m.Manufacter.UserId == userId);
        }

        public Task<bool> ModelNameExistsAsync(string name, int userId, int? id = null)
        {
            if (id is null)
            {
                return _context.Models.AnyAsync(m => m.ModelName == name && m.Manufacter.UserId == userId);
            }
            else
            {
                return _context.Models.AnyAsync(m => m.ModelName == name && m.Manufacter.UserId == userId && m.ModelId != id);
            }
        }

        public async Task<CoreModel> UpdateModelAsync(CoreModel model, int id)
        {
            var dataModel = Mapper.MapModel(model);

            _context.Models.Update(dataModel);

            await SaveAsync();

            return model;
        }

        private async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
